using UnityEngine;

public static class UiController
{
    public const KeyCode InteractionKey = KeyCode.E;

    public static bool CheckGlobalDismiss()
        => Input.GetKeyDown(KeyCode.Space)
        || Input.GetKeyDown(KeyCode.Return)
        || Input.GetKeyDown(KeyCode.Escape)
        || Input.GetMouseButtonDown(0)
        || Input.GetKeyDown(InteractionKey);

    public static string ProcessDisplayString(string text)
        => text
        .Replace("<em>", "<color=#fee761>")
        .Replace("</em>", "</color>")
        .Replace('’', '\'')
        .Replace("…", "...")
        ;
}
