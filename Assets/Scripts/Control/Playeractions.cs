using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public MonoBehaviour movementControllerToDisable;
    public InteractionPromptUI promptUI;
    public float rotateToTargetSpeed = 8f;

    [Header("Input")]
    public InputAction interactAction;

    private CharacterController cc;
    private InteractableAction currentInteractable;
    private bool busy;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        interactAction?.Enable();
    }

    private void Update()
    {
        if (busy || currentInteractable == null) return;

        if (promptUI)
            promptUI.Show(currentInteractable.GetPromptText());

        if ((interactAction != null && interactAction.WasPressedThisFrame()) ||
            (interactAction == null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame))
        {
            StartCoroutine(Execute(currentInteractable));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var ia = other.GetComponent<InteractableAction>();
        if (!ia) return;

        currentInteractable = ia;
        if (promptUI) promptUI.Show(ia.GetPromptText());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractableAction>() == currentInteractable)
        {
            currentInteractable = null;
            if (promptUI) promptUI.Hide();
        }
    }

    private IEnumerator Execute(InteractableAction ia)
    {
        busy = true;

        if (movementControllerToDisable)
            movementControllerToDisable.enabled = false;

        if (cc) cc.enabled = false;

        if (!ia.actionStartPoint)
        {
            Debug.LogWarning($"{ia.name} no tiene ActionStartPoint asignado.");
            EndExecution();
            yield break;
        }

     
        transform.position = ia.actionStartPoint.position;

     
        yield return RotateTowards(ia.actionStartPoint.position);

        switch (ia.actionType)
        {
            case ActionType.Push:
                ExecutePush(ia);
                break;

            case ActionType.AutoJump:
                yield return ExecuteAutoJump(ia);
                break;

            case ActionType.AutoClimb:
                yield return ExecuteAutoClimb(ia);
                break;

            case ActionType.Interact:
                ia.onInteract?.Invoke();
                break;
        }

        EndExecution();
    }

    private void EndExecution()
    {
        if (cc) cc.enabled = true;
        if (movementControllerToDisable)
            movementControllerToDisable.enabled = true;
        busy = false;
    }

    private IEnumerator RotateTowards(Vector3 worldTarget)
    {
        Vector3 dir = worldTarget - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            yield break;

        Quaternion targetRot = Quaternion.LookRotation(dir.normalized);

        while (Quaternion.Angle(transform.rotation, targetRot) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateToTargetSpeed * Time.deltaTime
            );
            yield return null;
        }
    }

    private void ExecutePush(InteractableAction ia)
    {
        var body = ia.GetPushBody();
        if (!body) return;

        transform.position = ia.actionStartPoint.position;
        transform.rotation = Quaternion.LookRotation(ia.transform.forward);

        Vector3 dir = ia.pushDirMode == InteractableAction.PushDirectionMode.UsePlayerForward
            ? transform.forward
            : ia.fixedPushDirection.normalized;

        body.AddForce(dir * ia.pushForce, ForceMode.Impulse);
    }

    private IEnumerator ExecuteAutoJump(InteractableAction ia)
    {
        if (!ia.actionEndPoint) yield break;

        Vector3 start = ia.actionStartPoint.position;
        Vector3 end = ia.actionEndPoint.position;

        float t = 0f;
        float duration = Mathf.Max(0.05f, ia.jumpDuration);

        while (t < duration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / duration);

            Vector3 pos = Vector3.Lerp(start, end, n);
            pos.y += Mathf.Sin(n * Mathf.PI) * ia.jumpArcHeight;

            transform.position = pos;
            yield return null;
        }

        transform.position = end;
    }

    private IEnumerator ExecuteAutoClimb(InteractableAction ia)
    {
        if (!ia.actionEndPoint) yield break;

        Vector3 start = ia.actionStartPoint.position;
        Vector3 end = ia.actionEndPoint.position;

        float t = 0f;
        float duration = Mathf.Max(0.05f, ia.climbDuration);

        while (t < duration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / duration);
            transform.position = Vector3.Lerp(start, end, n);
            yield return null;
        }

        transform.position = end;
    }
}

