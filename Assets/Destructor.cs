using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor : MonoBehaviour
{
    void Start()
    {
        float camara_altura = Camera.main.orthographicSize;
        float camara_ancho = camara_altura * Camera.main.aspect;

        transform.localScale = new(camara_ancho * 2, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
