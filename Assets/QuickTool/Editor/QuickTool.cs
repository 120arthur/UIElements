using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class QuickTool : EditorWindow
{
    [MenuItem("QuickTool/Open _%#T")]
    public static void ShowWindow()
    {
        var window = GetWindow<QuickTool>();
        window.titleContent = new GUIContent("QuickTool");
        window.minSize = new Vector2(250, 50);
    }

}