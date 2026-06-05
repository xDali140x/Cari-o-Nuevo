using UnityEngine;
using UnityEngine.Video;

public class TelevisorController : MonoBehaviour
{
    [Header("Pantallas")]
    public GameObject pantallaApagada;
    public GameObject pantallaVideo;

    [Header("Reproductor y canales")]
    public VideoPlayer reproductorVideo;
    public VideoClip[] canales;

    [Header("Estado inicial")]
    public bool iniciarEncendido = false;
    public int canalInicial = 0;
    [Header("Audio")]
    public AudioSource audioTelevisor;

    private bool estaEncendido;
    private int canalActual;

    private void Awake()
    {
        if (reproductorVideo != null)
        {
            reproductorVideo.playOnAwake = false;
            reproductorVideo.isLooping = true;

            // Se ejecuta cuando el nuevo canal ya está listo.
            reproductorVideo.prepareCompleted += AlPrepararVideo;
        }
    }

    private void Start()
    {
        estaEncendido = iniciarEncendido;

        if (canales != null && canales.Length > 0)
        {
            canalActual = Mathf.Clamp(canalInicial, 0, canales.Length - 1);
        }
        else
        {
            canalActual = 0;
        }

        if (estaEncendido)
        {
            CargarCanalActual();
        }
        else
        {
            MostrarPantallaApagada();
        }
    }

    private void OnDestroy()
    {
        if (reproductorVideo != null)
        {
            reproductorVideo.prepareCompleted -= AlPrepararVideo;
        }
    }

    public void AlternarEncendido()
    {
        estaEncendido = !estaEncendido;

        if (estaEncendido)
        {
            CargarCanalActual();
            Debug.Log("Televisor encendido");
        }
        else
        {
            MostrarPantallaApagada();
            Debug.Log("Televisor apagado");
        }
    }

    public void SiguienteCanal()
    {
        if (!estaEncendido)
        {
            return;
        }

        if (canales == null || canales.Length == 0)
        {
            Debug.LogWarning("No hay videos asignados como canales.");
            return;
        }

        canalActual++;

        if (canalActual >= canales.Length)
        {
            canalActual = 0;
        }

        CargarCanalActual();
        Debug.Log("Canal actual: " + canalActual);
    }
    public void AlternarMute()
    {
        if (audioTelevisor == null)
        {
            Debug.LogWarning("No se asignó el Audio Source del televisor.");
            return;
        }

        audioTelevisor.mute = !audioTelevisor.mute;

        Debug.Log(audioTelevisor.mute
            ? "Televisor silenciado"
            : "Sonido del televisor activado");
    }

    private void CargarCanalActual()
    {
        if (reproductorVideo == null)
        {
            Debug.LogWarning("No se asignó el Video Player.");
            return;
        }

        if (canales == null ||
            canales.Length == 0 ||
            canales[canalActual] == null)
        {
            Debug.LogWarning("El canal actual no tiene un video asignado.");
            MostrarPantallaApagada();
            return;
        }

        // Mantiene la pantalla negra mientras carga el nuevo canal.
        if (pantallaApagada != null)
        {
            pantallaApagada.SetActive(true);
        }

        if (pantallaVideo != null)
        {
            pantallaVideo.SetActive(true);
        }

        reproductorVideo.Stop();
        reproductorVideo.clip = canales[canalActual];
        reproductorVideo.Prepare();
    }

    private void AlPrepararVideo(VideoPlayer videoPlayer)
    {
        if (!estaEncendido)
        {
            return;
        }

        if (pantallaApagada != null)
        {
            pantallaApagada.SetActive(false);
        }

        if (pantallaVideo != null)
        {
            pantallaVideo.SetActive(true);
        }

        videoPlayer.Play();
    }

    private void MostrarPantallaApagada()
    {
        if (reproductorVideo != null)
        {
            reproductorVideo.Stop();
        }

        if (pantallaVideo != null)
        {
            pantallaVideo.SetActive(false);
        }

        if (pantallaApagada != null)
        {
            pantallaApagada.SetActive(true);
        }
    }
}