using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private Transform personaje;
    [SerializeField] public float velocidad;
    [SerializeField] private int distancia = 15;

    private Mundo mundo;

    private void Awake()
    {
        Instanciar<Enemigo>.A�adir(this);
    }

    void Start()
    {
        //Coge el script de mundo
        mundo = Instanciar<Mundo>.Coger();

        //Coge el tama�o
        transform.localScale = new(mundo.espacio_juego, .5f);
    }

    void Update()
    {
        //Se mueve
        transform.Translate(Time.deltaTime * velocidad * Vector2.up);

        //Se TPa debajo del jugador
        float nuevaPosY = Mathf.Max(personaje.position.y - distancia, transform.position.y);
        transform.position = new Vector2(transform.position.x, nuevaPosY);
    }
}
