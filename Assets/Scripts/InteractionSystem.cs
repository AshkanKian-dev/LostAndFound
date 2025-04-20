using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [Header("Interaction Settings")]
    [Tooltip("How far from the player we can interact.")]
    public float interactRange = 2f;
    public KeyCode interactKey = KeyCode.F;

    [Header("UI")]
    [SerializeField] private GameObject interactPromptUI;

    private IInteractable nearbyInteractable;

    void Start()
    {
        if (interactPromptUI != null)
            interactPromptUI.SetActive(false);
    }

    void Update()
    {
        DetectInteractable();

        // if you press F and something is in range, interact
        if (Input.GetKeyDown(interactKey) && nearbyInteractable != null)
            nearbyInteractable.Interact();
    }

    void DetectInteractable()
    {
        // find the closest IInteractable
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

        // if we just left an interactable, hide its dialogue
        if (closest == null && nearbyInteractable != null)
        {
            DialogueUIManager.Instance?.HideDialogue();
        }

        nearbyInteractable = closest;

        // toggle the “Press F” prompt
        if (interactPromptUI != null)
            interactPromptUI.SetActive(nearbyInteractable != null);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
