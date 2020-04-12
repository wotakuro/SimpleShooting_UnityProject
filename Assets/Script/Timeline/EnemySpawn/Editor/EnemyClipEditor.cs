using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using System.Reflection;

namespace TimelineExtention
{

    [CustomEditor(typeof(EnemySpawnClip))]
    public class EnemyClipEditor : Editor
    {
        private Vector4 screenStartPoint;
        private Vector4 screenEndPoint;

        private bool startFlag;
        private bool endFlag;

        private Camera startCamera;
        private Camera endCamera;

        private void OnEnable()
        {
            var clip = target as EnemySpawnClip;
            this.screenStartPoint = new Vector4(0, 0, clip.startPosition.z, 0);
            this.screenEndPoint = new Vector4(0, 0, clip.endPosition.z, 0);
            startFlag = true;
            endFlag = true;
            this.InitCamera();
            if (this.startCamera != null)
            {
                var point = this.startCamera.WorldToViewportPoint(clip.startPosition);
                this.screenStartPoint = new Vector4(point.x,point.y,point.z,0.0f);
            }
            if (this.endCamera != null)
            {
                var point = this.endCamera.WorldToViewportPoint(clip.endPosition);
                this.screenEndPoint = new Vector4(point.x, point.y, point.z, 0.0f);
            }
        }

        private void InitCamera()
        {
            // GetEditorClip.
            GameObject timelineGmo = GameObject.Find("Timeline");
            PlayableDirector timeline = null;
            if (timelineGmo != null)
            {
                timeline = timelineGmo.GetComponent<PlayableDirector>();
            }
            double timelineStart, timelineEnd;

            GetTimelineTrackPostion(out timelineStart, out timelineEnd);

            // origin camera
            var cameraGmo = GameObject.Find("MainCamera");
            var originCamera = cameraGmo.GetComponent<Camera>();


            //end
            if( timelineEnd >= 0.0)
            {
                timeline.time = timelineEnd;
                timeline.RebuildGraph();
                timeline.Evaluate();
            }
            var endCameraGmo = new GameObject();
            this.endCamera = endCameraGmo.AddComponent<Camera>();
            SetupCamera(this.endCamera, originCamera);


            // start
            if (timelineStart >= 0.0)
            {
                timeline.time = timelineStart;
                timeline.RebuildGraph();
                timeline.Evaluate();
            }
            var startCameraGmo = new GameObject();
            this.startCamera = startCameraGmo.AddComponent<Camera>();
            SetupCamera(this.startCamera, originCamera);
        }

        private void GetTimelineTrackPostion(out double start , out double end)
        {
            start = -1.0;
            end = -1.0;

            var obj = UnityEditor.Selection.activeObject;
            if(obj == null) { 
                return;
            }

            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var clipField = obj.GetType().GetField("m_Clip", flags);
            if (clipField == null)
            {
                return;
            }
            var clipValue = clipField.GetValue(obj);
            if( clipValue == null) {
                return;
            }

            FieldInfo startField = clipField.FieldType.GetField("m_Start", flags); ;
            FieldInfo durationField = clipField.FieldType.GetField("m_Duration", flags); ;

            if( startField == null) { 
                return; 
            }

            start = (double)startField.GetValue(clipValue);
            if (durationField != null)
            {
                end = (double)durationField.GetValue(clipValue) + start;
            }

        }

        private void SetupCamera(Camera dest,Camera src)
        {
            dest.gameObject.hideFlags = HideFlags.HideAndDontSave;
            dest.cullingMask = 0;
            dest.clearFlags = CameraClearFlags.Nothing;
            dest.orthographic = src.orthographic;
            if (dest.orthographic)
            {
                dest.orthographicSize = src.orthographicSize;
            }
            else
            {
                dest.fieldOfView = src.fieldOfView;
            }
            dest.nearClipPlane = src.nearClipPlane;
            dest.farClipPlane = src.farClipPlane;
            dest.enabled = false;
            dest.transform.position = src.transform.position;
            dest.transform.rotation = src.transform.rotation;
        }

        private void OnDisable()
        {
            if (startCamera)
            {
                GameObject.DestroyImmediate(startCamera.gameObject);
            }
            if (endCamera)
            {
                GameObject.DestroyImmediate(endCamera.gameObject);
            }
        }

