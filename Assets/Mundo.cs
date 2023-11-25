using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;
using System;

public class Mundo : MonoBehaviour
{
    //PRIVADAS
    [SerializeField] Transform personaje;
    [SerializeField] [Range(1, 100)] private float dificultad;
    [SerializeField] private float altura, espacio_pinchos = 3, espacio_pared = .5f;
    [SerializeField] private Transform[] plataformas;
    [SerializeField] private Objetos[] objetos;

    private float limite_izquierdo, limite_derecho, camara_ancho;
    private float y;

    //PUBLICAS
    [HideInInspector] public float espacio_juego;

    void Awake()
    {
        //Instancia la clase
        Instanciar<Mundo>.Añadir(this);

        //Limites de la camara
        float camara_altura = Camera.main.orthographicSize;
        camara_ancho = camara_altura * Camera.main.aspect;

        limite_izquierdo = -camara_ancho;
        limite_derecho = camara_ancho;

        y = -camara_altura;

        //Espacio de juego
        float resta = espacio_pared * 2 + espacio_pinchos;
        espacio_juego = camara_ancho * 2 - resta;

        //Generar
        for (int i = 0; i < 2; i++) Generar();
    }

    public void Generar() 
    {
        //Genera paredes
        for (int i = 0; i < altura; i++)
        {
            ///LIMITAR CANTIDAD DE PINCHOS SEGUIDOS 
            ///Y TAMPOCO DEMASIADOS SIN 
            ///Y QUE LOS PRIMEROS SEAN SIN 

            //Inicio sin obstaculos
            int tipo = 1;
            if (y > personaje.position.y + 1)
            {
                tipo = (UnityEngine.Random.Range(0, 101) <= dificultad)
                    ? 0
                    : 1;
            }

   

            //Derecha
            Instantiate(
                plataformas[tipo],
                new(limite_derecho - espacio_pared, y),
                Quaternion.Euler(0, 0, 180)
            ).parent = transform;

            //Izquierda
            Instantiate(
                plataformas[tipo],
                new(limite_izquierdo + espacio_pared, y),
                Quaternion.Euler(0, 0, 0)
            ).parent = transform;

            //Objetos
            if ((int)y % 5 == 0 && y > 0)
            {
                int objeto_tipo = UnityEngine.Random.Range(0, (100 * objetos.Length) + 1);
                float longitud = (espacio_juego -3) / 2;
                for (int index = 0; index < objetos.Length; index++)
                {
                    Objetos elemento = objetos[index];
                    int inicio = 100 * index; ;

                    if (objeto_tipo > inicio && objeto_tipo <= inicio + elemento.posibilidades)
                    {
                        Instantiate(
                            elemento.objeto,
                            new(UnityEngine.Random.Range(-longitud, longitud + 1), y),
                            Quaternion.identity
                        ).transform.parent = transform;
                        break;
                    }
                }
            }

            y++;
        }

        //Detector
        GameObject detector = new GameObject("---Detector---");
        detector.tag = "Detector";

        Transform detector_trans = detector.transform;
        detector_trans.localScale = new(camara_ancho*2, 1);
        detector_trans.position = new(0, y);
        detector_trans.parent = transform;

        detector.AddComponent<BoxCollider2D>().isTrigger = true;
    }

    [Serializable]
    public class Objetos 
    {
        public GameObject objeto;
        [Range(1, 100)] public float posibilidades;
    }
}
