using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

[System.Serializable]
public partial class SaveScript
{
    //----------------------------------------------------------------//
    //Variables por defecto: Estas varibles se usan automaticamente   //
    //----------------------------------------------------------------//
    //public int lenguaje = 0; // Lenguaje
    //public Dictionary<string, Sonido> sonido = new(){ //Sonidos
    //    { "vibracion", new() },
    //    { "musica", new() },
    //    { "sonidos", new() },
    //};
    public int dinero = 0;
    public int mejor_puntuacion;
    public bool iniciado;
    public bool muteado;

    //Skin
    public List<int> listaSkin = new();
    public miSkin2D miSkin2D; //Skin2D
    
}