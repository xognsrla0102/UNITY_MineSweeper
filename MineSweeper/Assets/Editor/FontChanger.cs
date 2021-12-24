using UnityEngine;
using UnityEditor;
using System.Reflection;

public class FontChanger : EditorWindow
{
    private static FontChanger window;

    private Font font;
    private int fontSize;

    [MenuItem("Tools/Font Changer")]
    private static void Init()
    {
        window = GetWindow<FontChanger>();
    }

    private void OnGUI()
    {
        font = EditorGUILayout.ObjectField("Font", font, typeof(Font), false) as Font;
        fontSize = EditorGUILayout.IntField("Font Size", fontSize);

        GUILayout.Space(10f);
        if (GUILayout.Button("Apply")) ChangeFont(font);
        else if (GUILayout.Button("Reset")) ChangeFont(null); 
    }

    private void ChangeFont(Font font)
    {
        BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty;
        PropertyInfo[] editorStyleInfos = typeof(EditorStyles).GetProperties(flags);
        PropertyInfo[] guiStyleInfos = GUI.skin.GetType().GetProperties();

        foreach (var styleInfo in editorStyleInfos)
        {
            if (!PropertyInfoExists(styleInfo)) continue;
            GUIStyle style = styleInfo.GetValue(null, null) as GUIStyle;
            style.font = font;
            style.fontSize = fontSize;
        }

        //foreach (var styleInfo in guiStyleInfos)
        //{
        //    if (!PropertyInfoExists(styleInfo)) continue;
        //    GUIStyle style = styleInfo.GetValue(null, null) as GUIStyle;
        //    style.font = font;
        //    style.fontSize = fontSize;
        //}

        foreach (var styleInfo in GUI.skin.customStyles)
        {
            styleInfo.font = font;
            styleInfo.fontSize = fontSize;
        }

        EditorWindow[] windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (var window in windows)
        {
            window.Repaint();
        }

        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    }

    private bool PropertyInfoExists(PropertyInfo info)
    {
        if (string.IsNullOrEmpty(info.Name)) return false;
        else if (info.PropertyType != typeof(GUIStyle)) return false;
        return true;
    }
}
