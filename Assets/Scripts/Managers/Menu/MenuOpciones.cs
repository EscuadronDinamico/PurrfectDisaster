using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MenuOpciones : MonoBehaviour
{


    [SerializeField] private GameObject menuOpciones;

    [SerializeField] private Slider volumenSlider;
    [SerializeField] private Slider brilloSlider;

    [SerializeField] private AudioMixer volumeMixer;

    private void Start()
    {
        
    }


    public void CerrarOpciones()
    {
        menuOpciones.SetActive(false);
    }

    public void CambiarVolumen()
    {
        float volume = volumenSlider.value;

        float dB = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;

        // Aplicamos el valor al Audio Mixer
        volumeMixer.SetFloat("MasterVolume", dB);

        // Guardamos la preferencia
        PlayerPrefs.SetFloat("MasterVolume", dB);

    }

    public void CambiarBrilloOpciones()
    {
        PostProcessingManager.instance.CambiarBrillo(brilloSlider.value);
    }
}
