using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System;
using UnityEngine;
using UnityEditor.SceneManagement;

public class AutoLayoutProgram
{
    private static double startTryTime = 0;
    private static double previewTryTime = 0;
    [MenuItem("ツール/作業用レイアウトにする")]
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

    public static void TryTimelineSelect()
    {
        double time = EditorApplication.timeSinceStartup;

        bool result = false;
        for (int i = 0; i < 10; ++i)
        {
            if (previewTryTime - startTryTime < 0.5 *i && 0.5 *i <= time - startTryTime)
            {
                if (i % 2 == 0)
                {
                    Selection.activeGameObject = null;
                }
                else
                {
                    result = SetupTimeline();
                }
            }
        }

        if ( result)
        {
            EditorApplication.update -= TryTimelineSelect;
            previewTryTime = 0;
            return;
        }
        if ( time - startTryTime > 10.0)
        {
            EditorApplication.update -= TryTimelineSelect;
            previewTryTime = 0;
            return;
        }
        previewTryTime = time;
    }


    public static void AutoLayout()
    {
        EditorUtility.LoadWindowLayout("Assets/Editor/Resources/Timeline.wlt");
        EditorSceneManager.OpenScene("Assets/Scenes/game.unity");

        startTryTime = EditorApplication.timeSinceStartup;
        EditorApplication.update += TryTimelineSelect;
    }

    public static bool SetupTimeline(){
        var stageTimelines = Resources.FindObjectsOfTypeAll<StageTimeline>();
        if( stageTimelines != null && stageTimelines.Length > 0)
        {
            SetTimelineLock(false);
            Selection.activeGameObject = stageTimelines[0].gameObject;

            if( IsTimelineWidowAvailable())
            {
                SetTimelineLock(true);
                return true;
            }
            else
            {
                SetTimelineLock(false);
                return false;
            }
        }
        return false;
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

    private static bool IsTimelineWidowAvailable()
    {
        EditorWindow timelineWindow = GetTimelineWindow();
        if(timelineWindow == null) { return false; }
        var bindFlag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        // state.editSequence.asset
        var stateProp = timelineWindow.GetType().GetProperty("state", bindFlag);
        var editSeqProp =  stateProp.PropertyType.GetProperty("editSequence" ,bindFlag);
        var assetProp = editSeqProp.PropertyType.GetProperty("asset", bindFlag);

        var stateValue = stateProp.GetValue(timelineWindow);
        if( stateValue == null) { return false; }
        var editValue = editSeqProp.GetValue(stateValue);
        if( editValue == null) { return false; }

        return (assetProp.GetValue(editValue) != null);
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
