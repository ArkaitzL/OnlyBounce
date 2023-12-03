using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor : MonoBehaviour
{
    [SerializeField] private bool todo;

    void Start()
    {
        float camara_altura = Camera.main.orthographicSize;
        float camara_ancho = camara_altura * Camera.main.aspect;

        transform.localScale = new(camara_ancho * 2, transform.localScale.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (todo || collision.tag == "Muerte")
        {
            Destroy(collision.gameObject);
        }
    }
}
