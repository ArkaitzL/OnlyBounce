using TMPro;
using UnityEngine;

namespace BaboOnLite {

    [DefaultExecutionOrder(1)]
    [AddComponentMenu("BaboOnLite/_Idiomas/Textos")]
    [DisallowMultipleComponent]
    [HelpURL("https://docs.google.com/document/d/1zPv7QP-ZyisadG5zREiMmzV7UWsYTUPZIPT0f_YlhSE/edit?usp=sharing")]
    public class Textos : MonoBehaviour
    {
        [SerializeField] private DictionaryBG<TextMeshProUGUI> textos = new();

        private void OnValidate()
        {
            Cambiar();
        }

        void Start()
        {
            Cambiar();
            //Se suscribe al evento para cuando se cambia el idioma
            Idiomas.cambiarTextos += () => {
                Cambiar();
            };
        }

        private void Cambiar() {
            //Añade los strings a los textos
            #region aplicar textos
            textos.ForEach((index, textMesh) => {
                if (textMesh != null && index != null)
                {
                    if (int.TryParse(index, out int i))
                    {
                        //Int
                        textMesh.text = Idiomas.Get(i);
                    }
                    else
                    {
                        //String
                        textMesh.text = Idiomas.Get(index);
                    }
                }
            });
            #endregion
        }
    }
}

