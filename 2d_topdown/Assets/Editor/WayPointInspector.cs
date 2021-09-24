using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WayPoint))]
public class WayPointInspector : Editor
{
    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorList.Show(serializedObject.FindProperty("points"), EditorListOption.Buttons);

        serializedObject.ApplyModifiedProperties();
    }

    
}