        public override void OnInspectorGUI()
        {
//            base.OnInspectorGUI();

            bool requireRepaint = false;
            var mousePos = Event.current.mousePosition;
            Vector3 mouseInfo = new Vector3( mousePos.x,mousePos.y,
                (Event.current.isMouse)?1.0f:0.0f);

            var clip = target as EnemySpawnClip;

            bool isApply = false;

            startFlag = EditorGUILayout.Foldout(startFlag,"開始位置");
            if (startFlag)
            {
                isApply |= OnGUIPositionInfo(mouseInfo, this.startCamera, ref clip.startPosition,
                    ref screenStartPoint, ref requireRepaint);
            }

            endFlag = EditorGUILayout.Foldout(endFlag, "終了位置");
            if (endFlag)
            {
                isApply |= OnGUIPositionInfo(mouseInfo, this.endCamera, ref clip.endPosition,
                    ref screenEndPoint, ref requireRepaint);
            }


            if(isApply)
            {
                EditorUtility.SetDirty(clip);
            }

            if (requireRepaint)
            {
                this.Repaint();
            }

        }

        private bool OnGUIPositionInfo(Vector3 mouseInfo,Camera camera ,ref Vector3 originPos,
            ref Vector4 screenNewPoint ,ref bool requireRepaint)
        {
            var screenOriginPoint = Vector3.zero;
            if (camera != null)
            {
                screenOriginPoint = camera.WorldToViewportPoint(originPos);
            }
            EditorGUILayout.BeginVertical();
            var newPos = EditorGUILayout.Vector3Field("座標", originPos);
            var rect = EditorGUILayout.GetControlRect(GUILayout.Height(150), GUILayout.Width(250));
            {
                var startRect = new Rect(rect.x + 25, rect.y + 10, 240, 135);
                DrawRect(startRect, mouseInfo, screenOriginPoint, ref screenNewPoint, ref requireRepaint);
            }
            EditorGUILayout.LabelField("奥行き");
            screenNewPoint.z = EditorGUILayout.Slider(screenNewPoint.z, 3.0f, 150.0f, GUILayout.Width(250));
            if (GUILayout.Button("ココにする", GUILayout.Width(100)) && camera != null)
            {
                newPos = camera.ViewportToWorldPoint(screenNewPoint);
            }
            EditorGUILayout.EndVertical();

            bool flag= (newPos != originPos);
            originPos = newPos;
            return flag;
        }

        private void DrawRect( Rect r, Vector3 mouseInfo,
            Vector3 originScreenPoint,
            ref Vector4 newScreenPoint,ref bool requireRepaint)
        {

            EditorGUI.DrawRect(new Rect(r.x-5,r.y-5,r.width+10,r.height+10), new Color(0.8f,0.8f,0.8f));
            EditorGUI.DrawRect(r, Color.white);

            Vector2 menuOriginPoint = new Vector2( r.x + r.width * originScreenPoint.x,
                r.y + r.height * (1.0f - originScreenPoint.y));

            menuOriginPoint.x = Mathf.Clamp(menuOriginPoint.x, r.x, r.x + r.width);
            menuOriginPoint.y = Mathf.Clamp(menuOriginPoint.y, r.y, r.y + r.height);


            if( mouseInfo.z >= 0.99f)
            {
                if(r.x <= mouseInfo.x && mouseInfo.x <= r.x + r.width)
                {
                    if (r.y <= mouseInfo.y && mouseInfo.y <= r.y + r.height)
                    {
                        newScreenPoint.x = (mouseInfo.x - r.x) / r.width;
                        newScreenPoint.y = 1.0f - ((mouseInfo.y - r.y) / r.height);
                        newScreenPoint.w = 1.0f;
                        requireRepaint = true;
                    }
                }
            }
            // blue
            if( newScreenPoint.w >= 0.99f)
            {
                Vector2 menuNewPoint = new Vector2(r.x + r.width * newScreenPoint.x,
                    r.y + r.height * (1.0f - newScreenPoint.y));
                EditorGUI.DrawRect(new Rect(menuNewPoint.x - 5, menuNewPoint.y - 1, 10, 2), Color.blue);
                EditorGUI.DrawRect(new Rect(menuNewPoint.x - 1, menuNewPoint.y - 5, 2, 10), Color.blue);
            }
            // origin
            EditorGUI.DrawRect(new Rect(menuOriginPoint.x - 5, menuOriginPoint.y - 1, 10, 2), Color.red);
            EditorGUI.DrawRect(new Rect(menuOriginPoint.x - 1, menuOriginPoint.y - 5, 2, 10), Color.red);
        }
    }
}