using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TimelineExtention
{

    [CustomEditor(typeof(EnemySpawnClip))]
    public class EnemyClipEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}