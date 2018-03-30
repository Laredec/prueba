using UnityEngine;
using System.Collections;

public class ReproducirSonido : MonoBehaviour
{
    public AudioClip Sonido;
    public AudioSource Source;
    public float Volumen = 1f;

    public void Reproducir()
    {
        if (!Source)
            Source = GameObject.Find("Sound").GetComponent<AudioSource>();

        if (Source)
            Source.PlayOneShot(Sonido, Volumen);
        

    }

}
