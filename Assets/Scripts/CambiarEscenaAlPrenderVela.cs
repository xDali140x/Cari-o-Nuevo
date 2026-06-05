using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CambiarEscenaAlPrenderVela : MonoBehaviour
{
    [Header("DETECCIÓN DEL ENCENDIDO")]

    [Tooltip("Arrastra aquí las partículas del fuego de la vela.")]
    public ParticleSystem particulasFuegoVela;

    [Tooltip("Déjalo activado si la vela se prende reproduciendo partículas.")]
    public bool detectarPorParticulas = true;


    [Header("OPCIÓN ALTERNATIVA")]

    [Tooltip("Úsalo solamente si al prender la vela se activa un GameObject completo.")]
    public GameObject objetoFuegoVela;

    [Tooltip("Actívalo solamente si el objeto del fuego comienza desactivado y luego aparece.")]
    public bool detectarPorObjetoActivado = false;


    [Header("ESCENA DE CINEMÁTICA")]

#if UNITY_EDITOR
    [Tooltip("Arrastra aquí el archivo .unity de la cinemática.")]
    public SceneAsset escenaCinematica;
#endif

    [Tooltip("Se rellena automáticamente al arrastrar la escena.")]
    public string nombreEscenaCinematica;


    [Header("CONFIGURACIÓN")]

    [Tooltip("Tiempo visible de la llama antes de cambiar de escena.")]
    public float segundosAntesDeCambiar = 1.5f;

    [Tooltip("Muestra mensajes útiles en la consola.")]
    public bool mostrarMensajesEnConsola = true;


    [Header("ESTADO ACTUAL")]

    public bool yaSeActivo = false;


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (escenaCinematica != null)
        {
            nombreEscenaCinematica = escenaCinematica.name;
        }
    }
#endif


    private void Update()
    {
        if (yaSeActivo)
        {
            return;
        }

        bool velaEstaPrendida = false;

        if (
            detectarPorParticulas &&
            particulasFuegoVela != null &&
            particulasFuegoVela.isPlaying
        )
        {
            velaEstaPrendida = true;
        }

        if (
            detectarPorObjetoActivado &&
            objetoFuegoVela != null &&
            objetoFuegoVela.activeInHierarchy
        )
        {
            velaEstaPrendida = true;
        }

        if (velaEstaPrendida)
        {
            IniciarCambioDeEscena();
        }
    }


    public void IniciarCambioDeEscena()
    {
        if (yaSeActivo)
        {
            return;
        }

        yaSeActivo = true;

        if (mostrarMensajesEnConsola)
        {
            Debug.Log(
                "La vela se prendió. Cargando cinemática en " +
                segundosAntesDeCambiar +
                " segundos."
            );
        }

        StartCoroutine(CargarCinematicaConRetraso());
    }


    private IEnumerator CargarCinematicaConRetraso()
    {
        yield return new WaitForSeconds(segundosAntesDeCambiar);

        if (string.IsNullOrWhiteSpace(nombreEscenaCinematica))
        {
            Debug.LogError("No asignaste la escena de cinemática.");
            yaSeActivo = false;
            yield break;
        }

        if (!Application.CanStreamedLevelBeLoaded(nombreEscenaCinematica))
        {
            Debug.LogError(
                "La escena '" + nombreEscenaCinematica +
                "' no está agregada en File > Build Profiles > Scene List."
            );

            yaSeActivo = false;
            yield break;
        }

        if (mostrarMensajesEnConsola)
        {
            Debug.Log("Cargando cinemática: " + nombreEscenaCinematica);
        }

        SceneManager.LoadSceneAsync(nombreEscenaCinematica);
    }
}