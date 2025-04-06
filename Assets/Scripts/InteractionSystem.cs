using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public float interactRange = 2f;
    public KeyCode interactKey = KeyCode.F;
    private IInteractable nearbyInteractable;

    void Update()
    {
        DetectInteractable();

        if (Input.GetKeyDown(interactKey) && nearbyInteractable != null)
        {
            nearbyInteractable.Interact();
        }
    }

    void DetectInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);
        float closestDist = Mathf.Infinity;
        IInteractable closest = null;

        foreach (Collider2D col in hits)
        {
            if (!col.isTrigger) continue;

            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
            {
                float dist = Vector2.Distance(transform.position, col.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = interactable;
                }
            }
        }

        nearbyInteractable = closest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
