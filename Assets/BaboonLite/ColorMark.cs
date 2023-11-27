using System.Collections.Generic;
using UnityEngine;

namespace BaboOnLite
{
    public class ColorMark 
    {
        public List<(string, Color)> colores = new List<(string, Color)>() {
            ("!", Color.red),
            ("?", Color.blue),
            ("*", Color.yellow),
            ("-", Color.black),
            ("$", Color.cyan),
            ("_", Color.white),
            ("|", Color.green),
        };
    }
}
