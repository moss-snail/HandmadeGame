using UnityEngine;

public class HudTests : MonoBehaviour
{
    public DialogueController DialogueController;
    public ControllerPrompt ControllerPrompt;

    public Sprite Snel;
    public Sprite Snel2;

    private void OnGUI()
    {
        if (GUILayout.Button("Basic Test"))
        {
            DialogueController.StartDialogue(Snel, "Test");
        }

        if (GUILayout.Button("Conversation Test"))
        {
            DialogueController.StartDialogue(Snel, "Test?", () => DialogueController.StartDialogue(Snel2, "Test!"));
        }

        if (GUILayout.Button("Choice"))
        {
            DialogueController.StartDialogue(Snel, "Test?",
                () => DialogueController.StartDialogue(Snel2, "Yes"),
                () => DialogueController.StartDialogue(Snel2, "No")
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
    }
}
