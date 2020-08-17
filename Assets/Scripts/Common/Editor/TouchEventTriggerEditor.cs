using UnityEditor;
using UnityEditor.EventSystems;

[CustomEditor(typeof(TouchEventTrigger))]
public class TouchEventTriggerEditor : EventTriggerEditor {
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("_isInstance"), true);
        this.serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}