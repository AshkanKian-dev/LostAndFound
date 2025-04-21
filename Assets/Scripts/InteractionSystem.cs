using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 2f;
    public KeyCode interactKey = KeyCode.F;

    [Header("UI")]
    [SerializeField] GameObject interactPromptUI;

    IInteractable nearbyInteractable;

    void Start()
    {
        if (interactPromptUI != null)
            interactPromptUI.SetActive(false);
    }

    void Update()
    {
        DetectInteractable();

        if (Input.GetKeyDown(interactKey) && nearbyInteractable != null)
            nearbyInteractable.Interact();
    }

    void DetectInteractable()
    {
        IInteractable closest = null;
        float closestDist = float.MaxValue;

        foreach (var col in Physics2D.OverlapCircleAll(transform.position, interactRange))
        {
            var inter = col.GetComponent<IInteractable>();
            if (inter == null) continue;

            float d = Vector2.Distance(transform.position, col.transform.position);
            if (d < closestDist)
            {
                closestDist = d;
                closest = inter;
            }
        }

        if (closest == null && nearbyInteractable != null)
            DialogueUIManager.Instance?.HideDialogue();

        nearbyInteractable = closest;

        if (interactPromptUI != null)
            interactPromptUI.SetActive(nearbyInteractable != null);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
