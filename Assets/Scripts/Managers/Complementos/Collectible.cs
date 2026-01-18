using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CondicionDeVictoria.Instance.collectedItems++;
            Destroy(gameObject);
        }
    }
}
