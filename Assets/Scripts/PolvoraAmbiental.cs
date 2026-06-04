using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PolvoraAmbiental : MonoBehaviour
{
    [Header("Clips de pólvora")]
    public AudioClip[] clips; // Arrastra aquí uno o varios clips

    [Header("Intervalos")]
    public float tiempoMinimo = 120f; // 2 minutos mínimo
    public float tiempoMaximo = 240f; // 4 minutos máximo

    private AudioSource audioSource;
    private float proximoDisparo;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        ProgramarSiguiente();
    }

    void Update()
    {
        if (Time.time >= proximoDisparo)
        {
            Disparar();
            ProgramarSiguiente();
        }
    }

    void Disparar()
    {
        if (clips.Length == 0) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }

    void ProgramarSiguiente()
    {
        proximoDisparo = Time.time + Random.Range(tiempoMinimo, tiempoMaximo);
    }
}