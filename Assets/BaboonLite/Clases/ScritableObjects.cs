using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BaboOnLite {

    [CreateAssetMenu(fileName = "Lenguaje", menuName = "BaboOnLite/Lenguaje")]
    public class Lenguaje : ScriptableObject
    {
        //Almacena las palabras del idioma
        public string[] dictionary = new string[0];

        //Copia el diccionario
        #region copiar
        public void Copiar()
        {
            GUIUtility.systemCopyBuffer = dictionary.inString();
            Debug.Log("Idioma copiado en el portapapeles");
        }
        #endregion
        //Crea un nuevo diccionario como el que le pasas
        #region pegar
        public void Pegar()
        {
            dictionary = dictionary.Concat(
                GUIUtility.systemCopyBuffer.inArray<string>()
             ).ToArray();
        }
        #endregion

        //Añadir datos al diccionario con el que le pasas
        #region pegar como nuevo
        public void PegarComoNuevo()
        {
            dictionary = GUIUtility.systemCopyBuffer.inArray<string>();
        }
        #endregion
    }
}