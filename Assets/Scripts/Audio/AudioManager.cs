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


    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, Vector3 ubicacionSFX)
    {
        indiceSFXSource=indiceSFXSource%sfxSoundSource.Length;

        sfxSoundSource[indiceSFXSource].transform.position = ubicacionSFX;

        sfxSoundSource[indiceSFXSource].PlayOneShot(clip);

        indiceSFXSource++;
    }


}
