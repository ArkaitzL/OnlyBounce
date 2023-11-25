using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;
using System;

public class Personaje : MonoBehaviour
{
    [SerializeField] private float velocidad_horizontal, velocidad_vertical;
    [SerializeField] private Potenciador potenciador;
    private Vector2 direccion;
    private Mundo mundo;

    private void Start()
    {
        //Coge el script de mundo
        mundo = Instanciar<Mundo>.Coger();
        //Iniciar
        direccion = new((UnityEngine.Random.Range(0, 2) == 0) ? 1 : -1, 0);
    }

    void Update()
    {
        //Horizontal
        transform.Translate(Time.deltaTime * velocidad_horizontal * direccion);

        //Vertical
        if (Input.GetMouseButton(0))
        {
            transform.Translate(Time.deltaTime * velocidad_vertical * Vector2.up);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Limite
        if (collision.tag == "Limite")
        {
            if (ControladorBG.Esperando("girar"))
            {
                direccion = new(direccion.x * -1, 0);
                ControladorBG.IniciarEspera("girar", .5f);
            }
        }
        //Muerte
        if (collision.tag == "Muerte")
        {
            Time.timeScale = 0;
            Debug.Log("Muerto");
        }
        //Detector
        if (collision.tag == "Detector")
        {
            mundo.Generar();
            collision.gameObject.SetActive(false);
        }
        //Potenciador
        if (collision.tag == "Potenciador")
        {
            if (ControladorBG.Esperando("potenciado"))
            {
                ControladorBG.IniciarEspera("potenciado", potenciador.duracion);
                ControladorBG.Mover(transform, new Movimiento(
                    potenciador.duracion,
                    transform.position.Y(potenciador.distancia), potenciador.animacion)
                );
            }
        }
        //Dinero
        if (collision.tag == "Dinero")
        {
            //Save.Data.dinero+=1;
            Destroy(collision.gameObject);
            //Animacion dinero coger ***
            Debug.Log("Dinero: +1");
        }
    }

    [Serializable]
    public class Potenciador 
    { 
        public float duracion;
        public float distancia;
        public AnimationCurve animacion;
    }
}
