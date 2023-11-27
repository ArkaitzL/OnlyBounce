using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;
using UnityEngine.UI;

public class Musica : MonoBehaviour
{
    [SerializeField] private AudioClip sonido;
    [SerializeField] private Image imagen;
    [SerializeField] private Sprite desmuteado, muteado;

    private AudioSource audio_source;

    private void Awake()
    {
        Instanciar<Musica>.Singletons(this, gameObject);
    }

    private void Start()
    {
        audio_source = gameObject.AddComponent<AudioSource>();
        audio_source.clip = sonido;

        audio_source.loop = true;
        audio_source.Play();

        Aplicar();
    }

    public void Mutear()
    {
        Save.Data.muteado = !Save.Data.muteado;
        Aplicar();
    }

    private void Aplicar()
    {
        audio_source.mute = Save.Data.muteado;
        imagen.sprite = (Save.Data.muteado) ? muteado : desmuteado;
    }
}
