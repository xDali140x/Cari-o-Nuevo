using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlatoSound : MonoBehaviour
{
    [Header("Sonidos")]
    public AudioClip sonidoFregando;   // Suena mientras frota la esponja
    public AudioClip sonidoLimpio;     // Suena cuando el plato queda limpio

    private AudioSource audioSource;
    private Plato platoScript;

    private float[] contadoresAnteriores;
    private bool[] yaDesaparecioAnterior;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        platoScript = GetComponent<Plato>();

        // Configuración 3D
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = 0.3f;
        audioSource.maxDistance = 4f;
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // Inicializar arrays del mismo tamaño que Plato.cs
        int n = platoScript.collidersADesaparecer.Length;
        contadoresAnteriores = new float[n];
        yaDesaparecioAnterior = new bool[n];
    }

    void Update()
    {
        if (platoScript == null) return;

        bool algunoFregando = false;

        for (int i = 0; i < platoScript.collidersADesaparecer.Length; i++)
        {
            // Detectar si está fregando activamente (contador subiendo)
            float contadorActual = ObtenerContador(i);
            if (contadorActual > 0.05f && !ObtenerYaDesaparecio(i))
            {
                algunoFregando = true;
            }

            // Detectar cuando un plato recién quedó limpio
            bool limpioAhora = ObtenerYaDesaparecio(i);
            if (limpioAhora && !yaDesaparecioAnterior[i])
            {
                if (sonidoLimpio != null)
                    audioSource.PlayOneShot(sonidoLimpio, 0.8f);
                yaDesaparecioAnterior[i] = true;
            }

            contadoresAnteriores[i] = contadorActual;
        }

        // Loop del sonido de fregando
        if (algunoFregando)
        {
            if (!audioSource.isPlaying && sonidoFregando != null)
            {
                audioSource.clip = sonidoFregando;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying && audioSource.clip == sonidoFregando)
            {
                audioSource.Stop();
                audioSource.loop = false;
            }
        }
    }

    // Accede al contador privado de Plato.cs via reflection
    float ObtenerContador(int i)
    {
        var field = typeof(Plato).GetField("contadores",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);
        if (field == null) return 0f;
        float[] arr = (float[])field.GetValue(platoScript);
        if (arr == null || i >= arr.Length) return 0f;
        return arr[i];
    }

    // Accede al yaDesaparecio privado de Plato.cs via reflection
    bool ObtenerYaDesaparecio(int i)
    {
        var field = typeof(Plato).GetField("yaDesaparecio",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);
        if (field == null) return false;
        bool[] arr = (bool[])field.GetValue(platoScript);
        if (arr == null || i >= arr.Length) return false;
        return arr[i];
    }
}