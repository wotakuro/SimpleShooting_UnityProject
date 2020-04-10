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

        public override TrackDrawOptions GetTrackOptions(TrackAsset track, Object binding)
        {
            var option = new TrackDrawOptions();
            var enemyTrack = track as EnemySpawnTrack;
            option.icon = AssetPreview.GetAssetPreview(enemyTrack.enemyPrefab);

            return option;
        }
    }

}
