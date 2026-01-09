using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{


    [Header("Referencias")]
    [Tooltip("El transform del personaje que la cámara seguirá")]
    public Transform player;

    [Tooltip("El pivote que rotará con el input - debe ser hijo del jugador")]
    public Transform cameraPivot;

    [Header("Configuración de sensibilidad")]
    [Tooltip("Qué tan rápido rota la cámara horizontalmente")]
    public float horizontalSensitivity = 2f;

    [Tooltip("Qué tan rápido rota la cámara verticalmente")]
    public float verticalSensitivity = 2f;

    [Header("Límites de rotación vertical")]
    [Tooltip("Ángulo mínimo (mirando hacia abajo)")]
    public float minVerticalAngle = -10f;

    [Tooltip("Ángulo máximo (mirando hacia arriba)")]
    public float maxVerticalAngle = 45f;

    [Header("Opciones adicionales")]
    [Tooltip("Invertir el eje vertical si el jugador lo prefiere")]
    public bool invertVertical = false;

    // Variables privadas para tracking de rotación
    private float currentHorizontalRotation = 0f;
    private float currentVerticalRotation = 0f;

    // Input del nuevo Input System
    private Vector2 lookInput;

    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        playerInput.actions["Look"].performed += OnLook;
        playerInput.actions["Look"].canceled += OnLook;

    }
    private void OnDisable()
    {
        playerInput.actions["Look"].performed -= OnLook;
        playerInput.actions["Look"].canceled -= OnLook;

    }

    void Start()
    {
        // Inicializamos las rotaciones actuales basadas en la rotación inicial del pivote
        // Esto es importante si quieres que la cámara empiece en un ángulo específico
        Vector3 initialRotation = cameraPivot.localEulerAngles;
        currentVerticalRotation = initialRotation.x;
        currentHorizontalRotation = initialRotation.y;

        // Si la rotación está en el rango 180-360, la convertimos a negativa
        // Esto es necesario porque Unity representa ángulos en el rango 0-360
        // pero nosotros trabajamos con -180 a 180 para facilitar los límites
        if (currentVerticalRotation > 180f)
        {
            currentVerticalRotation -= 360f;
        }
    }

    void Update()
    {
        // Solo procesamos la rotación de cámara si tenemos input
        if (lookInput.sqrMagnitude > 0.01f)
        {
            RotateCamera();
        }
    }

    private void RotateCamera()
    {
        // Calculamos cuánto queremos rotar este frame
        // Multiplicamos por Time.deltaTime para que sea independiente del framerate
        // Esto asegura que la cámara se mueva a la misma velocidad sin importar
        // si el juego corre a 30 FPS o 144 FPS
        float horizontalRotation = lookInput.x * horizontalSensitivity;
        float verticalRotation = lookInput.y * verticalSensitivity;

        // Aplicamos la inversión del eje vertical si está activada
        if (invertVertical)
        {
            verticalRotation = -verticalRotation;
        }

        // Actualizamos nuestras rotaciones acumuladas
        currentHorizontalRotation += horizontalRotation;
        currentVerticalRotation -= verticalRotation; // Restamos porque en Unity, ángulos positivos rotan hacia abajo

        // Aplicamos los límites verticales usando Mathf.Clamp
        // Esto asegura que currentVerticalRotation nunca sea menor que minVerticalAngle
        // ni mayor que maxVerticalAngle
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, minVerticalAngle, maxVerticalAngle);

        // Aplicamos la rotación al pivote de la cámara
        // Usamos Quaternion.Euler para crear la rotación desde ángulos
        // El orden es importante: primero la rotación vertical (X), luego horizontal (Y)
        cameraPivot.localRotation = Quaternion.Euler(currentVerticalRotation, currentHorizontalRotation, 0f);
    }

    // Este método es llamado por el nuevo Input System cuando detecta input de "Look"
    public void OnLook(InputAction.CallbackContext context)
    {
        // Leemos el valor del input como Vector2
        // X será el movimiento horizontal, Y será el movimiento vertical
        lookInput = context.ReadValue<Vector2>();
    }
}

