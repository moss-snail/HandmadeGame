using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class DialogueController : MonoBehaviour
{
    public Image Portrait;
    public TMP_Text DialogueText;
    public Button YesButton;
    public Button NoButton;
    private Action YesAction;
    private Action NoAction;
    private Action AdvanceAction;

    private static Action NoOp = () => { };

    private void Awake()
        => EndDialogue();

    private void Start()
    {
        YesButton.onClick.AddListener(() => HandleAction(YesAction));
        NoButton.onClick.AddListener(() => HandleAction(NoAction));
    }

    private void StartDialogue(Sprite portrait, string message, bool showButtons)
    {
        Portrait.sprite = portrait;
        DialogueText.text = message;

        YesButton.gameObject.SetActive(showButtons);
        NoButton.gameObject.SetActive(showButtons);
        gameObject.SetActive(true);
    }

    public void StartDialogue(Sprite portrait, string message, Action advanceAction)
    {
        YesAction = NoAction = null;
        AdvanceAction = advanceAction;
        StartDialogue(portrait, message, false);
    }

    public void StartDialogue(Sprite portrait, string message)
        => StartDialogue(portrait, message, NoOp);

    public void StartDialogue(Sprite portrait, string message, Action yesAction, Action noAction)
    {
        YesAction = yesAction;
        NoAction = noAction;
        AdvanceAction = null;
        StartDialogue(portrait, message, true);
    }

    private void HandleAction(Action action)
    {
        EndDialogue();
        action();
    }

    private void EndDialogue()
        => gameObject.SetActive(false);

    private void Update()
    {
        if (gameObject.activeInHierarchy && AdvanceAction != null)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(PlayerInteraction.InteractionKey))
                HandleAction(AdvanceAction);
        }
    }
}
