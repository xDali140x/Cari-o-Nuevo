using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VelaSound : MonoBehaviour
{
    [Header("Sonido de la vela")]
    public AudioClip sonidoEncendido; // El crepitar/chispa cuando se prende

    private AudioSource audioSource;
    private Vela velaScript;
    private bool yaReprodujo = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        velaScript = GetComponent<Vela>();

        // Configuración 3D
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = 0.3f;
        audioSource.maxDistance = 5f;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // Cuando la vela se activa por primera vez → suena
        if (velaScript != null && velaScript.isActivated && !yaReprodujo)
        {
            if (sonidoEncendido != null)
            {
                audioSource.PlayOneShot(sonidoEncendido);
            }
            yaReprodujo = true;
        }

        // Si la vela se resetea → permite que suene de nuevo
        if (velaScript != null && !velaScript.isActivated && yaReprodujo)
        {
            yaReprodujo = false;
        }
    }
}