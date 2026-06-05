using UnityEngine;

public class SonidoUnaVez : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        Invoke("ReproducirSonido", 3f);
    }

    void ReproducirSonido()
    {
        if (audioSource != null)
            audioSource.PlayOneShot(audioSource.clip);
    }
}