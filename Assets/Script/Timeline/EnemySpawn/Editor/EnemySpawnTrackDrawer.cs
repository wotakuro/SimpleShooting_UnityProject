using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEditor.Timeline;
using UnityEditor.Playables;
using UnityEditor;


namespace TimelineExtention
{
    [CustomTimelineEditor(typeof(EnemySpawnTrack))]
    public class EnemySpawnTrackDrawer : TrackEditor
    {
        private EnemyPreviewDrawer drawer;
        private Texture2D tex;

        private GameObject prefab;
        public override void OnCreate(TrackAsset track, TrackAsset copiedFrom)
        {
            base.OnCreate(track, copiedFrom);
            drawer = new EnemyPreviewDrawer();
        }

        public override TrackDrawOptions GetTrackOptions(TrackAsset track, Object binding)
        {
            if(drawer == null)
            {
                drawer = new EnemyPreviewDrawer();
            }
            var option = new TrackDrawOptions();
            var enemyTrack = track as EnemySpawnTrack;
            if (!drawer.renderTexture || !drawer.renderTexture.IsCreated() )
            {
                prefab = null;
            }
            if (prefab != enemyTrack.enemyPrefab && enemyTrack.enemyPrefab != null)
            {
                drawer.SetPrefab(enemyTrack.enemyPrefab);
                drawer.Render();
                tex = Convert(drawer.renderTexture);
                prefab = enemyTrack.enemyPrefab;
            }
            option.icon = tex;
            return option;
        }

        private Texture2D Convert(RenderTexture rt)
        {
            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
            var old = RenderTexture.active;
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();
            RenderTexture.active = old;
            return tex;
        }
    }

}
