using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace BaboOnLite
{
    public static class ArrayFun
    {
        //Funcion para coger un elemento de un IEnumerable
        internal static T Get<T>(this IEnumerable<T> array, int i) => array.ToArray()[i];

        //Convierte el texto en array
        #region inArray/inList
        private static IEnumerable<T> _Array<T>(string texto)
        {
            string subtexto = texto.Substring(
                texto.IndexOf("[") + 1,
                texto.IndexOf("]") - texto.IndexOf("[") - 1
            );

            List<T> array = new List<T>();
            foreach (string s in subtexto.Split(','))
            {
                try
                {
                    T elemento = (T)Convert.ChangeType(s.Trim(), typeof(T));
                    array.Add(elemento);
                }
                catch (FormatException)
                {
                    Bug.LogLite("[BL][Funciones: 1] No se a podido convertir en Array/Lista");
                }
            }

            return array;
        }
        //Funciones de llamada
        public static T[] inArray<T>(this string texto) => _Array<T>(texto).ToArray();
        public static List<T> inList<T>(this string texto) => _Array<T>(texto).ToList();
        #endregion

        //Convierte de array a textoo
        #region inString
        public static string inString<T>(this IEnumerable<T> array)
        {
            string texto = "[";
            for (int i = 0; i < array.Count(); i++)
            {
                texto += (i != array.Count() - 1)
                    ? $"{array.Get(i)},"
                    : $"{array.Get(i)}";
            }
            return texto + "]";
        }
        #endregion

        //Bucle de array
        #region foreach
        public static void ForEach<T>(this IEnumerable<T> array, Action<T> func)
        {
            for (int i = 0; i < array.Count(); i++)
            {
                func(array.Get(i));
            }
        }
        public static void ForEach<T>(this IEnumerable<T> array, Action<T, int> func)
        {
            for (int i = 0; i < array.Count(); i++)
            {
                func(array.Get(i), i);
            }
        }
        #endregion

        //Devuelve true si todas las condiciones son correctas
        #region every
        public static bool Every<T>(this IEnumerable<T> array, Func<T, bool> func)
        {
            foreach (T elemento in array)
            {
                if (!func(elemento)) return false;
            }
            return true;
        }
        #endregion

        //Devuelve true si alguna condicion es correcta
        #region some
        public static bool Some<T>(this IEnumerable<T> array, Func<T, bool> func)
        {
            foreach (T elemento in array)
            {
                if (func(elemento)) return true;
            }
            return false;
        }
        #endregion

        //Devuelve los elementos que cumplan la condicion
        #region filter
        private static IEnumerable<T> _Filter<T>(IEnumerable<T> array, Func<T, bool> func)
        {
            List<T> resultado = new List<T>();
            for (int i = 0; i < array.Count(); i++)
            {
                if (func(array.Get(i))) resultado.Add(array.Get(i));
            }
            return resultado;
        }
        //Funciones de llamada
        public static T[] Filter<T>(this T[] array, Func<T, bool> func) => _Filter(array, func).ToArray();
        public static List<T> Filter<T>(this List<T> array, Func<T, bool> func) => _Filter(array, func).ToList();
        #endregion

        //Devuelve el array modificado
        #region map
        public static IEnumerable<T2> _Map<T1, T2>(IEnumerable<T1> array, Func<T1, T2> func)
        {
            List<T2> resultado = new List<T2>();
            for (int i = 0; i < array.Count(); i++)
            {
                resultado.Add(func(array.Get(i)));
            }
            return resultado;
        }
        //Funciones de llamada
        public static T2[] Map<T1, T2>(this T1[] array, Func<T1, T2> func) => _Map(array, func).ToArray();
        public static List<T2> Map<T1, T2>(this List<T1> array, Func<T1, T2> func) => _Map(array, func).ToList();
        #endregion

        //Te dice si el numero del elemeno existe
        #region inside
        public static bool Inside<T>(this IEnumerable<T> array, int valor)
        {
            if (valor >= 0 && valor < array.Count())
            {
                return true;
            }
            return false;
        }
        #endregion

    }

    public static class Bug
    {
        //Muestra un log del string y lo devuelve
        #region log
        public static T Log<T>(this T texto, string otro = null)
        {
            string convertedtexto = Convert.ToString(texto);
            string otrotexto = otro != null ? $"{otro}" : "";
            Debug.Log($"{otrotexto} {convertedtexto}");

            return texto;
        }
        #endregion

        //Muestra un log con informacion basica
        #region [bug]log
        public static void Log(Color color = default)
        {
            string mensaje = "<b>**-------**</b>";
            string colorHex = ColorUtility.ToHtmlStringRGBA((color == default) ? Color.white : color);

            Debug.LogFormat("<color=#{0}>{1}</color>", colorHex, mensaje);
        }
        #endregion

        //Muestra un log del error de BaboonLite - PARA USO DE LA LIBRERIA
        #region [bug]logLite
        //Muestra un log del error de BaboonLite
        public static void LogLite(string texto)
        {
            string colorHex = ColorUtility.ToHtmlStringRGBA(Color.green);
            Debug.LogErrorFormat("<color=#{0}>{1}</color>", colorHex, texto);
        }
        #endregion

    }

    public static class Numeros {

        //Te suma o resta un valor a tu variable poniendole un limite
        #region equacionLimitada
        public static float EquacionLimitada(this float variable, float valor, float limite)
        {
            variable += valor;
            if (variable > limite)
            {
                variable -= limite + 1;
            }
            if (variable < 0)
            {
                variable += limite + 1;
            }

            return variable;
        }
        #endregion
    }

    public static class Transformar {

        //Te permite modificar unicamente el valor X, Y o Z de un Vector3
        #region transform
        public static Vector3 Y(this Vector3 trans, float num)
        {
            return new Vector3(trans.x, trans.y + num, trans.z);
        }
        public static Vector3 X(this Vector3 trans, float num)
        {
            return new Vector3(trans.x + num, trans.y, trans.z);
        }
        public static Vector3 Z(this Vector3 trans, float num)
        {
            return new Vector3(trans.x, trans.y, trans.z + num);
        }
        public static Vector2 Y(this Vector2 trans, float num)
        {
            return new Vector2(trans.x, trans.y + num);
        }
        public static Vector2 X(this Vector2 trans, float num)
        {
            return new Vector2(trans.x + num, trans.y);
        }
        #endregion

        //Te permite modificar unicamente el valor X, Y o Z de un Quaternion
        #region quaternion
        public static Quaternion Y(this Quaternion trans, float num)
        {
            num = trans.eulerAngles.y.EquacionLimitada(num, 360);
            return Quaternion.Euler(trans.eulerAngles.x, num, trans.eulerAngles.z);
        }
        public static Quaternion X(this Quaternion trans, float num)
        {
            num = trans.eulerAngles.x.EquacionLimitada(num, 360);
            return Quaternion.Euler(num, trans.eulerAngles.y, trans.eulerAngles.z);
        }
        public static Quaternion Z(this Quaternion trans, float num)
        {
            num = trans.eulerAngles.z.EquacionLimitada(num, 360);
            return Quaternion.Euler(trans.eulerAngles.x, trans.eulerAngles.y, num);
        }
        #endregion
    }

    public static class Hijos {

        //te permite coger los hijo de los hijos...
        #region get childs
        public static Transform GetChilds(this Transform elemento, params int[] hijos) => Childs(elemento, hijos);
        public static Transform GetChilds(this GameObject elemento, params int[] hijos) => Childs(elemento.transform, hijos);
        private static Transform Childs(Transform elemento, int[] hijos)
        {
            Transform resultado = elemento;

            foreach (int hijo in hijos)
            {
                resultado = resultado.GetChild(hijo);
            }

            return resultado;
        }
        #endregion
    }

}