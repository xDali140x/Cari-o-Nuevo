using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BroomSound : MonoBehaviour
{
    [Header("Sonidos de la escoba")]
    public AudioClip sonidoRebote;    // Cuando cae y rebota en el piso
    public AudioClip sonidoGolpe;     // Cuando choca contra algo

    [Header("Configuración")]
    [Range(0f, 1f)]
    public float velocidadMinimaRebote = 0.5f; // Qué tan fuerte debe golpear para sonar

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Configuración 3D espacial
        audioSource.spatialBlend = 1f;          // 100% sonido 3D
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = 0.3f;         // Distancia donde se oye full volumen
        audioSource.maxDistance = 8f;           // Distancia máxima donde se oye
    }

    void OnCollisionEnter(Collision collision)
    {
        float velocidad = collision.relativeVelocity.magnitude;

        // Solo reproduce si el golpe fue suficientemente fuerte
        if (velocidad < velocidadMinimaRebote) return;

        // Volumen proporcional a la fuerza del golpe (máximo 1)
        float volumen = Mathf.Clamp01(velocidad / 5f);

        // Si choca con el piso → rebote; si choca con otra cosa → golpe
        bool esPiso = collision.gameObject.CompareTag("Ground") ||
                      collision.gameObject.CompareTag("Floor") ||
                      collision.contacts[0].normal.y > 0.7f; // el suelo tiene normal hacia arriba

        AudioClip clipAReproducir = esPiso ? sonidoRebote : sonidoGolpe;

        if (clipAReproducir != null)
        {
            audioSource.PlayOneShot(clipAReproducir, volumen);
        }
    }
}