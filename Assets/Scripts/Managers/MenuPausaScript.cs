using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausaScript : MonoBehaviour
{

    [SerializeField] private GameObject menuPausa;


    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Salir()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

}
