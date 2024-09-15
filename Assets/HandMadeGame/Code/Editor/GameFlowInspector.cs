using UnityEditor;

[CustomEditor(typeof(GameFlow))]
public sealed class GameFlowInspector : Editor
{
    private new GameFlow target => (GameFlow)base.target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField($"Inventory contains {target.Inventory.Count} items");
        foreach (NestItem item in target.Inventory)
            EditorGUILayout.LabelField($"* {item.name}");
    }
}
