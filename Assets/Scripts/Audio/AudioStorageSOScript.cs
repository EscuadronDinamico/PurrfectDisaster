using UnityEngine;

[CreateAssetMenu(fileName ="AudioStorage", menuName = "ScriptableObjects/AudioStorageSOScript", order = 1)]
public class AudioStorageSOScript : ScriptableObject
{

    [SerializeField] private AudioClip[] musicClips;

    [Range(0f,1f)]
    [SerializeField] private float volumenMusica=0.5f;

    [SerializeField] private bool es3D = false;
    public bool Es3D => es3D;

    public AudioClip ReproducirAleatorio()
    {
               int indiceAleatorio = Random.Range(0, musicClips.Length+1);
        return musicClips[indiceAleatorio];
    }
}
