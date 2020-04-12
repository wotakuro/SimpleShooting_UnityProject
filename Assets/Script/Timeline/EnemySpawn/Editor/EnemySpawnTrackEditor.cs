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
            EnemySpawnTrack enemySpawnTrack = this.target as EnemySpawnTrack;


            bool newExplode = EditorGUILayout.Toggle("爆発した時、巻き込むか", enemySpawnTrack.explodeFlag);
            bool newColor   = EditorGUILayout.Toggle("時間による色変更(2Dのみ)", enemySpawnTrack.colorFlag);
            bool newSize    = EditorGUILayout.Toggle("時間による大きさ変更", enemySpawnTrack.sizeFlag);
            //            base.OnInspectorGUI();
            if (GUILayout.Button("敵の見た目を変更"))
            {
                var window = EditorWindow.GetWindow<PopupSelectEnemy>();
                window.SetTargetTrack(enemySpawnTrack);
                window.Show();
            }

            if(newExplode != enemySpawnTrack.explodeFlag || 
                newColor != enemySpawnTrack.colorFlag ||
                newSize != enemySpawnTrack.sizeFlag )
            {
                enemySpawnTrack.explodeFlag = newExplode;
                enemySpawnTrack.colorFlag = newColor;
                enemySpawnTrack.sizeFlag = newSize;
                EditorUtility.SetDirty(this);
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