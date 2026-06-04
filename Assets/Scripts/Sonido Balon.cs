using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BallSound : MonoBehaviour
{
    [Header("Sonidos del balón")]
    public AudioClip sonidoRebote;   // Cuando bota en el piso
    public AudioClip sonidoGolpe;    // Cuando lo patean o chocan

    [Header("Configuración")]
    [Range(0f, 5f)]
    public float velocidadMinima = 0.8f; // Velocidad mínima para que suene

    private AudioSource audioSource;
    private float tiempoUltimoSonido = 0f;
    public float tiempoEntreSonidos = 0.15f; // Evita que suene 50 veces seguidas

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Sonido 3D espacial
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = 0.5f;
        audioSource.maxDistance = 10f;
        audioSource.playOnAwake = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        float velocidad = collision.relativeVelocity.magnitude;

        // Ignorar golpes muy suaves
        if (velocidad < velocidadMinima) return;

        // Evitar spam de sonido en colisiones múltiples
        if (Time.time - tiempoUltimoSonido < tiempoEntreSonidos) return;

        tiempoUltimoSonido = Time.time;

        // Volumen proporcional a la fuerza (máximo 1)
        float volumen = Mathf.Clamp01(velocidad / 8f);

        // Normal apuntando hacia arriba = piso = rebote
        bool esPiso = collision.contacts[0].normal.y > 0.5f;

        AudioClip clip = esPiso ? sonidoRebote : sonidoGolpe;

        if (clip != null)
            audioSource.PlayOneShot(clip, volumen);
    }
}