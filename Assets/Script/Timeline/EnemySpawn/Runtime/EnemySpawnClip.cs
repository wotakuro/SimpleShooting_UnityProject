using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TimelineExtention
{
	public class EnemySpawnClip : PlayableAsset, ITimelineClipAsset
	{
        
        [SerializeField]
        public Vector3 startPosition;
        [SerializeField]
        public Vector3 endPosition;


        public ClipCaps clipCaps
		{
			get { return ClipCaps.None; }
		}

        public GameObject enemyPrefab
        {
            get;set;
        }
        public bool explodeFlag { get; set; }
        public bool sizeFlag { get; set; }
        public bool colorFlag { get; set; }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<EnemySpawnPlayable>.Create(graph);

            var behaviour = playable.GetBehaviour();
            behaviour.InitEnemyFromPrefab( enemyPrefab , startPosition,endPosition);
            behaviour.SetEnemyFlags(explodeFlag, sizeFlag, colorFlag);
            return playable;   
		}
	}
}