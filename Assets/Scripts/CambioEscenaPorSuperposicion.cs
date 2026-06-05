using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CambioEscenaPorSuperposicion : MonoBehaviour
{
    [Header("ZONA DE CAMBIO")]

    [Tooltip("Arrastra aquí el Box Collider del objeto CambioEscena.")]
    public Collider zonaCambio;


    [Header("COLLIDER DE LA CÁMARA")]

    [Tooltip("Arrastra aquí el Sphere Collider de la Main Camera.")]
    public Collider colliderCamara;


    [Header("ESCENA DE DESTINO")]

#if UNITY_EDITOR
    [Tooltip("Arrastra aquí el archivo .unity desde la ventana Project.")]
    public SceneAsset escenaDestino;
#endif

    [Tooltip("Se completa automáticamente al arrastrar la escena.")]
    public string nombreEscenaDestino;


    [Header("CONFIGURACIÓN")]

    [Tooltip("Evita intentar cargar la escena más de una vez.")]
    public bool activarSoloUnaVez = true;

    [Tooltip("Muestra información en la consola.")]
    public bool mostrarMensajesEnConsola = true;


    [Header("ESTADO ACTUAL")]

    public bool yaSeActivo = false;


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (zonaCambio == null)
        {
            zonaCambio = GetComponent<Collider>();
        }

        if (escenaDestino != null)
        {
            nombreEscenaDestino = escenaDestino.name;
        }
    }
#endif


    private void Start()
    {
        if (zonaCambio == null)
        {
            Debug.LogError("No asignaste el collider de la zona de cambio.");
        }

        if (colliderCamara == null)
        {
            Debug.LogError("No asignaste el collider de la Main Camera.");
        }

        if (string.IsNullOrWhiteSpace(nombreEscenaDestino))
        {
            Debug.LogError("No asignaste la escena de destino.");
        }
    }


    private void Update()
    {
        if (activarSoloUnaVez && yaSeActivo)
        {
            return;
        }

        if (zonaCambio == null || colliderCamara == null)
        {
            return;
        }

        bool estanTocandose =
            zonaCambio.bounds.Intersects(colliderCamara.bounds);

        if (estanTocandose)
        {
            CargarEscena();
        }
    }


    private void CargarEscena()
    {
        if (string.IsNullOrWhiteSpace(nombreEscenaDestino))
        {
            Debug.LogError("No asignaste la escena de destino.");
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(nombreEscenaDestino))
        {
            Debug.LogError(
                "La escena '" + nombreEscenaDestino +
                "' no está agregada en File > Build Profiles > Scene List."
            );

            return;
        }

        yaSeActivo = true;

        if (mostrarMensajesEnConsola)
        {
            Debug.Log(
                "La cámara tocó la zona. Cargando escena: " +
                nombreEscenaDestino
            );
        }

        SceneManager.LoadSceneAsync(nombreEscenaDestino);
    }
}