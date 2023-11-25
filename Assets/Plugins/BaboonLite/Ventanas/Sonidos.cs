using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace BaboOnLite
{
    public class Sonidos : EditorWindow
    {
        //VARIABLES 

        //Publicas
        [SerializeField] private DictionaryBG<AudioClip> sonidosLocal = new();
        [SerializeField] private DictionaryBG<AudioClip> musicaLocal = new();
        [SerializeField] private AutoPlay[] autoPlay;

        //Estaticas
        [SerializeField] private static DictionaryBG<AudioClip> sonidos = new();
        [SerializeField] private static DictionaryBG<AudioClip> musica = new();
        private static List<SonidosCreados> musicaCreada = new();
        private static List<SonidosCreados> sonidoCreado = new();

        private static Transform padre;

        //Privadas
        private bool play, autoPlayActivo, botones;
        private string botonSonido;
        private Vector2 scroll = Vector2.zero;
        private SerializedObject serializedObject;

        //Referencias
        public Dictionary<string, Sonido> sonido { get => Save.Data.sonido; set => Save.Data.sonido = value; }

        [MenuItem("Window/BaboOnLite/Sonidos")]
        public static void IniciarVentana()
        {
            Sonidos ventana = GetWindow<Sonidos>("Sonidos");
            ventana.minSize = new Vector2(200, 200);

            //Dependencia
            Save dependecia = GetWindow<Save>("Save");
            dependecia.minSize = new Vector2(200, 200);
        }

        private void OnGUI()
        {
            //Crea la GUI basica de la ventana

            //Inicio del GUI
            GUILayout.Label("Sonidos: Administra los sonidos de tu juego facilmente", EditorStyles.boldLabel);
            scroll = EditorGUILayout.BeginScrollView(scroll);

            //Administra el volumen y el estado de la vibracion, musica y sonidos
            #region sonidos volumen/estado
            EditorGUILayout.Space(10);

            GUILayout.Label("Vibracion: ", EditorStyles.boldLabel);
            sonido["vibracion"].estado = EditorGUILayout.Toggle("Activo: ", sonido["vibracion"].estado);

            EditorGUILayout.Space(10);

            Volumen("Musica", musicaCreada);

            EditorGUILayout.Space(10);

            Volumen("Sonidos", sonidoCreado);

            EditorGUILayout.Space(10);

            #endregion

            Separador();

            //Lista de musicas
            #region musica
            EditorGUILayout.Space(10);

            autoPlayActivo = EditorGUILayout.Toggle("Reproductor automatico: ", autoPlayActivo);
            if (autoPlayActivo)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("autoPlay"));
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUILayout.Space(10);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("musicaLocal"));
            if (serializedObject.ApplyModifiedProperties())
            {
                musica = musicaLocal;
            }

            EditorGUILayout.Space(10);
            #endregion

            Separador();

            //Lista de sonidos
            #region sonidos

            //Sonido de botones
            EditorGUILayout.Space(10);

            botones = EditorGUILayout.Toggle("Sonido en botones: ", botones);
            if (botones)
            {
                botonSonido = GUILayout.TextField(botonSonido);
            }

            EditorGUILayout.Space(10);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("sonidosLocal"));
            if (serializedObject.ApplyModifiedProperties())
            {
                sonidos = sonidosLocal;
            }
            EditorGUILayout.Space(10);
            #endregion

            EditorGUILayout.EndScrollView();
        }

        private void OnEnable()
        {
            //Declarar el serializedObject
            serializedObject = new SerializedObject(this);

            //Detecta cuando entras en el playmode
            #region play
            EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
            {
                if (state == PlayModeStateChange.ExitingEditMode)
                {
                    play = true;
                }
                if (state == PlayModeStateChange.ExitingPlayMode)
                {
                    sonidoCreado = sonidoCreado.Filter((elemento) => elemento.inmortal);
                    musicaCreada = musicaCreada.Filter((elemento) => elemento.inmortal);
                }
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    //Reproduce automaticamente la musica
                    if (autoPlayActivo)
                    {
                        foreach (var musica in autoPlay)
                        {
                            if (EditorSceneManager.GetActiveScene().path == AssetDatabase.GetAssetPath(musica.escena))
                            {
                                GetMusica(musica.musica, false, true);
                                return;
                            }
                        }
                    }
                    //Asigna a los botones sonidos
                    if (botones)
                    {
                        Button[] botones = FindObjectsOfType<Button>(includeInactive: true);
                        foreach (Button boton in botones)
                        {
                            boton.onClick.AddListener(() => { GetSonido(botonSonido); });
                        }
                    }
                }
            };

            if (play)
            {
                //Asigna las variables
                sonidos = sonidosLocal;
                musica = musicaLocal;         
            }
            #endregion
        }

        //Metodos para cambiar el volumen y los estados
        #region sonidos volumen/estado
        public static void Estado(string nombre, bool estado)
        {
            if (!sonidos.Inside(nombre))
            {
                Bug.LogLite($"[BL][Sonidos: 4] No existe ningun elemento con el nombre {nombre}. Elija: musica, sonidos o vibracion");
                return;
            }
            Save.Data.sonido[nombre].estado = estado;
        }
        public static void Volumen(string nombre, int volumen)
        {
            if (!sonidos.Inside(nombre))
            {
                Bug.LogLite($"[BL][Sonidos: 4] No existe ningun elemento con el nombre {nombre}. Elija: musica o sonidos");
                return;
            }
            if (volumen < 0)
            {
                volumen = 0;
                Bug.LogLite("[BL][Sonidos: 1] El volumen no puede ser menor que 0");
            }
            else if (volumen < 0)
            {
                volumen = 100;
                Bug.LogLite("[BL][Sonidos: 2] El volumen no puede ser mayor que 100");
            }
            Save.Data.sonido[nombre].volumen = volumen;
        }
        #endregion

        //Metodos para instanciar un sonido o vibracion
        #region instanciar
        public static void GetVibracion()
        {
            if (Save.Data.sonido["vibracion"].estado)
            {
                if (SystemInfo.supportsVibration)
                {
                    Handheld.Vibrate();
                    return;
                }
                Debug.Log("El dispositivo actual no soporta la vibracion");
            }
        }
        public static AudioSource GetSonido(string nombre, bool inmortal = false, bool bucle = false)
        {
            int volumen = Save.Data.sonido["sonidos"].volumen;
            if (!Save.Data.sonido["sonidos"].estado)
            {
                volumen = 0;
            }
            AudioSource audio = Creador(sonidos, nombre, volumen, inmortal, bucle);
            sonidoCreado.Add(new(audio, inmortal));
            return audio;
        }
        public static AudioSource GetMusica(string nombre, bool inmortal = false, bool bucle = false)
        {
            int volumen = Save.Data.sonido["musica"].volumen;
            if (!Save.Data.sonido["musica"].estado)
            {
                volumen = 0;
            }
            AudioSource audio = Creador(musica, nombre, volumen, inmortal, bucle);
            musicaCreada.Add(new(audio, inmortal));
            return audio;
        }
        private static AudioSource Creador(DictionaryBG<AudioClip> lista, string nombre, int volumen, bool inmortal, bool bucle)
        {
            //Comprueba que exista el audio
            if (!lista.Inside(nombre))
            {
                //Ese sonido no esta dentro del array
                Bug.LogLite($"[BL][Sonidos: 3] No existe el sonido {nombre} dentro de Sounds");
                return null;
            }
            //Crea un contenedor padre
            if (padre == null)
            {
                padre = new GameObject($"Sonidos").transform;
                padre.position = Vector3.zero;
            }

            //Instancia el audio
            GameObject instancia = new GameObject($"Sonido-{nombre}");
            instancia.transform.position = Vector3.zero;
            instancia.transform.SetParent(padre);

            //Crea el audioi source
            AudioSource audioSource = instancia.AddComponent<AudioSource>();
            audioSource.clip = lista.Get(nombre);
            audioSource.volume = ((float)volumen / 100);

            //Le da la inmortalidad entre escena
            if (inmortal) DontDestroyOnLoad(instancia);

            //Lo activa en bucle
            if (bucle) audioSource.loop = true;
            else Destroy(instancia, lista.Get(nombre).length);

            //Lo activa y devuelve
            audioSource.Play();
            return audioSource;
        }
        #endregion

        //Metodos para añadir diseños al gui
        #region gui diseño

        private void Volumen(string nombre, List<SonidosCreados> lista) 
        {
            GUILayout.Label(nombre, EditorStyles.boldLabel);
            nombre = nombre.ToLower();

            bool estado = sonido[nombre].estado;
            int volumen = sonido[nombre].volumen;

            sonido[nombre].estado = EditorGUILayout.Toggle("Activo: ", sonido[nombre].estado);
            sonido[nombre].volumen = EditorGUILayout.IntSlider("Volumen: ", sonido[nombre].volumen, 0, 100);

            //Comprueba si los valores se han modificado
            if (estado != sonido[nombre].estado)
            {
                foreach (var elemento in lista)
                {
                    //Se mutea o desmutea
                    elemento.sonido.volume = (!sonido[nombre].estado)
                    ? 0
                    : ((float)sonido[nombre].volumen / 100);
                }
            }
            if (volumen != sonido[nombre].volumen)
            {
                foreach (var elemento in lista) 
                {
                    elemento.sonido.volume = ((float)sonido[nombre].volumen / 100);
                }
            }
        }
        private void Separador(int altura = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, altura);
            rect.height = altura;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }
        #endregion
    }
}
