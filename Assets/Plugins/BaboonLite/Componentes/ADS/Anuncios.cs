using UnityEngine.Advertisements;
using UnityEngine;
using System;

namespace BaboOnLite {

    [DefaultExecutionOrder(1)]
    [AddComponentMenu("BaboOnLite/_ADS/Anuncios")]
    [DisallowMultipleComponent]
    [HelpURL("https://docs.google.com/document/d/1zPv7QP-ZyisadG5zREiMmzV7UWsYTUPZIPT0f_YlhSE/edit?usp=sharing")]
    public class Anuncios : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        //PUBLICAS
        [Header("Interstitial")]
        [SerializeField] private string androidInterstitial = "Interstitial_Android";
        [SerializeField] private string iosInterstitial = "Interstitial_iOS";
        [Header("Rewarded")]
        [SerializeField] private string androidRewarded = "Rewarded_Android";
        [SerializeField] private string iosRewarded = "Rewarded_iOS";
        //ESTATICAS
        public static Action<Action> verRewarded;
        public static Action verInterstitial;
        //PRIVADAS
        private Action recompensa = null;
        private string rewarded, interstitial;

        //Asigna el id correspondiente
        private void Awake()
        {
            //Declara los delegados
            verInterstitial = VerInterstitial;
            verRewarded = VerRewarded;

            //Asigna el id correspondiente
            interstitial = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? iosInterstitial
                : androidInterstitial;
            rewarded = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? iosRewarded
                : androidRewarded;
        }

        //Enseña el anuncio que quieres ver
        public void VerInterstitial()
        {
            Advertisement.Load(interstitial, this);
        }
        public void VerRewarded()
        {
            Advertisement.Load(rewarded, this);
        }
        public void VerRewarded(Action recompensa)
        {
            this.recompensa = recompensa;
            Advertisement.Load(rewarded, this);
        }

        //FUNCIONES DE UNITYADS

        //Avisa cuando esta todo cargado
        #region cargado
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            //Comprueba que este todo cargado
            if (!Advertisement.isInitialized)
            {
                Bug.LogLite("[BL][Anuncio: 1] No estan inicializados los anuncios");
                return;
            }

            Advertisement.Show(adUnitId, this);
        }
        #endregion

        //Errores al cargar o mostrar los anuncios
        #region errores
        public void OnUnityAdsFailedToLoad(string id, UnityAdsLoadError error, string mensaje)
        {
            Bug.LogLite($"[BL][Anuncio: 2] No se ha podido cargar el anuncio: \n {id} \n {error} \n {mensaje}");
        }
        public void OnUnityAdsShowFailure(string id, UnityAdsShowError error, string mensaje)
        {
            Bug.LogLite($"[BL][Anuncio: 3] No se ha podido mostrar el anuncio: \n {id} \n {error} \n {mensaje}");
        }
        #endregion

        //Otras opciones para los anuncios
        #region mas opciones
        // Se llama cuando comienza la reproducción del anuncio
        public void OnUnityAdsShowStart(string _adUnitId) { }
        // Se llama cuando el usuario hace clic en el anuncio
        public void OnUnityAdsShowClick(string _adUnitId) { }
        // Se llama cuando se completa la reproducción del anuncio
        public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState) {
            recompensa?.Invoke();
            recompensa = null;
        }
        #endregion
    }

}