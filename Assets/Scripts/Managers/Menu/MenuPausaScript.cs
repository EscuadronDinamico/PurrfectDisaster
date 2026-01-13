using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausaScript : MonoBehaviour
{

    [SerializeField] private GameObject menuPausa;

    [SerializeField] private GameObject menuOpciones;



    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Salir()
    {
        SceneManager.LoadScene("MenuPrincipal");
        Time.timeScale = 1f;
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void AbrirOpciones()
    {
        menuOpciones.SetActive(true);
    }

}
