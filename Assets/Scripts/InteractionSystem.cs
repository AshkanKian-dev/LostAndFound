using System.Collections.Generic;
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRange);
        IInteractable closest = null;
        float closestDistance = float.MaxValue;

        foreach (var col in colliders)
        {
            Debug.Log("Checking collider: " + col.gameObject.name);

            IInteractable interactable = col.GetComponent<IInteractable>();

            if (interactable != null)
            {
                float distance = Vector2.Distance(transform.position, col.transform.position);
                Debug.Log("Found interactable: " + col.gameObject.name + " at distance " + distance);

                if (distance < closestDistance)
                {
                    closest = interactable;
                    closestDistance = distance;
                }
            }
            else
            {
                Debug.Log(col.gameObject.name + " has no IInteractable.");
            }
        }

        nearbyInteractable = closest;

        if (nearbyInteractable != null)
        {
            Debug.Log("Nearest interactable: " + ((MonoBehaviour)nearbyInteractable).gameObject.name);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
