using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelMenu;
    public GameObject panelSettings;
    public GameObject panelControles;

    [Header("Durante el juego")]
    public bool estaJugando = false;
    public Transform cameraTransform;
    public float distanciaFlotante = 1.5f;

    [Header("Input")]
    public InputActionReference botonB;

    void OnEnable()
    {
        botonB.action.Enable();
        botonB.action.performed += OnBotonB;
    }

    void OnDisable()
    {
        botonB.action.performed -= OnBotonB;
        botonB.action.Disable();
    }

    void Start()
    {
        panelMenu.SetActive(true);
        panelSettings.SetActive(false);
        panelControles.SetActive(false);
    }

    void OnBotonB(InputAction.CallbackContext context)
    {
        if (!estaJugando) return;

        if (panelSettings.activeSelf)
        {
            CerrarSettings();
        }
        else
        {
            AbrirSettings();
        }
    }

    public void BotonPlay()
    {
        panelMenu.SetActive(false);
        panelSettings.SetActive(false);
        panelControles.SetActive(false);
        estaJugando = true;
    }

    public void AbrirSettings()
    {
        panelControles.SetActive(false);
        panelSettings.SetActive(true);

        if (estaJugando)
        {
            PosicionarFrenteAlJugador(panelSettings);
        }
    }

    public void CerrarSettings()
    {
        panelSettings.SetActive(false);

        if (!estaJugando)
            panelMenu.SetActive(true);
    }

    public void AbrirControles()
    {
        panelSettings.SetActive(false);
        panelControles.SetActive(true);

        if (estaJugando)
        {
            PosicionarFrenteAlJugador(panelControles);
        }
    }

    public void CerrarControles()
    {
        panelControles.SetActive(false);
        panelSettings.SetActive(true);
    }

    void PosicionarFrenteAlJugador(GameObject panel)
    {
        panel.transform.position = cameraTransform.position + cameraTransform.forward * distanciaFlotante;
        panel.transform.rotation = Quaternion.LookRotation(panel.transform.position - cameraTransform.position);
    }

    public void BotonMute()
    {
        AudioListener.volume = AudioListener.volume > 0 ? 0 : 1;
    }

    public void BotonReiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BotonSalir()
    {
        Application.Quit();
    }
}