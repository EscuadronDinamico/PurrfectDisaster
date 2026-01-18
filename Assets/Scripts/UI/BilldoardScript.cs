using UnityEngine;

public class BilldoardScript : MonoBehaviour
{

    [SerializeField] private Transform cameraTransform;

    private void LateUpdate()
    {
        if (cameraTransform)
        {
            transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
                cameraTransform.rotation * Vector3.up);
        }
    }
}
