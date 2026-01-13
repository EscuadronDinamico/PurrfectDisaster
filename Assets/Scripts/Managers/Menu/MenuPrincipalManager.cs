using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private GameObject menuOpciones;
    public void BotonJugar()
    {
        SceneManager.LoadScene("nivel1");
    }

    public void BotonSalir()
    {
        Application.Quit();
    }
    public void BotonCreditos()
    {
        SceneManager.LoadScene("MenuCreditos");
    }
    public void BotonOpciones()
    {
        menuOpciones.SetActive(true);
    }

}
