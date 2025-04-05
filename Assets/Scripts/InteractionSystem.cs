using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public float interactRange = 2f;
    private IInteractable nearbyInteractable;

    void Update()
    {
        DetectInteractable();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (nearbyInteractable != null)
            {
                Debug.Log($"[InteractionSystem] Interacting with: {nearbyInteractable}");
                nearbyInteractable.Interact();
            }
            else
            {
                Debug.Log("[InteractionSystem] No object to interact with.");
            }
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
                nearbyInteractable = interactable;
                return;
            }
        }
        nearbyInteractable = null;
    }

    public void RegisterInteractable(IInteractable interactable)
    {
        nearbyInteractable = interactable;
    }
}
