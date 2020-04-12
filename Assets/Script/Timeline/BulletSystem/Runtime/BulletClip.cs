using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TimelineExtention
{
    public class BulletClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        public BulletManager.PlayerMode playMode;

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<BulletPlayable>.Create(graph);
            var behaviour = playable.GetBehaviour();
            behaviour.playMode = playMode;
            return playable;
        }
    }
}