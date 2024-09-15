using TMPro;
using UnityEngine;

public sealed class ControllerPrompt : MonoBehaviour
{
    public TMP_Text Text;

    private void Awake()
        => Hide();

    public void Show(KeyCode keyCode, string action)
    {
        string keySprite = keyCode switch
        {
            KeyCode.E => @"<sprite=""keyboard_e"" index=0>",
            KeyCode.F => @"<sprite=""keyboard_f"" index=0>",
            _ => $"[{keyCode}]"
        };

        Text.text = $"{keySprite} {action}";
        gameObject.SetActive(true);
    }

    public void Hide()
        => gameObject.SetActive(false);
}
