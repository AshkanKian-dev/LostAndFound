using System.Collections.Generic;
using UnityEngine;

public class NPCInteractionManager : MonoBehaviour
{
    // Drag and drop your NPCs (which have the NPCDialogue component) here
    public List<NPCDialogue> npcList = new List<NPCDialogue>();

    // Reference to your player's Transform (drag your player GameObject here)
    public Transform playerTransform;

    // Maximum distance for an NPC to be considered "in range"
    public float interactionDistance = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Get the closest NPC in range and trigger its interaction
            NPCDialogue closestNPC = GetClosestNPC();
            if (closestNPC != null)
            {
                closestNPC.Interact();
            }
        }
    }

    // Find the closest NPC within the interactionDistance
    NPCDialogue GetClosestNPC()
    {
        NPCDialogue closestNPC = null;
        float minDistance = interactionDistance;
        foreach (NPCDialogue npc in npcList)
        {
            if (npc == null)
                continue;

            float distance = Vector3.Distance(playerTransform.position, npc.transform.position);
            if (distance <= minDistance)
            {
                minDistance = distance;
                closestNPC = npc;
            }
        }
        return closestNPC;
    }
}
