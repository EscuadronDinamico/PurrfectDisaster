using UnityEngine;
using UnityEngine.InputSystem;

public class PausaScript : MonoBehaviour
{

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject menuPausa;


    private void Awake()
    {
        playerInput.actions["Pausa"].performed += TogglePausa;
    }

    private void TogglePausa(InputAction.CallbackContext context)
    {
        print(menuPausa.activeSelf);
        if (menuPausa.activeSelf)
        {
            print(menuPausa.activeSelf);
            menuPausa.SetActive(false);
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            print("Pausar");
            menuPausa.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
    }
    private void OnDisable()
    {
        playerInput.actions["Pausa"].performed -= TogglePausa;
    }
}
