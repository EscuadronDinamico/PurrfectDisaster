using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource musicSource, sfxSoundSource;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);


        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSoundSource.PlayOneShot(clip);
    }


}
