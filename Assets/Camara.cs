using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    [SerializeField] Transform personaje;
    private float diferencia;

    private void Start()
    {
        diferencia = transform.position.y - personaje.position.y;
    }

    private void Update()
    {
        transform.position = new(
            0,
            personaje.position.y + diferencia
        );
    }
}
