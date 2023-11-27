
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using BaboOnLite;

public class Banner : MonoBehaviour
{
    [SerializeField] BannerPosition posicion = BannerPosition.BOTTOM_CENTER;

    [SerializeField] string andoridId = "Banner_Android";
    [SerializeField] string iosId = "Banner_iOS";
    string id = null;

    void Start()
    {
        //Asigna el id correspondiente
        #if UNITY_IOS
            id = iosId;
        #elif UNITY_ANDROID
            id = andoridId;
        #endif

        //Establece la posicion
        Advertisement.Banner.SetPosition(posicion);

        //Carga el banner
        LoadBanner();
    }

    public void LoadBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        //Carga el banner
        Advertisement.Banner.Load(id, options);
    }

    #region errores
    private void OnBannerError(string message)
    {
        Bug.LogLite($"[BL][Banner: 1] No se ha podido cargar el banner: \n {message}");
    }
    #endregion

    #region mas opciones
    private void OnBannerClicked() { }
    private void OnBannerShown() { }
    private void OnBannerHidden() { }
    private void OnBannerLoaded() { }
    #endregion

}
