using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public class Mundo : MonoBehaviour
{
    [SerializeField] [Range(1, 100)] private float dificultad;
    [SerializeField] private float altura, espacio_pared = .5f;
    [SerializeField] private Transform[] plataformas;

    private float limite_izquierdo, limite_derecho, camara_ancho;
    private float y;

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

        //Generar
        for (int i = 0; i < 2; i++) Generar();
    }

    public void Generar() 
    {
        //Genera paredes
        for (int i = 0; i < altura; i++)
        {
            ///LIMITAR CANTIDAD DE PINCHOS SEGUIDOS Y TAMPOCO DEMASIADOS SIN ***
            int tipo = (Random.Range(0, 101) <= dificultad)
                ? 0
                : 1;

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

            y++;
        }

        //Detector
        GameObject detector = new GameObject("---Detector---");
        detector.tag = "Detector";
        detector.transform.localScale = new(camara_ancho*2, 1);

        detector.AddComponent<BoxCollider2D>().isTrigger = true;

        Instantiate(
            detector,
            new(0, y),
            Quaternion.identity
        ).transform.parent = transform;
    }
}
