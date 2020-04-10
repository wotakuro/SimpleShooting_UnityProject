using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System;
using UnityEngine;
using UnityEditor.SceneManagement;

public class AutoLayoutProgram 
{
    [MenuItem("Tools/AutoLayout")]
    public static void FromMenu()
    {
        AutoLayout();
    }

//    [UnityEditor.InitializeOnLoadMethod]
    public static void Init()
    {
        string path = "Temp/UnityEditor1stCheck.txt";
        if (!System.IO.File.Exists(path))
        {
            System.IO.File.WriteAllText(path, "debug");
            EditorApplication.delayCall += AutoLayout;
        }
    }


    public static void AutoLayout()
    {
        EditorUtility.LoadWindowLayout("Assets/Editor/Resources/Timeline.wlt");
        EditorSceneManager.OpenScene("Assets/Scenes/game.unity");
        var stageTimelines = Resources.FindObjectsOfTypeAll<StageTimeline>();
        if( stageTimelines != null && stageTimelines.Length > 0)
        {
            Selection.activeGameObject = stageTimelines[0].gameObject;
            SetTimelineLock();

        }
    }
    private static void SetTimelineLock()
    {
        EditorWindow timelineWindow = null;

        EditorWindow[] windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        if (windows != null && windows.Length > 0)
        {
            foreach (var window in windows)
                if (window.GetType().Name.Contains("Timeline"))
                    timelineWindow = window;
        }

        if (timelineWindow != null)
        {
            Type timelineType = timelineWindow.GetType();
            timelineWindow = EditorWindow.GetWindow(timelineType, false);

            var setLockedMethod = timelineType.GetMethod("set_locked");
            setLockedMethod.Invoke(timelineWindow, new object[] { true });
        }
    }

    private static Type GetType(string fullname)
    {
        Type type = null;
        var domain = System.AppDomain.CurrentDomain;
        var asms = domain.GetAssemblies();

        foreach( var asm in asms)
        {
            type = asm.GetType(fullname);
            if( type != null)
            {
                return type;
            }
        }

        return type;
    }

}
