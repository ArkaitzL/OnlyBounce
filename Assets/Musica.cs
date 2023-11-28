using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;
using UnityEngine.UI;

public class Musica : MonoBehaviour
{
    [SerializeField] private AudioClip sonido;

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
    }

    public void Cambiar()
    {
        audio_source.mute = Save.Data.muteado;
    }
}
