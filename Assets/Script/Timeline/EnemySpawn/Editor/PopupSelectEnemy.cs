
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
        private List<EnemyPreviewDrawer> previewDrawers = new List<EnemyPreviewDrawer>();


        public void OnEnable()
        {
            this.ReloadPrefabs();
            this.minSize = new Vector2(128 * 6, 128 * 5);
            this.position = new Rect(this.position.x, this.position.y, minSize.x, minSize.y);
        }

        public void ReloadPrefabs()
        {
            ClearData();
            var guids = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Prefabs/Enemys" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab.GetComponent<Enemy>() == null)
                {
                    continue;
                }
                prefabs.Add(prefab);
                // setup preview
                var previewObj = new EnemyPreviewDrawer();
                previewObj.SetPrefab(prefab);
                previewObj.SetRenderSize(128, 128);
                previewDrawers.Add(previewObj);
            }
        }

        private void OnDisable()
        {
            ClearData();
        }

        private void ClearData()
        {
            if (previewDrawers != null)
            {
                foreach (var preview in previewDrawers)
                {
                    preview.Dispose();
                }
                previewDrawers.Clear();
            }
            if (prefabs != null)
            {
                prefabs.Clear();
            }
        }

        private void Update()
        {
            this.Repaint();
        }

        public void OnGUI()
        {
            GUILayout.Label("敵選択", EditorStyles.boldLabel);
            if(GUILayout.Button("新しく敵の種類を追加する"))
            {
                var window = EditorWindow.CreateInstance<PopupNewEnemyCreate>();
                window.ShowAuxWindow();
            }
            EditorGUILayout.LabelField("");

            GUILayout.Label("敵を選択してください", EditorStyles.boldLabel);
            int cnt = 0;
            bool isClose = false;
            scroll = EditorGUILayout.BeginScrollView(scroll);
            for (int i = 0;i<prefabs.Count;++i)
            {
                var prefab = prefabs[i];
                if (cnt == 0)
                {
                    EditorGUILayout.BeginHorizontal();
                }

                previewDrawers[i].Render();
                var texture = previewDrawers[i].renderTexture;
                GUIContent content = new GUIContent(texture);

                EditorGUILayout.BeginVertical(GUILayout.MaxWidth(128));
                if (GUILayout.Button(content, GUILayout.MaxWidth(128), GUILayout.MaxHeight(128)))
                {
                    if (targetTrack != null)
                    {
                        targetTrack.enemyPrefab = prefab;
                        targetTrack.RebuildGraph();
                        EditorUtility.SetDirty(targetTrack);
                        isClose = true;
                        break;
                    }
                }
                EditorGUILayout.LabelField(prefab.name,GUILayout.MaxWidth(120));
                EditorGUILayout.EndHorizontal();
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