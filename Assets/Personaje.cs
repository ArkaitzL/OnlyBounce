using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public class Personaje : MonoBehaviour
{
    [SerializeField] private float velocidad_horizontal, velocidad_vertical;
    private Vector2 direccion;
    private Mundo mundo;

    private void Start()
    {
        //Coge el script de mundo
        mundo = Instanciar<Mundo>.Coger();
        //Iniciar
        direccion = new((Random.Range(0, 2) == 0) ? 1 : -1, 0);
    }

    void Update()
    {
        //Horizontal
        transform.Translate(Time.deltaTime * velocidad_horizontal * direccion);

        //Vertical
        if (Input.GetKey(KeyCode.Space))
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
        }
    }
}
