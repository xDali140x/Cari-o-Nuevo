using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelMenu;
    public GameObject panelSettings;
    public GameObject panelControles;

    [Header("Botones Menu Principal")]
    public Button btnPlay;
    public Button btnSettings;

    [Header("Botones Settings")]
    public Toggle btnMute;
    public Button btnRestart;
    public Button btnQuit;
    public Button btnControles;
    public Button btnCerrarSettings;

    [Header("Botones Controles")]
    public Button btnCerrarControles;

    [Header("Durante el juego")]
    public bool estaJugando = false;
    public Transform cameraTransform;
    public float distanciaFlotante = 1.5f;

    [Header("Input")]
    public InputActionReference botonB;

    [Header("Mute")]
    public Sprite spriteMuteOn;
    public Sprite spriteMuteOff;

    [Header("Locomocion")]
    public CharacterController characterController;
    public MonoBehaviour[] locomotionComponents;

    [Header("Ray Interactors")]
    public GameObject rayInteractorDerecho;
    public GameObject rayInteractorIzquierdo;

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
        // Bloquear movimiento al inicio
        if (characterController != null)
            characterController.enabled = false;

        foreach (var component in locomotionComponents)
            component.enabled = false;
        // Conectar botones
        btnPlay.onClick.AddListener(BotonPlay);
        btnSettings.onClick.AddListener(AbrirSettings);
        btnMute.onValueChanged.AddListener(BotonMute);
        btnRestart.onClick.AddListener(BotonReiniciar);
        btnQuit.onClick.AddListener(BotonSalir);
        btnControles.onClick.AddListener(AbrirControles);
        btnCerrarSettings.onClick.AddListener(CerrarSettings);
        btnCerrarControles.onClick.AddListener(CerrarControles);

        // Estado inicial
        panelMenu.SetActive(true);
        panelSettings.SetActive(false);
        panelControles.SetActive(false);
    }

    void OnBotonB(InputAction.CallbackContext context)
    {
        if (!estaJugando) return;

        if (panelSettings.activeSelf)
            CerrarSettings();
        else
            AbrirSettings();
    }

    public void BotonPlay()
    {
        panelMenu.SetActive(false);
        panelSettings.SetActive(false);
        panelControles.SetActive(false);
        estaJugando = true;
        // Desbloquear movimiento
        if (characterController != null)
            characterController.enabled = true;

        foreach (var component in locomotionComponents)
            component.enabled = true;
        SceneTransitionManager.singleton.GoToSceneAsync(1);
    }

    public void AbrirSettings()
    {
        panelControles.SetActive(false);
        panelSettings.SetActive(true);

        if (estaJugando)
        {
            PosicionarFrenteAlJugador(panelSettings);
            if (rayInteractorDerecho != null) rayInteractorDerecho.SetActive(true);
            if (rayInteractorIzquierdo != null) rayInteractorIzquierdo.SetActive(true);
        }
    }

    public void CerrarSettings()
    {
        panelSettings.SetActive(false);
        if (rayInteractorDerecho != null) rayInteractorDerecho.SetActive(false);
        if (rayInteractorIzquierdo != null) rayInteractorIzquierdo.SetActive(false);

        if (!estaJugando)
            panelMenu.SetActive(true);
    }

    public void AbrirControles()
    {
        panelSettings.SetActive(false);
        panelControles.SetActive(true);

        if (estaJugando)
            PosicionarFrenteAlJugador(panelControles);
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

    public void BotonMute(bool muteado)
    {
        AudioListener.volume = muteado ? 0 : 1;
        btnMute.GetComponentInChildren<Image>().sprite = muteado ? spriteMuteOn : spriteMuteOff;
    }

    public void BotonReiniciar()
    {
        SceneTransitionManager.singleton.GoToSceneAsync(0);
    }

    public void BotonSalir()
    {
        Application.Quit();
    }
}