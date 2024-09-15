using UnityEngine;

public class HudTests : MonoBehaviour
{
    public DialogueController DialogueController;
    public ControllerPrompt ControllerPrompt;
    public InfoPopupController InfoPopupController;

    public Sprite Snel;
    public Sprite Snel2;

    private bool DialogueSessionActive;

    private void Awake()
    {
        DialogueController.DialogueStart += () => DialogueSessionActive = true;
        DialogueController.DialogueEnd += () => DialogueSessionActive = false;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10f, 0f, 120f, 1000f));
        GUILayout.Label($"Dialogue session {(DialogueSessionActive ? "active" : "inactive")}");

        if (GUILayout.Button("Basic Test"))
        {
            DialogueController.ShowDialogue(Snel, "Test");
        }

        if (GUILayout.Button("Conversation Test"))
        {
            DialogueController.ShowDialogue(Snel, "Test?", () => DialogueController.ShowDialogue(Snel2, "Test!"));
        }

        if (GUILayout.Button("Choice"))
        {
            DialogueController.ShowDialogue(Snel, "Test?",
                () => DialogueController.ShowDialogue(Snel2, "Yes"),
                () => DialogueController.ShowDialogue(Snel2, "No")
            );
        }

        if (GUILayout.Button("Custom choice"))
        {
            DialogueController.ShowDialogue(Snel, "Test?",
                "Yes, I'm ready!", () => { },
                "Not quite...", () => { }
            );
        }

        if (GUILayout.Button("Show prompt E"))
            ControllerPrompt.Show(KeyCode.E, "Do the thing");
        if (GUILayout.Button("Show prompt F"))
            ControllerPrompt.Show(KeyCode.F, "Do the other thing");
        if (GUILayout.Button("Show prompt G"))
            ControllerPrompt.Show(KeyCode.G, "Do the other thing");
        if (GUILayout.Button("Hide prompt"))
            ControllerPrompt.Hide();

        if (GUILayout.Button("Info Popup"))
            InfoPopupController.ShowPopup("This is a test of the emergency birdcast system, don't let it get your feathers ruffled!", () => Debug.Log("Test popup dismissed"));

        GUILayout.EndArea();
    }
}
