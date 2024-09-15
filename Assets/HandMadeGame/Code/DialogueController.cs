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
    public TMP_Text YesText;
    public TMP_Text NoText;
    private Action YesAction;
    private Action NoAction;
    private Action AdvanceAction;

    private static Action NoOp = () => { };

    private void Awake()
        => gameObject.SetActive(false);

    private void Start()
    {
        YesButton.onClick.AddListener(() => HandleAction(YesAction));
        NoButton.onClick.AddListener(() => HandleAction(NoAction));
    }

    private bool _DialogueWasJustShown = false;
    private void ShowDialogue(Sprite portrait, string message, bool showButtons, string yesText = "Yes", string noText = "No")
    {
        Portrait.sprite = portrait;
        DialogueText.text = UiController.ProcessDisplayString(message);

        if (showButtons)
        {
            YesText.text = yesText;
            NoText.text = noText;
        }

        YesButton.gameObject.SetActive(showButtons);
        NoButton.gameObject.SetActive(showButtons);

        _DialogueWasJustShown = true;

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            UiController.StartUiInteraction();
        }
    }

    public void ShowDialogue(Sprite portrait, string message, Action advanceAction)
    {
        YesAction = NoAction = null;
        AdvanceAction = advanceAction;
        ShowDialogue(portrait, message, false);
    }

    public void ShowDialogue(Sprite portrait, string message)
        => ShowDialogue(portrait, message, NoOp);

    public void ShowDialogue(Sprite portrait, string message, Action yesAction, Action noAction)
    {
        YesAction = yesAction;
        NoAction = noAction;
        AdvanceAction = null;
        ShowDialogue(portrait, message, true);
    }

    public void ShowDialogue(Sprite portrait, string message, string yesText, Action yesAction, string noText, Action noAction)
    {
        YesAction = yesAction;
        NoAction = noAction;
        AdvanceAction = null;
        ShowDialogue(portrait, message, true, yesText, noText);
    }

    private void HandleAction(Action action)
    {
        _DialogueWasJustShown = false;
        action();

        // If the action didn't continue the dialogue, the dialogue session ends.
        if (!_DialogueWasJustShown)
        {
            gameObject.SetActive(false);
            UiController.EndUiInteraction();
        }
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy && AdvanceAction != null)
        {
            if (UiController.CheckGlobalDismiss())
                HandleAction(AdvanceAction);
        }
    }
}
