using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] sfxSoundSource;
    [SerializeField] private static int indiceSFXSource;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);


        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void PlayMusic(AudioStorageSOScript soundMusic)
    {
        if(!soundMusic.Es3D)musicSource.spatialBlend = 0;
        else musicSource.spatialBlend = 1;
        
        
        AudioClip audioAReproducir = soundMusic.ReproducirAleatorio();
        musicSource.clip = audioAReproducir;
        musicSource.Play();
    }

    public void PlaySFX(AudioStorageSOScript soundSFX, Vector3 ubicacionSFX)
    {


        AudioClip audioAReproducir = soundSFX.ReproducirAleatorio();
        indiceSFXSource = indiceSFXSource % sfxSoundSource.Length;


        if (!soundSFX.Es3D) sfxSoundSource[indiceSFXSource].spatialBlend = 0;
        else sfxSoundSource[indiceSFXSource].spatialBlend = 1;
        

        sfxSoundSource[indiceSFXSource].transform.position = ubicacionSFX;

        sfxSoundSource[indiceSFXSource].PlayOneShot(audioAReproducir);

        indiceSFXSource++;
    }
    public void PlaySFX(AudioStorageSOScript soundSFX)
    {


        AudioClip audioAReproducir = soundSFX.ReproducirAleatorio();

        indiceSFXSource = indiceSFXSource % sfxSoundSource.Length;

        if (!soundSFX.Es3D) sfxSoundSource[indiceSFXSource].spatialBlend = 0;
        else sfxSoundSource[indiceSFXSource].spatialBlend = 1;
        

        sfxSoundSource[indiceSFXSource].PlayOneShot(audioAReproducir);

        indiceSFXSource++;
    }


}
