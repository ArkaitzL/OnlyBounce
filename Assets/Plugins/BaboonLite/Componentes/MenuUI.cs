using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaboOnLite
{
    [DefaultExecutionOrder(0)]
    [AddComponentMenu("BaboOnLite/MenuUI")]
    [DisallowMultipleComponent]
    [HelpURL("https://docs.google.com/document/d/1zPv7QP-ZyisadG5zREiMmzV7UWsYTUPZIPT0f_YlhSE/edit?usp=sharing")]

    public class MenuUI : MonoBehaviour
    {

        //VARIABLES

        [Header("General")]
        [SerializeField] bool activarTiempo = true;
        [Header("Botones")]
        [SerializeField] KeyCode[] reiniciar = { KeyCode.R };
        [SerializeField] KeyCode[] pausar = { KeyCode.P };

        //FUNCIONES PRIVADAS

        private void Start() {
            //Activa el tiempo automaticamente
            if(activarTiempo) Time.timeScale = 1;
        }

        //Botones
        private void Update()
        {
            //Le da una funcion a los botones
            #region botones
            Botones(reiniciar, ReiniciarEscena);
            Botones(pausar, AlternarTiempo);
            #endregion
        }

        //Le da una funcion a los botones
        #region botones
        private void Botones(KeyCode[] teclas, Action func)
        {
            teclas.ForEach((tecla) => {
                if (Input.GetKeyDown(tecla))
                {
                    func.Invoke();
                }
            });
        }
        #endregion

        //FUNCIONES PUBLICAS

        //Cambia la escena
        #region cambio_escena
        public static void CambioEscena(int escena)
        {
            SceneManager.LoadScene(escena);
        }
        public static void CambioEscena(string escena)
        {
            SceneManager.LoadScene(escena);
        }
        #endregion

        //Reinicia la escena
        #region reinicio
        public static void ReiniciarEscena()
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().name
            );
        }
        #endregion

        //Varias funciones para activar y desactivar el estado de un GameObject
        #region cambiar_estado
        //Activa y desactiva un GameObject dependiendo su anterior estado
        public static void AlterarEstado(GameObject componente)
        {
            componente.SetActive(
                !componente.activeSelf
            );
        }

        //Activa o desactiva un GameObject dependiendo lo que quieras
        public static void Activar(GameObject componente)
        {
            componente.SetActive(
                true
            );
        }
        public static void Desactivar(GameObject componente)
        {
            componente.SetActive(
                false
            );
        }
        #endregion

        //Varias funciones para cambiar el estado del tiempo
        #region tiempo
        //Pausa o quita el pausa del juego
        public static void PausarTiempo(bool pausa = true)
        {
            Time.timeScale = (pausa) ? 0 : 1;
        }
        //Altrna entre pausa y play
        public static void AlternarTiempo()
        {
            Time.timeScale = (Time.timeScale == 1) ? 0 : 1;
        }
        #endregion

        //Te permite abrir una url
        #region url
        public void AbrirURL(string url)
        {
            Application.OpenURL(url);
        }
        #endregion
    }
}
