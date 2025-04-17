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

        if (Input.GetKeyDown(interactKey) && nearbyInteractable != null)
            nearbyInteractable.Interact();
    }

    void DetectInteractable()
    {
        nearbyInteractable = null;
        float closestDist = float.MaxValue;

        // grab every Collider2D (including triggers) within range
        foreach (var col in Physics2D.OverlapCircleAll(transform.position, interactRange))
        {
            var interactable = col.GetComponent<IInteractable>();
            if (interactable == null) continue;

            float d = Vector2.Distance(transform.position, col.transform.position);
            if (d < closestDist)
            {
                closestDist = d;
                nearbyInteractable = interactable;
            }
        }

        if (interactPromptUI != null)
            interactPromptUI.SetActive(nearbyInteractable != null);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
