using UnityEngine;

public class RadioController : MonoBehaviour
{
    [Header("Canciones")]
    public AudioClip[] canciones; // Arrastra tus 9 canciones

    [Header("Audio")]
    public AudioSource audioRadio;

    [Header("Estado inicial")]
    public bool iniciarEncendida = true;
    public int cancionInicial = 0;

    private bool estaEncendida;
    private int cancionActual;

    private void Start()
    {
        estaEncendida = iniciarEncendida;

        if (canciones != null && canciones.Length > 0)
            cancionActual = Mathf.Clamp(cancionInicial, 0, canciones.Length - 1);
        else
            cancionActual = 0;

        if (estaEncendida)
            CargarCancionActual();
        else
            audioRadio.Stop();
    }

    // Botón 1: Cambiar canción
    public void SiguienteCancion()
    {
        if (!estaEncendida) return;

        if (canciones == null || canciones.Length == 0)
        {
            Debug.LogWarning("No hay canciones asignadas.");
            return;
        }

        cancionActual = (cancionActual + 1) % canciones.Length;
        CargarCancionActual();
        Debug.Log("Canción actual: " + canciones[cancionActual].name);
    }

    // Botón 2: Mutear / Desmutear
    public void AlternarMute()
    {
        if (audioRadio == null)
        {
            Debug.LogWarning("No se asignó el Audio Source de la radio.");
            return;
        }

        audioRadio.mute = !audioRadio.mute;
        Debug.Log(audioRadio.mute ? "Radio silenciada" : "Radio con sonido");
    }

    // Opcional: encender/apagar
    public void AlternarEncendido()
    {
        estaEncendida = !estaEncendida;

        if (estaEncendida)
            CargarCancionActual();
        else
            audioRadio.Stop();

        Debug.Log(estaEncendida ? "Radio encendida" : "Radio apagada");
    }

    private void CargarCancionActual()
    {
        if (audioRadio == null || canciones == null || canciones[cancionActual] == null)
        {
            Debug.LogWarning("Faltan referencias en la radio.");
            return;
        }

        audioRadio.clip = canciones[cancionActual];
        audioRadio.loop = true; // cada canción loopea hasta que cambias
        audioRadio.Play();
    }
}