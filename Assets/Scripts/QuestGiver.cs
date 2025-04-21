using UnityEngine;

// no longer RequireComponent<DialogueInteraction> since we’re not using it
public class QuestGiver : MonoBehaviour, IInteractable
{
    [Header("Quest Settings")]
    [Tooltip("Drag your LearnableObject (the tomb) here")]
    public LearnableObject targetTomb;

    [Tooltip("Drag the empty Transform where you want your ring to appear")]
    public Transform questLocation;

    private bool questStarted = false;

    public void Interact()
    {
        // if we've already given the quest, do nothing
        if (questStarted)
            return;

        questStarted = true;

        // 1) Unlock the tomb immediately
        if (targetTomb != null)
            targetTomb.Unlock();

        // 2) Spawn the quest‑ring
        if (questLocation != null)
            QuestManager.Instance.ShowQuestArea(questLocation.position);

        // 3) (Optional) let the player know the tomb is open
        DialogueUIManager.Instance.ShowDialogue(
          "The tomb is now open! Go learn its secrets.",
          null /* no portrait */
        );
    }
}
