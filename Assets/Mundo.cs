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
    [SerializeField] private float altura, espacio_pinchos = 3, espacio_pared = .5f, max_seguidos = 8;
    [SerializeField] private Transform[] plataformas;
    [SerializeField] private Objetos[] objetos;
    [SerializeField] private Dificultad[] dificultades;

    private Enemigo enemigo;
    private Contador[] contador = { new(), new() };
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

    private void Start()
    {
        //Coge el enemigo
        enemigo = Instanciar<Enemigo>.Coger();
        //establece la dificultad inicial
        IniciarDificultad();
    }

    public void Generar() 
    {
        //Genera paredes
        for (int i = 0; i < altura; i++)
        {
            //Derecha
            Instantiate(
                plataformas[Tipo(0)],
                new(limite_derecho - espacio_pared, y),
                Quaternion.Euler(0, 0, 180)
            ).parent = transform;

            //Izquierda
            Instantiate(
                plataformas[Tipo(1)],
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

    private int Tipo(int lado) 
    {
        int tipo = 1;

        //Evitar que aparezcan al principio
        if (y > personaje.position.y + 1)
        {
            tipo = (UnityEngine.Random.Range(0, 101) <= dificultad)
                ? 0
                : 1;

            //Evitar repeticiones
            if (contador[lado].tipo == tipo)
            {
                contador[lado].cant++;

                if (contador[lado].cant == max_seguidos)
                {
                    tipo = (tipo == 0) ? 1 : 0;

                    contador[lado].tipo = tipo;
                    contador[lado].cant = 1;
                }
            }
            else
            {
                contador[lado].tipo = tipo;
                contador[lado].cant = 1;
            }
        }
        return tipo;
    }

    private void IniciarDificultad()
    {
        foreach (var dif in dificultades)
        {
            switch (dif.nombre)
            {
                case "dificultad":
                    dificultad = dif.inicial;
                    break;
                case "dinero":
                    objetos[1].posibilidades = dif.inicial;
                    break;
                case "enemigo":
                    enemigo.velocidad = dif.inicial;
                    break;
            }
        }
    }
    public void MasDificultad(int puntos) 
    {
        foreach (var dif in dificultades)
        {
            if (ConseguirDificultad(dif.nombre) >= dif.maximo) return;

            if (dif.incremento_ppt + dif.ultima_puntuacion <= puntos)
            {
                dif.ultima_puntuacion += dif.incremento_ppt;

                switch (dif.nombre)
                {
                    case "dificultad":
                        dificultad += dif.incremento_num;
                        break;
                    case "dinero":
                        objetos[1].posibilidades += dif.incremento_num;
                        break;
                    case "enemigo":
                        enemigo.velocidad += dif.incremento_num;
                        break;
                }
            }
        }
    }

    private float ConseguirDificultad(string nombre)
    {
        switch (nombre)
        {
            case "dificultad":
                return dificultad;
            case "dinero":
                return objetos[1].posibilidades;
            case "enemigo":
                return enemigo.velocidad;
        }
        return 0;
    }

    [Serializable]
    public class Dificultad
    {
        public string nombre;
        public float inicial;
        public float maximo;
        public float incremento_num;
        public float incremento_ppt;
        [HideInInspector] public float ultima_puntuacion;

    }
    [Serializable]
    public class Objetos 
    {
        public GameObject objeto;
        [Range(1, 100)] public float posibilidades;
    }

    [Serializable]
    public class Contador 
    {
        public int tipo;
        public int cant;
    }
}
