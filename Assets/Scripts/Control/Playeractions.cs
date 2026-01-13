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
    public InputAction interactAction; // asigna en Inspector (Keyboard E, Gamepad South)

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
        if (busy) return;
        if (currentInteractable == null) return;

        // Mostrar prompt
        if (promptUI) promptUI.Show(currentInteractable.GetPromptText());

        // Presionar tecla
        if ((interactAction != null && interactAction.WasPressedThisFrame()) ||
            (interactAction == null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame))
        {
            StartCoroutine(Execute(currentInteractable));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var ia = other.GetComponent<InteractableAction>();
        if (ia != null)
        {
            currentInteractable = ia;
            if (promptUI) promptUI.Show(ia.GetPromptText());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var ia = other.GetComponent<InteractableAction>();
        if (ia != null && ia == currentInteractable)
        {
            currentInteractable = null;
            if (promptUI) promptUI.Hide();
        }
    }

    private IEnumerator Execute(InteractableAction ia)
    {
        busy = true;
        if (movementControllerToDisable) movementControllerToDisable.enabled = false;

        // Orientar hacia el objeto
        yield return StartCoroutine(RotateTowards(ia.transform.position));

        switch (ia.actionType)
        {
            case ActionType.Push:
                ExecutePush(ia);
                break;
            case ActionType.AutoJump:
                yield return StartCoroutine(ExecuteAutoJump(ia));
                break;
            case ActionType.AutoClimb:
                yield return StartCoroutine(ExecuteAutoClimb(ia));
                break;
            case ActionType.Interact:
                ia.onInteract?.Invoke();
                break;
        }

        if (movementControllerToDisable) movementControllerToDisable.enabled = true;
        busy = false;
    }

    private IEnumerator RotateTowards(Vector3 worldTarget)
    {
        Vector3 to = (worldTarget - transform.position);
        to.y = 0f;
        if (to.sqrMagnitude < 0.0001f) yield break;

        Quaternion targetRot = Quaternion.LookRotation(to.normalized);
        while (Quaternion.Angle(transform.rotation, targetRot) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateToTargetSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void ExecutePush(InteractableAction ia)
    {
        var rb = ia.GetPushBody();
        if (!rb) return;

        Vector3 dir = ia.pushDirMode == InteractableAction.PushDirectionMode.UsePlayerForward
            ? transform.forward
            : ia.fixedPushDirection.normalized;

        rb.AddForce(dir * ia.pushForce, ForceMode.Impulse);
    }

    private IEnumerator ExecuteAutoJump(InteractableAction ia)
    {
        if (!ia.jumpTarget) yield break;

        Vector3 start = transform.position;
        Vector3 end = ia.jumpTarget.position;
        float duration = Mathf.Max(0.05f, ia.jumpDuration);
        float t = 0f;

        bool hadCC = cc && cc.enabled;
        if (hadCC) cc.enabled = false;

        while (t < duration)
        {
            t += Time.deltaTime;
            float norm = Mathf.Clamp01(t / duration);

            Vector3 pos = Vector3.Lerp(start, end, norm);
            float arc = Mathf.Sin(norm * Mathf.PI) * ia.jumpArcHeight;
            pos.y += arc;

            transform.position = pos;
            yield return null;
        }

        transform.position = end;
        if (hadCC) cc.enabled = true;
    }

    private IEnumerator ExecuteAutoClimb(InteractableAction ia)
    {
        if (!ia.climbTarget) yield break;

        Vector3 start = transform.position;
        Vector3 end = ia.climbTarget.position;
        float duration = Mathf.Max(0.05f, ia.climbDuration);
        float t = 0f;

        bool hadCC = cc && cc.enabled;
        if (hadCC) cc.enabled = false;

        while (t < duration)
        {
            t += Time.deltaTime;
            float norm = Mathf.Clamp01(t / duration);
            transform.position = Vector3.Lerp(start, end, norm);
            yield return null;
        }

        transform.position = end;
        if (hadCC) cc.enabled = true;
    }
}
