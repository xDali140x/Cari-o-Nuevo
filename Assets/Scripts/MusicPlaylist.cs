using System.Collections;
using UnityEngine;

public class MusicPlaylist : MonoBehaviour
{
    [Header("Canciones")]
    public AudioClip[] canciones; // Arrastra aquí tus 7 AudioClips

    [Header("Configuración")]
    public bool aleatorio = false;        // ¿Orden aleatorio?
    public float volumen = 1f;
    [Range(0f, 1f)]
    public float spatialBlend = 1f;       // 1 = 3D espacial, 0 = 2D global

    [Header("Audio 3D")]
    public float minDistance = 5f;
    public float maxDistance = 30f;

    private AudioSource audioSource;
    private int indiceActual = 0;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volumen;
        audioSource.spatialBlend = spatialBlend;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.loop = false; // El loop lo maneja el script
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        if (canciones == null || canciones.Length == 0)
        {
            Debug.LogWarning("MusicPlaylist: No hay canciones asignadas.");
            return;
        }

        StartCoroutine(ReproducirPlaylist());
    }

    IEnumerator ReproducirPlaylist()
    {
        while (true)
        {
            if (aleatorio)
                indiceActual = Random.Range(0, canciones.Length);

            AudioClip clip = canciones[indiceActual];

            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
                Debug.Log($"▶ Reproduciendo: {clip.name}");
                yield return new WaitForSeconds(clip.length);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (!aleatorio)
                indiceActual = (indiceActual + 1) % canciones.Length;
        }
    }
}