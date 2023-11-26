using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Controlador : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI puntos_txt;
    [SerializeField] private Transform personaje;
    [SerializeField] private float valor_puntos = 1;

    private int puntos;
    private float y;

    private void Start()
    {
        y = personaje.position.y;
    }
    private void Update()
    {
        //Puntuacion
        if (personaje.position.y >= valor_puntos + y)
        {
            y += 1;
            puntos += 1;

            puntos_txt.text = $"{puntos}";
        }

    }
}
