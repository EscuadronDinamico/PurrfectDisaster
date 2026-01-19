using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractableAction : MonoBehaviour
{
    [Header("General")]
    public ActionType actionType = ActionType.Push;
    [SerializeField] private string promptText = "Acción";
    [SerializeField] private string keyHint = "E";

    [Header("Puntos de acción (OBLIGATORIOS)")]
    public Transform actionStartPoint;
    public Transform actionEndPoint;

    [Header("Empujar (Push)")]
    public Rigidbody pushTarget;
    public float pushForce = 5f;
    public enum PushDirectionMode { UsePlayerForward, FixedDirection }
    public PushDirectionMode pushDirMode = PushDirectionMode.UsePlayerForward;
    public Vector3 fixedPushDirection = Vector3.forward;

    [Header("Salto automático (AutoJump)")]
    public float jumpArcHeight = 1.5f;
    public float jumpDuration = 0.6f;

    [Header("Escalar automático (AutoClimb)")]
    public float climbDuration = 0.8f;

    [Header("Interact (genérico)")]
    public UnityEvent onInteract;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public string GetPromptText()
    {
        return $"[{keyHint}] {promptText}";
    }

    public Rigidbody GetPushBody()
    {
        if (pushTarget != null) return pushTarget;
        return GetComponentInParent<Rigidbody>() ?? GetComponent<Rigidbody>();
    }

    private void OnDrawGizmosSelected()
    {
        if (!actionStartPoint) return;

        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(actionStartPoint.position, 0.15f);

        
        if (actionEndPoint)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(actionStartPoint.position, actionEndPoint.position);
            Gizmos.DrawSphere(actionEndPoint.position, 0.15f);
        }

        
        if (actionType == ActionType.Push)
        {
            Gizmos.color = Color.red;
            Vector3 dir = pushDirMode == PushDirectionMode.UsePlayerForward
                ? transform.forward
                : fixedPushDirection.normalized;
            Gizmos.DrawRay(actionStartPoint.position, dir);
        }
    }
}




