using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BaboOnLite;
using UnityEngine.SceneManagement;

public class Controlador : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform salvador;
    [SerializeField] private Transform personaje;
    [SerializeField] private GameObject enemigo, menu_muerte, tutorial;
    [SerializeField] private TextMeshProUGUI muerte_txt, puntos_txt;
    [Header("Valores")]
    [SerializeField] private float valor_puntos = 1;
    [SerializeField] private float tiempo_revivir = 1;

    [HideInInspector] public int dinero;
    [HideInInspector] public bool muerto = true;

    private Mundo mundo;
    private int puntos;
    private float y;

    private void Awake()
    {
        //Instancia la clase
        Instanciar<Controlador>.Añadir(this);

        //Guarda el inicio del peresonaje
        y = personaje.position.y;
    }

    private void Start()
    {
        //Tutorial
        if (!Save.Data.iniciado)
        {
            Save.Data.iniciado = true;

            tutorial.SetActive(true);
            TextMeshProUGUI texto = tutorial.GetComponent<TextMeshProUGUI>();

            ControladorBG.Rutina(5f, () => {       
                ControladorBG.ColorText(texto, new Color32(0,0,0,0), .2f);
            });
        }

        //Coge el script de mundo
        mundo = Instanciar<Mundo>.Coger();
    }

    private void Update()
    {
        //Puntuacion
        if (personaje.position.y >= valor_puntos + y)
        {
            y += 1;
            puntos += 1;

            puntos_txt.text = $"{puntos}";

            mundo.MasDificultad(puntos);
        }
    }

    public void Salvar(float posY) 
    {
        salvador.position = new(0, posY);
    }

    public void Muerto() 
    {
        muerto = true;

        //Desactiva componentes
        puntos_txt.enabled = false;
        enemigo.GetComponent<Enemigo>().enabled = false;

        //Menu muerte
        menu_muerte.SetActive(true);
        muerte_txt.text =    $"Puntos x{puntos}\t\t{(int)puntos/10}$\n" +
                             $"Dinero x{dinero}\t\t{dinero*3}$" +
                             $"\n___________________" +
                             $"\nTotal\t\t\t{Total()}$"; ;
    }

    public void Revivir() 
    {
        Anuncios.verRewardedRecompensa(() => {
            //Te salva
            Salvar(personaje.transform.position.y);

            //Activa componentes
            puntos_txt.enabled = true;
            personaje.gameObject.SetActive(true);

            //Desactiva Menu muerte
            menu_muerte.SetActive(false);

            ControladorBG.Rutina(tiempo_revivir, () =>
            {
                muerto = false;

                //Enemigo
                enemigo.GetComponent<Enemigo>().enabled = true;
                if (enemigo.transform.position.y > personaje.position.y - 5)
                {
                    enemigo.transform.position = new(0, personaje.position.y - 5);
                }
            });
        }); 
    }

    public void Multiplicador() 
    {
        Anuncios.verRewardedRecompensa(() =>
        {
            Terminar(2);
            SceneManager.LoadScene("Menu");
        });
    }

    public void Continuar() 
    {
        Terminar(1);
        SceneManager.LoadScene("Menu");
    }

    private void Terminar(int multiplicador) 
    {
        Save.Data.dinero += Total() * multiplicador;
        Mejor();
    }

    private void OnApplicationQuit()
    {
        Terminar(1);
    }

    private void Mejor() 
    {
        if (puntos > Save.Data.mejor_puntuacion)
        {
            Save.Data.mejor_puntuacion = puntos;
        }
    }

    private int Total() => (dinero * 3) + ((int)puntos / 10);
}
