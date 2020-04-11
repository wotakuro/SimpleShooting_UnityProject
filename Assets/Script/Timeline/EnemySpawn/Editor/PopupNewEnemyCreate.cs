using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace TimelineExtention
{
    public class PopupNewEnemyCreate : EditorWindow
    {
        const string basePrefab = "Assets/Prefabs/EnemyTemplate/_EnemyBase.prefab";
        const string baseSpritePrefab = "Assets/Prefabs/EnemyTemplate/_EnemySpriteBase.prefab";
        const string generatePath = "Assets/Prefabs/Enemys/Generated/";

        struct ObjectInfo
        {
            public Object objectValue;
            public string objType;
            public string objPath;
            public ObjectInfo(Object obj, string path)
            {
                this.objectValue = obj;
                var prefab = obj as GameObject;
                if (prefab != null)
                {
                    objType = "(Prefab)";
                }
                else
                {
                    objType = "(Texture2D)";

                }
                objPath = path;
            }
        }

        private List<ObjectInfo> objectInfos;
        private Vector2 scrollPos;

        private void OnEnable()
        {

            objectInfos = new List<ObjectInfo>();
            var guids = AssetDatabase.FindAssets("t:prefab");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!CheckPath(path))
                {
                    continue;
                }
                var prefab = AssetDatabase.LoadAssetAtPath<Object>(path);
                objectInfos.Add(new ObjectInfo(prefab, path));
            }
            guids = AssetDatabase.FindAssets("t:texture");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!CheckPath(path))
                {
                    continue;
                }
                var prefab = AssetDatabase.LoadAssetAtPath<Object>(path);
                objectInfos.Add(new ObjectInfo(prefab, path));
            }
        }

        private bool CheckPath(string path)
        {
            if (!path.StartsWith("Assets/"))
            {
                return false;
            }
            if (path.StartsWith("Assets/Prefabs/Enemys/"))
            {
                return false;
            }
            if (path.StartsWith("Assets/Prefabs/EnemyTemplate/"))
            {
                return false;
            }
            if (path.StartsWith("Assets/Prefabs/SystemUse/"))
            {
                return false;
            }
            return true;
        }

        private void OnGUI()
        {
            bool closeFlag = false;
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            foreach (var objInfo in objectInfos)
            {
                var obj = objInfo.objectValue;
                var previewTex = AssetPreview.GetAssetPreview(obj);
                GUIContent content = new GUIContent(previewTex);

                EditorGUILayout.BeginVertical();
                if (GUILayout.Button(previewTex, GUILayout.Width(128), GUILayout.Height(128)))
                {
                    if (objInfo.objType == "(Prefab)")
                    {
                        CreatePrefabFrom(objInfo.objectValue, objInfo.objPath);
                    }else if(objInfo.objType == "(Texture2D)")
                    {
                        CreateFromTexture2D(objInfo.objectValue, objInfo.objPath);
                    }
                    closeFlag = true;
                }
                GUILayout.Label(obj.name);
                GUILayout.Label(objInfo.objType);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
            if(closeFlag)
            {

                EditorWindow.GetWindow<PopupSelectEnemy>().ReloadPrefabs();
                this.Close();
            }
        }

        private void CreatePrefabFrom(Object obj, string path)
        {
            var baseObject = AssetDatabase.LoadAssetAtPath<GameObject>(basePrefab);
            var contentsRoot = (GameObject)PrefabUtility.InstantiatePrefab(baseObject);

            var gameObject = (GameObject)PrefabUtility.InstantiatePrefab(obj);
            gameObject.transform.SetParent(contentsRoot.transform);
            PrefabUtility.SaveAsPrefabAsset(contentsRoot, generatePath + "FromPrefab/" + obj.name + ".prefab");
            GameObject.DestroyImmediate(contentsRoot);
        }
        private void CreateFromTexture2D(Object obj, string path)
        {
            var texture = obj as Texture2D;
            TextureImporter textureImporter = TextureImporter.GetAtPath(path) as TextureImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spritePixelsPerUnit = texture.height / 1.2f;
            textureImporter.SaveAndReimport();

            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path );

            var baseObject = AssetDatabase.LoadAssetAtPath<GameObject>(baseSpritePrefab);
            var contentsRoot = (GameObject)PrefabUtility.InstantiatePrefab(baseObject);

            contentsRoot.GetComponentInChildren<SpriteRenderer>().sprite = sprite;

            PrefabUtility.SaveAsPrefabAsset(contentsRoot, generatePath + "From2D/" + sprite.name + ".prefab");
            GameObject.DestroyImmediate(contentsRoot);
        }
    }
}