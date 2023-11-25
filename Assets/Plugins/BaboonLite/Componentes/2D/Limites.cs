using UnityEngine;

namespace BaboOnLite
{
    [DefaultExecutionOrder(0)]
    [AddComponentMenu("BaboOnLite/_2D/Limites")]
    [DisallowMultipleComponent]
    [HelpURL("https://docs.google.com/document/d/1zPv7QP-ZyisadG5zREiMmzV7UWsYTUPZIPT0f_YlhSE/edit?usp=sharing")]

    public class Limites : MonoBehaviour
    {

        //PUBLICAS

        //Transforms de los limites
        [SerializeField] private Manual manual;
        [Header("Automatico")]
        //Automatizar
        [SerializeField] private bool alturaAutomatica;
        [SerializeField] private bool instanciaDefecto;

        //PRIVADAS

        private float camWidth;

        private void OnValidate()
        {
            //Valida el uso de height junto a instance
            #region altura automatica
            if (instanciaDefecto)
            {
                alturaAutomatica = true;
            }
            #endregion
        }

        //Posiciona los elementos a los bordes de la camara
        private void Awake()
        {
            //Comprueba que la instancia manual este bien
            #region errores
            if (!instanciaDefecto)
            {
                if (manual.derecho == null || manual.izquierdo == null)
                {
                    //No estan ni los limites automatico, ni los manuales
                    Bug.LogLite("[BL][limites: 1] No tienes asignado ningun limite, se asignaran automaticamente");
                    alturaAutomatica = true;
                    instanciaDefecto = true;
                }
            }
            #endregion

            //Crea los limites de la camara
            #region crear limites
            //Longitud de la camara
            camWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;

            //Instancia los limites
            if (instanciaDefecto)
            {
                manual.izquierdo = Instance("left");
                manual.derecho = Instance("right");
            }

            //Asigna la altura
            if (alturaAutomatica)
            {
                Altura(manual.izquierdo);
                Altura(manual.derecho);
            }

            //Cambia la posicion
            manual.izquierdo.position = Posicion(manual.izquierdo.localScale.z, -1);
            manual.derecho.position = Posicion(manual.derecho.localScale.z, 1);
            #endregion
        }

        //Todas las funciones
        #region funciones
        //Instancia dos BoxCollider2D
        private Transform Instance(string name)
        {
            GameObject ob = new GameObject(name);
            ob.AddComponent<BoxCollider2D>();
            ob.transform.SetParent(transform);

            return ob.transform;
        }

        //Adapta el tamaño a la altura de la camara
        private void Altura(Transform go)
        {
            float camHeight = Camera.main.orthographicSize * 2;

            Vector3 scale = go.localScale;
            scale.y = camHeight;
            go.localScale = scale;
        }

        //Adapta la posicion a los bordes de la camara
        private Vector3 Posicion(float posicion, float signo)
        {
            return new Vector3(
                (Camera.main.transform.position.x - (camWidth / 2) - (posicion / 2)) * signo,
            0, 0);
        }
        #endregion
    }
}
