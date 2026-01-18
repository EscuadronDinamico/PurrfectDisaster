using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject root; 
    [SerializeField] private TextMeshProUGUI label;
    private void Awake()
    {
        if (root) root.SetActive(false);
    }

    public void Show(string text)
    {
        if (label) label.text = text;
        if (root) root.SetActive(true);
    }

    public void Hide()
    {
        if (root) root.SetActive(false);
    }
}

