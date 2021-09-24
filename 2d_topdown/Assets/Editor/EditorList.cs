using UnityEngine;
using UnityEditor;
using System;

[Flags]
public enum EditorListOption
{
    Buttons = 8
}

public static class EditorList
{
    public static void Show (SerializedProperty list, EditorListOption options) {
        //EditorGUILayout.PropertyField(list);
        
        EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
        ShowElements(list, options);
    }

    private static void ShowElements (SerializedProperty list, EditorListOption options)
    {
        bool showBtns = (options & EditorListOption.Buttons) != 0;

        for (int i = 0; i < list.arraySize; i++) {
            EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            //EditorGUILayout.Space();
            if (showBtns)   
                ShowBtns(list, i);
            EditorGUILayout.EndHorizontal();
        }

        if (list.arraySize == 0 && GUILayout.Button(addBtnContent, EditorStyles.miniButton)) {
            list.arraySize += 1;
        }
    }

    static GUIContent
        moveBtnContent = new GUIContent("\u21b4", "move down"),
        duplicateBtnContent = new GUIContent("+", "dulicate"),
        deleteBtnContent = new GUIContent("-", "delete"),
        addBtnContent = new GUIContent("+", "add element");

    static GUILayoutOption miniBtnWidth = GUILayout.Width(20f);
    static void ShowBtns(SerializedProperty list, int index)
    {
        if (GUILayout.Button(moveBtnContent, EditorStyles.miniButtonLeft, miniBtnWidth)) {
            list.MoveArrayElement(index, index + 1);
        }
        if (GUILayout.Button(duplicateBtnContent, EditorStyles.miniButtonMid, miniBtnWidth)) {
            list.InsertArrayElementAtIndex(index);
        }
        if (GUILayout.Button(deleteBtnContent, EditorStyles.miniButtonRight, miniBtnWidth)) {
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (list.arraySize == oldSize)
                list.DeleteArrayElementAtIndex(index);
        }
    }
}
