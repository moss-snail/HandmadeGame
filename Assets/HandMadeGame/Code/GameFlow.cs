using System.Collections.Generic;
using UnityEngine;

public sealed class GameFlow : MonoBehaviour
{
    public static GameFlow Instance { get; private set; }

    [HideInInspector]
    public List<NestItem> Inventory = new();

    public DialogueController DialogueController;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Multiple {nameof(GameFlow)} instances exist in the scene!");
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void HandleInteraction(Quest quest)
    {
        Debug.Log($"Interacting with quest '{quest.name}'");
        DialogueController.StartDialogue(quest.CharacterPortrait, quest.QuestRejectedDialogue);
    }

    public void HandleInteraction(NestItem item)
    {
        Debug.Log($"Interacting with nest item '{item.name}'");
        Inventory.Add(item);
        item.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
