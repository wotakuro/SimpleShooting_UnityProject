
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEditor.Timeline;
using UnityEditor.Playables;
using UnityEditor;


namespace TimelineExtention
{

    public class PopupSelectEnemy : EditorWindow
    {
        private EnemySpawnTrack targetTrack;
        private Vector2 scroll;

        public void SetTargetClip(EnemySpawnTrack target)
        {
            this.targetTrack = target;
        }
        private List<GameObject> prefabs = new List<GameObject>();
        public void OnEnable()
        {
            var guids = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Prefabs/Enemys" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                prefabs.Add(prefab);
            }
            this.minSize = new Vector2(128 * 6, 128 * 5);
            this.position = new Rect(this.position.x, this.position.y, minSize.x, minSize.y);
        }

        public void OnGUI()
        {
            GUILayout.Label("敵を選択してください", EditorStyles.boldLabel);
            int cnt = 0;
            bool isClose = false;
            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (var prefab in prefabs)
            {
                if (cnt == 0)
                {
                    EditorGUILayout.BeginHorizontal();
                }

                var texture = AssetPreview.GetAssetPreview(prefab);
                GUIContent content = new GUIContent(texture);
                if (GUILayout.Button(content, GUILayout.MaxWidth(128), GUILayout.MaxHeight(128)))
                {
                    if (targetTrack != null)
                    {
                        targetTrack.enemyPrefab = prefab;
                        targetTrack.RebuildGraph();
                        EditorUtility.SetDirty(targetTrack);
                        isClose = true;
                    }
                }
                ++cnt;
                if (cnt >= 5)
                {
                    EditorGUILayout.EndHorizontal();
                    cnt = 0;
                }
            }
            if (cnt != 0)
            {
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            if (isClose)
            {
                this.Close();
            }
        }
    }
}