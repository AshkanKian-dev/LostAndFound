using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public float interactRange = 2f;
    private IInteractable nearbyInteractable;  // Stores the currently interactable object

    void Update()
    {
        DetectInteractable();

        // Check if player presses "F" to interact
        if (Input.GetKeyDown(KeyCode.F) && nearbyInteractable != null)
        {
            nearbyInteractable.Interact();
        }
    }

    void DetectInteractable()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRange);
        foreach (Collider2D col in colliders)
        {
            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
            {
                nearbyInteractable = interactable;  // Register this interactable object
                return;  // Exit loop as we've found an interactable object
            }
        }
        nearbyInteractable = null;  // No interactable found within range
    }

    // Register an interactable object (called when the player is in range)
    public void RegisterInteractable(IInteractable interactable)
    {
        nearbyInteractable = interactable;  // Store the interactable object
    }
}
