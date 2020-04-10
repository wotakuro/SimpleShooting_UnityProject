using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TimelineExtention
{
	[TrackClipType(typeof(EnemySpawnClip))]
	public class EnemySpawnTrack : TrackAsset {

        [SerializeField]
        public GameObject enemyPrefab;
#if UNITY_EDITOR
        private PlayableDirector playableDirector;
#endif
        protected override void OnCreateClip(TimelineClip clip)
        {
            clip.displayName = "敵出現";
        }

        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            EnemySpawnClip enemyClip = clip.asset as EnemySpawnClip;
            enemyClip.prefab = this.enemyPrefab;
#if UNITY_EDITOR
            this.playableDirector = gameObject.GetComponent<PlayableDirector>();
#endif            
            return base.CreatePlayable(graph, gameObject, clip);
        }
#if UNITY_EDITOR
        public void RebuildGraph()
        {
            if (playableDirector != null)
            {
                playableDirector.RebuildGraph();
                playableDirector.Evaluate();
            }
        }
#endif
    }
}