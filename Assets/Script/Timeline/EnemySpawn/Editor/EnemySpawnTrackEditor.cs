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

        private EnemyPreviewDrawer previewDrawer;
        Rect buttonRect;

        void OnEnable()
        {
            previewDrawer = new EnemyPreviewDrawer();
            EditorApplication.update += this.OnUpdate;
        }
        void OnDisable()
        {
            previewDrawer.Dispose();
            EditorApplication.update -= this.OnUpdate;
        }
        void OnUpdate()
        {
            Repaint();
        }
        public override void OnInspectorGUI()
        {
//            base.OnInspectorGUI();
            if (GUILayout.Button("敵を変更"))
            {
                var window = EditorWindow.GetWindow<PopupSelectEnemy>();
                window.SetTargetClip(this.target as EnemySpawnTrack);
                window.Show();
            }
            if (Event.current.type == EventType.Repaint)
            {
                buttonRect = GUILayoutUtility.GetLastRect();
            }
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
            previewDrawer.SetRenderSize((int)r.width, (int)r.height);
            previewDrawer.SetPrefab(enemySpawn.enemyPrefab);
            previewDrawer.Render();
            EditorGUI.DrawPreviewTexture(r, previewDrawer.renderTexture);

        }

    }
}