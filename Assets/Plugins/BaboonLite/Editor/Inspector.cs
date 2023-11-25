using UnityEditor;
using UnityEngine;

namespace BaboOnLite {
    //Editor del  scriptableObject lenguaje
    #region lenguaje
    [CustomEditor(typeof(Lenguaje))]
    public class LenguajeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Lenguaje lang = (Lenguaje)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Copiar")) lang.Copiar();
            if (GUILayout.Button("Pegar")) lang.Pegar();
            if (GUILayout.Button("Pegar como nuevo")) lang.PegarComoNuevo();
        }
    }
    #endregion

    [CustomEditor(typeof(DictionaryBG<>))]
    public class DictionaryBGEditor : Editor
    {
        public override void OnInspectorGUI()
        {
           
        }
    }
}
