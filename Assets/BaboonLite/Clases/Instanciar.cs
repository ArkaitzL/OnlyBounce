using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace BaboOnLite 
{
    public class Instanciar<T> : MonoBehaviour
    {
        //VARIABLES
        private static Dictionary<Type, Info> instancias = new Dictionary<Type, Info>();
        private static bool suscrito;

        //Añade una clase al diccionario
        #region añadir
        public static void Añadir(T instancia)
        {
            //Cuando cambia de escena elimina las instancias locales
            if (!suscrito)
            {
                SceneManager.sceneUnloaded += (Scene escena) =>
                {
                    instancias = instancias.Where(
                        valor => valor.Value.multi
                    ).ToDictionary(
                        valor => valor.Key, valor => valor.Value
                    );
                    suscrito = true;
                };
            }

            Type tipo = typeof(T);

            //Detecta si ya existe un elemento de ese tipo
            if (instancias.ContainsKey(tipo))
            {
                Bug.LogLite("[BL][Instanciar: 1] Ya tienes un objeto de ese tipo");
                return;
            }

            //Añade el elemento
            instancias.Add(tipo, new Info(instancia));
        }
        #endregion

        //Te devuelve la clase del T
        #region coger
        public static T Coger()
        {
            Type tipo = typeof(T);

            //Devuelve la instancia 
            if (instancias.ContainsKey(tipo))
            {
                return instancias[tipo].instancia;
            }

            //Si no existe manda un mensaje para avisar
            Bug.LogLite("[BL][Instanciar: 2] No existe ninguna clase con ese tipo");
            return default(T);
        }
        #endregion

        //Crea un singleton
        #region singletons

        public static void Singletons(T instancia, GameObject gameObject)
        {
            Type tipo = typeof(T);
            if (instancias.ContainsKey(tipo))
            {
                Destroy(gameObject);
                return;
            }

            instancias.Add(tipo, new Info(instancia, true));
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        //Clase para almacenar la Info que vas a guardar
        struct Info
        {
            public T instancia;
            public bool multi;

            public Info(T instancia, bool multi = false)
            {
                this.instancia = instancia;
                this.multi = multi;
            }
        }
    }
}