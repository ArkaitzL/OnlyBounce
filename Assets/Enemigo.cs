using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float velocidad;

    private Mundo mundo;

    void Start()
    {
        //Coge el script de mundo
        mundo = Instanciar<Mundo>.Coger();

        //Coge el tamaño
        transform.localScale = new(mundo.espacio_juego, .5f);
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * velocidad * Vector2.up);
    }
}
