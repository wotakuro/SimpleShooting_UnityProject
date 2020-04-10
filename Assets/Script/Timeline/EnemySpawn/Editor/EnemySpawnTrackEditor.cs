using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEditor.Timeline;
using UnityEditor.Playables;
using UnityEditor;


namespace TimelineExtention
{

    [CustomEditor(typeof(EnemySpawnTrack))]
    public class EnemySpawnTrackEditor : Editor
    {

        Rect buttonRect;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("敵を変更"))
            {
                var window = EditorWindow.CreateInstance<PopupSelectEnemy>();
                window.SetTargetClip(this.target as EnemySpawnTrack);
                window.ShowAuxWindow();
            }
            if (Event.current.type == EventType.Repaint)
            {
                buttonRect = GUILayoutUtility.GetLastRect();
            }
            //            enemySpawn.enemyPrefab;
        }
        public override bool HasPreviewGUI()
        {
            //プレビュー表示できるものがあれば true を返す
            return true;
        }
        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            base.OnPreviewGUI(r, background);
            var enemySpawn = target as EnemySpawnTrack;
            var preview = AssetPreview.GetAssetPreview(enemySpawn.enemyPrefab);
            if (preview != null)
            {
                EditorGUI.DrawPreviewTexture(r, preview);
            }
        }
    }
}