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
        [SerializeField]
        public float explodePow = 7.5f;
        [SerializeField]
        public bool sizeFlag = true;
        [SerializeField]
        public bool colorFlag = true;


#if UNITY_EDITOR
        private PlayableDirector playableDirector;
#endif
        protected override void OnCreateClip(TimelineClip clip)
        {
            clip.displayName = "敵出現";
            var clipData = clip.asset as EnemySpawnClip;
            clipData.startPosition = new Vector3(0, 0, 100);
            clipData.endPosition = new Vector3(0, 0, 10);
        }

        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            EnemySpawnClip enemyClip = clip.asset as EnemySpawnClip;
            enemyClip.enemyPrefab = this.enemyPrefab;
            enemyClip.explodePow = this.explodePow;
            enemyClip.sizeFlag = this.sizeFlag;
            enemyClip.colorFlag = this.colorFlag;
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
                double timebackup = playableDirector.time;

                playableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
                EditorTimelineEvaluator.Evaluate(playableDirector);
            }
        }
#endif
    }
}