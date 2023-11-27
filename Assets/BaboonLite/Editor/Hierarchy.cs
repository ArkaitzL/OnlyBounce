using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BaboOnLite;

[InitializeOnLoad]
public static class Hierarchy
{
    //Se suscribe al evento del Hierarchy
    static Hierarchy() => EditorApplication.hierarchyWindowItemOnGUI += (int id, Rect seccion) =>
    {
        //VARIABLES
        //Coge los objetos del hierarchy
        GameObject gameObject = EditorUtility.InstanceIDToObject(id) as GameObject;
        //Coge los colores de la lista
        List<(string, Color)> color = new ColorMark().colores;

        //Pos X y longitud
        float posX = 28f, width = 5f;

        //Aplica los colores necesarios
        #region aplicar
        if (gameObject != null)
        {
            color.ForEach(elemento =>
            {
                (string caracter, Color color) = elemento;

                if (gameObject.name.Contains(caracter))
                {
                    Rect colorRect = new Rect(seccion);
                    colorRect.x -= posX;
                    colorRect.width = width;

                    EditorGUI.DrawRect(colorRect, color);
                }
            });
        }
        #endregion
    };
}