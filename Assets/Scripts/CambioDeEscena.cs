using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena : MonoBehaviour
{
    [Header("Collider que activa el cambio")]
    public Collider colliderDeLaCamara;

    [Header("Escena que se cargará")]
    public string nombreDeLaEscena;

    [Header("Configuración")]
    public bool cambiarSoloUnaVez = true;
    public bool mostrarMensajesEnConsola = true;

    [Header("Estado actual")]
    public bool escenaYaFueActivada = false;

    public void OnTriggerEnter(Collider otroCollider)
    {
        if (cambiarSoloUnaVez && escenaYaFueActivada)
        {
            return;
        }

        if (otroCollider != colliderDeLaCamara)
        {
            return;
        }

        if (string.IsNullOrEmpty(nombreDeLaEscena))
        {
            Debug.LogError("No asignaste el nombre de la escena en el Inspector.");
            return;
        }

        escenaYaFueActivada = true;

        if (mostrarMensajesEnConsola)
        {
            Debug.Log("Cargando escena: " + nombreDeLaEscena);
        }

        SceneManager.LoadScene(nombreDeLaEscena);
    }
}