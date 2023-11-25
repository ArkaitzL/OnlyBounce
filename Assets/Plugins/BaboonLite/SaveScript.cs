using System.Collections.Generic;
using UnityEngine;
namespace BaboOnLite
{
    [System.Serializable]
    public partial class SaveScript
    {
        //----------------------------------------------------------------//
        //Variables por defecto: Estas varibles se usan automaticamente   //
        //----------------------------------------------------------------//
        public int lenguaje = 0; // Lenguaje
        public Dictionary<string, Sonido> sonido = new(){ //Sonidos
            { "vibracion", new() },
            { "musica", new() },
            { "sonidos", new() },
        };
        //Skins
        public int dinero = 0;
        public List<int> listaSkin = new();
        public miSkin2D miSkin2D; //Skin2D
    }
}
