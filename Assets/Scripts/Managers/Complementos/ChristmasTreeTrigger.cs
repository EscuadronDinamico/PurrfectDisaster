using UnityEngine;

public class ChristmasTreeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (CondicionDeVictoria.Instance.HasAllItems())
        {
            VictoryManager.Instance.ShowVictoryMessage();
        }
        else
        {
            Debug.Log("Aún no estás listo para este momento.");
        }
    }
}
