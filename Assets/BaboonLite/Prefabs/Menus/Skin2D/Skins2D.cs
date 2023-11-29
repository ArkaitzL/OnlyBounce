using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BaboOnLite
{

    [DefaultExecutionOrder(1)]
    //[AddComponentMenu("BaboOnLite/_Menus/Skins2D")]
    [DisallowMultipleComponent]
    [HelpURL("https://docs.google.com/document/d/1zPv7QP-ZyisadG5zREiMmzV7UWsYTUPZIPT0f_YlhSE/edit?usp=sharing")]
    public class Skins2D : MonoBehaviour
    {
        [Header("Objetos:")]
        [SerializeField] private RectTransform contenedor;
        [SerializeField] private RectTransform precio;
        [SerializeField] private RectTransform salir;
        [SerializeField] private GameObject skinPref;
        [Header("Personalizar:")]
        [SerializeField] private Sprite icono;
        [SerializeField] private TMP_FontAsset fuente;
        [SerializeField] private Skin2DColores colores;
        [Header("Skins:")]
        [SerializeField] private Skin2D[] skins;

        private GameObject seleccionado;
        private float tamaño = 450f; //Tamaño de separacion entre skins

        private void Start()
        {
            //Asigna la primera skin
            if (Save.Data.miSkin2D.imagen == null && skins.Length > 0)
            {
                //Seleccionas la skin
                Save.Data.miSkin2D.id = skins[0].id;
                Save.Data.miSkin2D.imagen = skins[0].imagen;
            }

            //Crea el menu con todas las skins
            #region crear
            //Dinero actual
            TextMeshProUGUI textoDinero = precio.GetChild(0).GetComponent<TextMeshProUGUI>();
            textoDinero.text = Save.Data.dinero.ToString();
            textoDinero.color = colores.textos;
            textoDinero.font = fuente;

            precio.GetChild(1).GetComponent<Image>().sprite = icono;

            //Colores
            GetComponent<Image>().color = colores.fondo;
            salir.GetComponent<Image>().color = colores.botonSalir;

            //Skins
            bool inicial = false;
            foreach (var skin in skins)
            {
                /*
                 * 0.- Seleccion
                 * 1.- Imagen
                 * 2.- Precio
                 * 3.- Boton
                 */

                //Comprueba si esta desbloqueada
                if (!skin.desbloqueado)
                {
                    skin.desbloqueado = Save.Data.listaSkin.Some((id) => id == skin.id);
                }
                //Crea las tarjetas
                Transform skinOBJ = Instantiate(skinPref, contenedor).transform;
                //Aumenta el tamaño del contenedor
                if (!inicial)
                {
                    inicial = true;
                }
                else
                {
                    contenedor.sizeDelta += new Vector2(tamaño, 0);
                }

                //Añade el contenido
                Datos(skinOBJ, skin);

                //3.-Boton
                skinOBJ.GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (skin.desbloqueado)
                    {
                        //Seleccionas la skin
                        Save.Data.miSkin2D.id = skin.id;
                        Save.Data.miSkin2D.imagen = skin.imagen;

                        seleccionado.SetActive(false);
                        skinOBJ.GetChild(0).gameObject.SetActive(true);
                        seleccionado = skinOBJ.GetChild(0).gameObject;
                    }
                    else
                    {
                        Comprar(skinOBJ, skin);
                    }
                });
            }
            #endregion
        }

        private void Datos(Transform skinOBJ, Skin2D skin) {
            //Imprime los datos en las tarjetas
            #region datos
            //1.-Imagen
            skinOBJ.GetChild(1).GetChild(0).GetComponent<Image>().sprite = skin.imagen;
            //Fondo
            Image fondo = skinOBJ.GetChild(1).GetComponent<Image>();
            if (skin.id == Save.Data.miSkin2D.id) 
            {
                skinOBJ.GetChild(0).gameObject.SetActive(true);
                seleccionado = skinOBJ.GetChild(0).gameObject;
            }

            fondo.color = colores.tarjeta.fondoSkin;

            //2.-Precio
            Transform precio = skinOBJ.GetChild(2);
            precio.GetComponent<Image>().color = colores.tarjeta.fondoPrecio;
            if (skin.desbloqueado)
            {
                precio.gameObject.SetActive(false);
            }
            else
            {
                TextMeshProUGUI textoPrecio = precio.GetChild(0).GetComponent<TextMeshProUGUI>();
                textoPrecio.text = skin.precio.ToString();
                textoPrecio.color = Color.white;
                textoPrecio.font = fuente;

                precio.GetChild(1).GetComponent<Image>().sprite = icono;
                if (skin.precio > Save.Data.dinero)
                {
                    textoPrecio.color = new Color(149 / 255f, 61 / 255f, 61 / 255f);
                }
            }
            //3.-Seleccion
            skinOBJ.GetChild(0).GetComponent<Image>().color = colores.bloqueo;
            #endregion
        }

        private void Comprar(Transform skinOBJ, Skin2D skin)
        {
            //Desbloqueas la sikin si tienes el dinero 
            #region comprar
            if (Save.Data.dinero >= skin.precio)
            {
                Save.Data.dinero -= skin.precio;
                Save.Data.listaSkin.Add(skin.id);
                skin.desbloqueado = true;

                TextMeshProUGUI textoDinero = precio.GetChild(0).GetComponent<TextMeshProUGUI>();
                textoDinero.text = Save.Data.dinero.ToString();

                Datos(skinOBJ, skin);
            }
            #endregion
        }
    }
}
