using UnityEngine;
using UnityEngine.Advertisements;

namespace BaboOnLite
{
    [DefaultExecutionOrder(0)]
    [AddComponentMenu("BaboOnLite/_ADS/Inicializar")]
    [DisallowMultipleComponent]
    [HelpURL("https://docs.google.com/document/d/1zPv7QP-ZyisadG5zREiMmzV7UWsYTUPZIPT0f_YlhSE/edit?usp=sharing")]
    public class Inicializar : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField] string androidID;
        [SerializeField] string iosID;
        [SerializeField] bool testMode = true;
        private string gameID;

        //Inicializa los anuncios
        private void Awake()
        {
            #if UNITY_IOS
                _gameId = iosID;
            #elif UNITY_ANDROID
                gameID = androidID;
            #elif UNITY_EDITOR
                gameID = androidID;
            #endif

            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(gameID, testMode, this);
            }
        }

        //FUNCIONES DE UNITYADS

        //Mensajes de informacion
        #region mensajes
        public void OnInitializationComplete()
        {
            //Debug.Log("Anuncios cargados.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string mensaje)
        {
            Debug.Log($"Error al cargar los anuncios: {error} - {mensaje}");
        }
        #endregion
    }
}
