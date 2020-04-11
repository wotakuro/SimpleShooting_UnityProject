using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System;
using UnityEngine;
using UnityEditor.SceneManagement;

public class AutoLayoutProgram
{
    [MenuItem("ツール/作業用レイアウトにする")]
    public static void FromMenu()
    {
        AutoLayout();
        EditorApplication.delayCall += SetupTimeline;
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
    }

    public static void SetupTimeline(){
        var stageTimelines = Resources.FindObjectsOfTypeAll<StageTimeline>();
        if( stageTimelines != null && stageTimelines.Length > 0)
        {
            SetTimelineLock(false);
            Selection.activeGameObject = stageTimelines[0].gameObject;
            SetTimelineLock(true);
        }
    }
    private static void SetTimelineLock(bool flag)
    {
        // state.editSequence.asset
        EditorWindow timelineWindow = GetTimelineWindow();
        if (timelineWindow != null)
        {
            Type timelineType = timelineWindow.GetType();
            timelineWindow = EditorWindow.GetWindow(timelineType, false);

            var setLockedMethod = timelineType.GetMethod("set_locked");
            setLockedMethod.Invoke(timelineWindow, new object[] { flag });
        }
    }
    private static EditorWindow GetTimelineWindow()
    {
        EditorWindow timelineWindow = null;

        EditorWindow[] windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        if (windows != null && windows.Length > 0)
        {
            foreach (var window in windows)
                if (window.GetType().Name.Contains("Timeline"))
                    timelineWindow = window;
        }
        return timelineWindow;

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
