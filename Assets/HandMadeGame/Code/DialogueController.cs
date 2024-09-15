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

    /// <summary>Fires when a new dialogue session starts</summary>
    public static event Action DialogueStart;
    /// <summary>Fires when a dialogue session ends</summary>
    public static event Action DialogueEnd;

    private void Awake()
        => gameObject.SetActive(false);

    private void Start()
    {
        YesButton.onClick.AddListener(() => HandleAction(YesAction));
        NoButton.onClick.AddListener(() => HandleAction(NoAction));
    }

    private bool _DialogueWasJustShown = false;
    private void ShowDialogue(Sprite portrait, string message, bool showButtons)
    {
        Portrait.sprite = portrait;
        DialogueText.text = message;

        YesButton.gameObject.SetActive(showButtons);
        NoButton.gameObject.SetActive(showButtons);

        _DialogueWasJustShown = true;

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            DialogueStart?.Invoke();
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

    private void HandleAction(Action action)
    {
        _DialogueWasJustShown = false;
        action();

        // If the action didn't continue the dialogue, the dialogue session ends.
        if (!_DialogueWasJustShown)
        {
            gameObject.SetActive(false);
            DialogueEnd?.Invoke();
        }
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy && AdvanceAction != null)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(PlayerInteraction.InteractionKey))
                HandleAction(AdvanceAction);
        }
    }
}
