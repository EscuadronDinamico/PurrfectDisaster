using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractableAction : MonoBehaviour
{
    [Header("General")]
    public ActionType actionType = ActionType.Push;
    [SerializeField] private string promptText = "Acción";
    [SerializeField] private string keyHint = "E";

    [Header("Empujar (Push)")]
    public Rigidbody pushTarget;
    public float pushForce = 5f;
    public enum PushDirectionMode { UsePlayerForward, FixedDirection }
    public PushDirectionMode pushDirMode = PushDirectionMode.UsePlayerForward;
    public Vector3 fixedPushDirection = Vector3.forward;

    [Header("Salto automático (AutoJump)")]
    public Transform jumpTarget;
    public float jumpArcHeight = 1.5f;
    public float jumpDuration = 0.6f;

    [Header("Escalar automático (AutoClimb)")]
    public Transform climbTarget;
    public float climbDuration = 0.8f;

    [Header("Interact (genérico)")]
    public UnityEvent onInteract;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public string GetPromptText() => $"[{keyHint}] {promptText}";

    public Rigidbody GetPushBody()
    {
        if (pushTarget != null) return pushTarget;
        return GetComponentInParent<Rigidbody>() ?? GetComponent<Rigidbody>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.25f);

        if (actionType == ActionType.AutoJump && jumpTarget)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, jumpTarget.position);
            Gizmos.DrawSphere(jumpTarget.position, 0.1f);
        }

        if (actionType == ActionType.AutoClimb && climbTarget)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, climbTarget.position);
            Gizmos.DrawCube(climbTarget.position, Vector3.one * 0.1f);
        }
    }
}

