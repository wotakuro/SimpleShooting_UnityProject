using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TimelineExtention
{
    [TrackBindingType(typeof(BulletManager))]
    [TrackClipType(typeof(BulletClip))]
    public class BulletTrack : TrackAsset
    {
#if UNITY_EDITOR
        private PlayableDirector playableDirector;
        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            foreach (var c in GetClips())
            {
                var clipValue = (BulletClip)c.asset;
                c.displayName = BulletManager.PlayModeText[(int) clipValue.playMode];
            }
            this.playableDirector = gameObject.GetComponent<PlayableDirector>();

            return base.CreatePlayable(graph, gameObject, clip);
        }
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
