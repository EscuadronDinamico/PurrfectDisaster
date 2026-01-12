using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{

    public static PostProcessingManager instance;

    [Header("Configuración")]
    public Volume globalVolume;
    private ColorAdjustments _colorAdjustments;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if(globalVolume.profile.TryGet<ColorAdjustments>(out var colorAdjustments))
            {
                _colorAdjustments = colorAdjustments;
                float BrilloGuardado = PlayerPrefs.GetFloat("BrilloGuardado", 0f);
                CambiarBrillo(BrilloGuardado);
            }

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CambiarBrillo(float cantidad)
    {
        _colorAdjustments.postExposure.value = cantidad;
        PlayerPrefs.SetFloat("BrilloGuardado", cantidad);
    }

}
