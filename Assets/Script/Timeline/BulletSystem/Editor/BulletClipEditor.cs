using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TimelineExtention
{
    [CustomEditor(typeof(BulletClip))]
    public class BulletClipEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            BulletClip clip = target as BulletClip;
            int originIdx = (int)clip.playMode;
            int idx = EditorGUILayout.Popup( originIdx , BulletManager.PlayModeText);
            if( idx != originIdx)
            {
                clip.playMode = (BulletManager.PlayerMode)idx;
                EditorUtility.SetDirty(target);
            }
        }
    }
}