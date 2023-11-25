using UnityEditor;

namespace BaboOnLite {
    public class misPrefabs
    {
        //----------------------------------------------------------------//
        // En MenuItem añade: "GameObject/UI/BaboOnLite/" + "TuCarpeta"   //
        // En ElementoCanvas añade: "TuCarpeta/" + "TuPrefab.prefab"      //
        //----------------------------------------------------------------//

        //CANVAS
        [MenuItem("GameObject/UI/BaboOnLite/Canvas")]
        private static void InstanciarCanva(MenuCommand menuCommand)
        {
            Prefabs.ElementoCanvas(null);
        }
        //FPS
        [MenuItem("GameObject/UI/BaboOnLite/Fps (Android)")]
        private static void InstanciarFps(MenuCommand menuCommand)
        {
            Prefabs.ElementoCanvas("Fps/Fps.prefab");
        }
        //SKINS2D
        [MenuItem("GameObject/UI/BaboOnLite/Skin (2D)")]
        private static void InstanciarSkin2D(MenuCommand menuCommand)
        {
            Prefabs.ElementoCanvas("Menus/Skin2D/Skins2D.prefab");
        }
    }
}
