using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownCamera))]
public class TopDownCamera_SceneEditor : Editor
{
    #region Variables
    TopDownCamera targetCamera;
    #endregion Variables

    public override void OnInspectorGUI()
    {
        targetCamera = (TopDownCamera)target;
        base.OnInspectorGUI();
    }

    void OnSceneGUI() {
        if (!targetCamera || !targetCamera.target)  return;

        Transform cameraTarget = targetCamera.target;
        Vector3 targetPosition = cameraTarget.position;
        targetPosition.y += targetCamera.lookAtHeight;

        Handles.color = new Color(1f, 0f, 0f, 0.15f);
        Handles.DrawSolidDisc(targetPosition, Vector3.up, targetCamera.distance);

        Handles.color = new Color(0f, 1f, 0f, 0.75f);
        Handles.DrawWireDisc(targetPosition, Vector3.up, targetCamera.distance);

        // ** slider handles **
        Handles.color = new Color(1f, 0f, 0f, 0.5f);
        targetCamera.distance = Handles.ScaleSlider(targetCamera.distance, targetPosition, cameraTarget.forward * (-1), Quaternion.identity, targetCamera.distance, 0.1f);
        targetCamera.distance = Mathf.Clamp(targetCamera.distance, 2f, float.MaxValue);

        Handles.color = new Color(0f, 0f, 1f, 0.5f);
        targetCamera.height = Handles.ScaleSlider(targetCamera.height, targetPosition, Vector3.up, Quaternion.identity, targetCamera.height, 0.1f);
        targetCamera.height = Mathf.Clamp(targetCamera.height, 2f, float.MaxValue);

        // ** Labels **
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 15;
        labelStyle.normal.textColor = Color.white;

        labelStyle.alignment = TextAnchor.UpperCenter;
        Handles.Label(targetPosition + (cameraTarget.forward * (-1) * targetCamera.distance), "Distance", labelStyle);

        labelStyle.alignment = TextAnchor.MiddleRight;
        Handles.Label(targetPosition + (Vector3.up * targetCamera.height), "Height", labelStyle);

        targetCamera.HandleCamera();
    }
}
