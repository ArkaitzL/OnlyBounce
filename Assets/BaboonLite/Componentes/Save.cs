using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaboOnLite
{
    [DefaultExecutionOrder(0)]
    [AddComponentMenu("BaboOnLite/Save")]
    [DisallowMultipleComponent]
    [HelpURL("https://www.notion.so/BaboOnLite-c6252ac92bbc4f8ea231b1276008c13a?pvs=4")]
    public class Save : MonoBehaviour
    {
        //VARIABELS
        [SerializeField] private SaveScript data = new SaveScript();
        [HideInInspector] public bool mensajes = false;

        //ESTATICA
        [HideInInspector] public static SaveScript Data { get => miInstancia.data; set => miInstancia.data = value; }
        private static Save miInstancia;

        // Recoge los datos y los carga desde PlayerPrefs
        private void Awake()
        {
            // Convierte el script en Singleton
            Instanciar<Save>.Singletons(this, gameObject);
            miInstancia = Instanciar<Save>.Coger();

            //Carga los datos
            Cargar();
        }

        //Carga los datos si los hay
        #region cargar_datos
        public void Cargar()
        {
            string jsonString = PlayerPrefs.GetString("data");
            if (!string.IsNullOrEmpty(jsonString))
            {
                data = JsonUtility.FromJson<SaveScript>(jsonString);
                if (mensajes)
                {
                    Debug.Log("[BL]Datos cargados correctamente");
                }
            }
            else
            {
                // No se ha encontrado ningún dato en PlayerPrefs
                Bug.LogLite("[BL][Save: 1] No se han encontrado datos guardados");
            }
        }
        #endregion

        // Al salir de la aplicación, guarda los datos en PlayerPrefs
        #region guardar_datos
        private void OnApplicationPause(bool pause)
        {
            if (pause) Guardar();
        }
        private void OnApplicationQuit()
        {
            Guardar();
        }
        public void Guardar()
        {
            string jsonString = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("data", jsonString);

            if (mensajes)
            {
                Debug.Log("[BL]Datos guardados en PlayerPrefs");
            }
        }
        #endregion

        // Elimina los datos de PlayerPrefs
        #region eliminar_datos
        public void Eliminar()
        {
            PlayerPrefs.DeleteKey("data");
            if (mensajes) Debug.Log("[BL]Datos eliminados correctamente de PlayerPrefs");
        }
        #endregion
    }
}
