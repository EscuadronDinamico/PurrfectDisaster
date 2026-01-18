using UnityEngine;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager Instance;

    public GameObject victoryPanel;
    public Text victoryText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    // Se debe agregar un canva UI para que el mensaje se ejecute
    public void ShowVictoryMessage()
    {
        victoryPanel.SetActive(true);

        victoryText.text =
        "La Navidad no trata de lo que acumulamos, sino de lo que compartimos.\n\n" +
        "Es un recordatorio de que el valor de la vida no está en los objetos,\n" +
        "sino en los vínculos, la generosidad y el tiempo que ofrecemos a otros.\n\n" +
        "Cuando dejamos de correr y nos detenemos a dar, es cuando realmente ganamos.";

        Time.timeScale = 0f;
    }
}

