using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReproducirCinematicaYCambiarEscena : MonoBehaviour
{
    [Header("VIDEO")]

    [Tooltip("Arrastra aquí el componente Video Player.")]
    public VideoPlayer reproductorVideo;


    [Header("ESCENA QUE SE CARGARÁ AL TERMINAR")]

#if UNITY_EDITOR
    [Tooltip("Arrastra aquí el archivo .unity de la siguiente escena.")]
    public SceneAsset escenaSiguiente;
#endif

    [Tooltip("Se rellena automáticamente al arrastrar la escena.")]
    public string nombreEscenaSiguiente;


    [Header("CONFIGURACIÓN")]

    [Tooltip("Muestra mensajes útiles dentro de la consola.")]
    public bool mostrarMensajesEnConsola = true;


    [Header("ESTADO ACTUAL")]

    public bool videoPreparado = false;

    public bool cambioDeEscenaIniciado = false;


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (reproductorVideo == null)
        {
            reproductorVideo = GetComponent<VideoPlayer>();
        }

        if (escenaSiguiente != null)
        {
            nombreEscenaSiguiente = escenaSiguiente.name;
        }
    }
#endif


    private void OnEnable()
    {
        if (reproductorVideo == null)
        {
            return;
        }

        reproductorVideo.prepareCompleted += CuandoElVideoEstePreparado;
        reproductorVideo.loopPointReached += CuandoTermineElVideo;
        reproductorVideo.errorReceived += CuandoOcurraUnError;
    }


    private void OnDisable()
    {
        if (reproductorVideo == null)
        {
            return;
        }

        reproductorVideo.prepareCompleted -= CuandoElVideoEstePreparado;
        reproductorVideo.loopPointReached -= CuandoTermineElVideo;
        reproductorVideo.errorReceived -= CuandoOcurraUnError;
    }


    private void Start()
    {
        if (reproductorVideo == null)
        {
            Debug.LogError("No asignaste el componente Video Player.");
            return;
        }

        if (string.IsNullOrWhiteSpace(nombreEscenaSiguiente))
        {
            Debug.LogError("No asignaste la escena siguiente.");
            return;
        }

        reproductorVideo.playOnAwake = false;
        reproductorVideo.isLooping = false;

        if (mostrarMensajesEnConsola)
        {
            Debug.Log("Preparando cinemática...");
        }

        if (reproductorVideo.isPrepared)
        {
            ReproducirVideo();
        }
        else
        {
            reproductorVideo.Prepare();
        }
    }


    private void CuandoElVideoEstePreparado(VideoPlayer video)
    {
        videoPreparado = true;

        if (mostrarMensajesEnConsola)
        {
            Debug.Log("Cinemática preparada. Reproduciendo video.");
        }

        ReproducirVideo();
    }


    private void ReproducirVideo()
    {
        reproductorVideo.Play();
    }


    private void CuandoTermineElVideo(VideoPlayer video)
    {
        if (mostrarMensajesEnConsola)
        {
            Debug.Log("La cinemática terminó.");
        }

        CargarEscenaSiguiente();
    }


    private void CuandoOcurraUnError(VideoPlayer video, string mensaje)
    {
        Debug.LogError("Error al reproducir la cinemática: " + mensaje);
    }


    private void CargarEscenaSiguiente()
    {
        if (cambioDeEscenaIniciado)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(nombreEscenaSiguiente))
        {
            Debug.LogError("No asignaste la escena siguiente.");
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(nombreEscenaSiguiente))
        {
            Debug.LogError(
                "La escena '" + nombreEscenaSiguiente +
                "' no está agregada en File > Build Profiles > Scene List."
            );

            return;
        }

        cambioDeEscenaIniciado = true;

        if (mostrarMensajesEnConsola)
        {
            Debug.Log("Cargando escena siguiente: " + nombreEscenaSiguiente);
        }

        SceneManager.LoadSceneAsync(nombreEscenaSiguiente);
    }
}