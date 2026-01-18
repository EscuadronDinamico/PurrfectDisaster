using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{


    [Header("Referencias")]
    [Tooltip("El transform del personaje que la cámara seguirá")]
    [SerializeField] private Transform player;

    [Tooltip("El pivote que rotará con el input - debe ser hijo del jugador")]
    [SerializeField] private Transform cameraPivot;

    [Header("Configuración de sensibilidad")]
    [Tooltip("Qué tan rápido rota la cámara horizontalmente")]
    [SerializeField] private float horizontalSensitivity = 2f;

    [Tooltip("Qué tan rápido rota la cámara verticalmente")]
    [SerializeField] private float verticalSensitivity = 2f;

    [Header("Suavizado de rotación")]
    [Tooltip("Tiempo aproximado para alcanzar la rotación objetivo (más alto = más suave)")]
    [Range(0.01f, 0.5f)]
    [SerializeField] private float smoothTime = 0.12f;

    [Header("Límites de rotación vertical")]
    [Tooltip("Ángulo mínimo (mirando hacia abajo)")]
    [SerializeField] private float minVerticalAngle = -10f;

    [Tooltip("Ángulo máximo (mirando hacia arriba)")]
    [SerializeField] private float maxVerticalAngle = 45f;

    [Header("Opciones adicionales")]
    [Tooltip("Invertir el eje vertical si el jugador lo prefiere")]
    [SerializeField] private bool invertVertical = false;

    // Variables privadas para las rotaciones actuales (donde está la cámara ahora)
    private float currentHorizontalRotation = 0f;
    private float currentVerticalRotation = 0f;

    // Variables privadas para las rotaciones objetivo (hacia donde queremos ir)
    private float targetHorizontalRotation = 0f;
    private float targetVerticalRotation = 0f;

    // CRÍTICO: Variables de velocidad para SmoothDamp
    // Estas DEBEN ser variables de instancia que persistan entre frames
    // SmoothDamp las usa y las modifica para mantener la continuidad del movimiento
    private float horizontalVelocity = 0f;
    private float verticalVelocity = 0f;


    [Header("Camaras disponibles")]
    [Tooltip("Camara over the shoulder o en encima del hombro")]
    [SerializeField] private CinemachineCamera OverTheShoulderCamera;
    [Tooltip("Camara en tercera persona normal")]
    [SerializeField] private CinemachineCamera camara3raPersona;

    // Input del nuevo Input System
    private Vector2 lookInput;

    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        playerInput.actions["Look"].performed += OnLook;
        playerInput.actions["Look"].canceled += OnLook;
        playerInput.actions["CambioCamara"].performed += CambioCamara;
    }

    private void OnDisable()
    {
        playerInput.actions["Look"].performed -= OnLook;
        playerInput.actions["Look"].canceled -= OnLook;
        playerInput.actions["CambioCamara"].performed -= CambioCamara;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Obtenemos la rotación inicial del pivote
        Vector3 initialRotation = cameraPivot.localEulerAngles;

        // IMPORTANTE: En Unity Euler angles, X es pitch (arriba/abajo) y Y es yaw (izquierda/derecha)
        currentVerticalRotation = initialRotation.x;
        currentHorizontalRotation = initialRotation.y;

        // Inicializamos los objetivos con los mismos valores que las rotaciones actuales
        targetVerticalRotation = initialRotation.x;
        targetHorizontalRotation = initialRotation.y;

        // Convertimos ángulos del rango 180-360 a valores negativos
        // Esto nos permite trabajar con el rango -180 a 180 que es más intuitivo
        if (currentVerticalRotation > 180f)
        {
            currentVerticalRotation -= 360f;
            targetVerticalRotation -= 360f;
        }

        if (currentHorizontalRotation > 180f)
        {
            currentHorizontalRotation -= 360f;
            targetHorizontalRotation -= 360f;
        }

        
    }

    void Update()
    {
        // Primero actualizamos las rotaciones objetivo basadas en el input
        UpdateTargetRotations();

        // Luego suavizamos el movimiento hacia esas rotaciones objetivo
        SmoothRotateToTarget();
    }

    private void UpdateTargetRotations()
    {
        // Solo procesamos el input si hay movimiento significativo
        if (lookInput.sqrMagnitude > 0.01f)
        {
            // Calculamos cuánto cambiar la rotación objetivo este frame
            // NO multiplicamos por Time.deltaTime aquí porque el input ya viene
            // en la escala correcta y SmoothDamp maneja el tiempo internamente
            float horizontalChange = lookInput.x * horizontalSensitivity;
            float verticalChange = lookInput.y * verticalSensitivity;

            // Aplicamos la inversión del eje vertical si está activada
            if (invertVertical)
            {
                verticalChange = -verticalChange;
            }

            // Actualizamos las rotaciones objetivo
            targetHorizontalRotation += horizontalChange;
            targetVerticalRotation -= verticalChange;

            // Aplicamos los límites verticales al OBJETIVO, no a la rotación actual
            // Esto previene que el objetivo vaya más allá de los límites permitidos
            targetVerticalRotation = Mathf.Clamp(targetVerticalRotation, minVerticalAngle, maxVerticalAngle);
        }
    }

    private void SmoothRotateToTarget()
    {
        // Usamos SmoothDamp para interpolar suavemente desde la rotación actual
        // hacia la rotación objetivo
        // IMPORTANTE: Usamos las variables de velocidad que declaramos arriba
        // SmoothDamp las lee Y las modifica, por eso necesitan persistir entre frames
        currentHorizontalRotation = Mathf.SmoothDamp(
            currentHorizontalRotation,      // Dónde estamos ahora
            targetHorizontalRotation,        // Hacia dónde queremos ir
            ref horizontalVelocity,          // La velocidad actual (SmoothDamp la actualiza)
            smoothTime                       // Cuánto tiempo toma alcanzar el objetivo
        );

        currentVerticalRotation = Mathf.SmoothDamp(
            currentVerticalRotation,
            targetVerticalRotation,
            ref verticalVelocity,
            smoothTime
        );

        // Finalmente aplicamos las rotaciones suavizadas al pivote
        cameraPivot.localRotation = Quaternion.Euler(currentVerticalRotation, currentHorizontalRotation, 0f);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // Leemos el input del jugador
        lookInput = context.ReadValue<Vector2>();
    }

    private void CambioCamara(InputAction.CallbackContext contexto)
    {
        float valorCamaraShoulder=OverTheShoulderCamera.Priority;
        float valorCamara3raPersona=camara3raPersona.Priority;
        if (valorCamaraShoulder> valorCamara3raPersona)
        {
            OverTheShoulderCamera.Priority = 9;
            camara3raPersona.Priority = 10;
        }
        else
        {
            OverTheShoulderCamera.Priority = 10;
            camara3raPersona.Priority = 9;
        }
        //cambio de prioridad de camaras

    }
}