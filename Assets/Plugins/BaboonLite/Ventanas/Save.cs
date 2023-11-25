using System;
using UnityEditor;
using UnityEngine;

namespace BaboOnLite {
    public class Save : EditorWindow
    {
        //VARIABLES
        [SerializeField] private bool mensajes;

        public static event Action actualizar;
        private Vector2 scroll1 = Vector2.zero;
        private bool play;

        //Variables que almacenan la data
        #region data
        [SerializeField] private static SaveScript data = new();
        [SerializeField] private SaveScript dataLocal = new();

        public static SaveScript Data
        {
            get
            {
                actualizar?.Invoke();
                return data;
            }
            set
            {
                actualizar?.Invoke();
                data = value;
            }
        }
        #endregion


        [MenuItem("Window/BaboOnLite/Save")]
        public static void IniciarVentana()
        {
            Save ventana = GetWindow<Save>("Save");
            ventana.minSize = new Vector2(200, 200);
        }

        private void OnGUI()
        {
            //Crea la GUI basica de la ventana
            #region gui

            GUILayout.Label("Save: Guarda tus datos de manera facil y comoda", EditorStyles.boldLabel);

            EditorGUILayout.Space(10);
            mensajes = EditorGUILayout.Toggle("Mostrar mensajes: ", mensajes);
            EditorGUILayout.Space(10);

            Separador();

            //Eliminar data
            if (GUILayout.Button("Eliminar data"))
            {
                PlayerPrefs.DeleteKey("data");
                data = new();
                dataLocal = new();
                Repaint();

                if (mensajes) Debug.Log("[BL]Datos eliminados correctamente de PlayerPrefs");
            }
            //Actualizar data
            if (GUILayout.Button("Actualizar data"))
            {
                Actualizar();
            }

            #endregion

            //Imprime el contenido de data
            #region data

            scroll1 = EditorGUILayout.BeginScrollView(scroll1);

            //Imprime la data
            SerializedObject objeto = new SerializedObject(this);
            SerializedProperty contenido = objeto.FindProperty("dataLocal");
            EditorGUILayout.PropertyField(contenido, true);

            EditorGUILayout.EndScrollView();
            #endregion
        }
        private void OnEnable()
        {
            #region cargar
            if (play)
            {
                //Se suscribe al evento
                actualizar += Actualizar;

                //Cargar los datos al iniciar el play mode
                string jsonString = PlayerPrefs.GetString("data");
                if (!string.IsNullOrEmpty(jsonString))
                {
                    data = JsonUtility.FromJson<SaveScript>(jsonString);
                    if (mensajes) Debug.Log("[BL]Datos cargados correctamente");
                }
                else
                {
                    // No se ha encontrado ningún dato en PlayerPrefs
                    Bug.LogLite("[BL][Save: 1] No se han encontrado datos guardados");
                }
            }
            #endregion

            EditorApplication.playModeStateChanged += (PlayModeStateChange state) => {
                //Guarda los datos al salir del play mode
                #region guardar
                if (state == PlayModeStateChange.ExitingPlayMode)
                {
                    string jsonString = JsonUtility.ToJson(data);
                    PlayerPrefs.SetString("data", jsonString);

                    if (mensajes) Debug.Log("[BL]Datos guardados en PlayerPrefs");
                    play = false;

                }
                #endregion
                //Detecta cuando entras en el playmode
                #region play
                if (state == PlayModeStateChange.ExitingEditMode)
                {
                    play = true;
                }
                #endregion
            };
        }

        private void Actualizar() {
            dataLocal = data;
            Repaint();
        }

        //Metodos para añadir diseños al gui
        #region gui diseño
        private void Separador(int altura = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, altura);
            rect.height = altura;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }
        #endregion
    }
}
