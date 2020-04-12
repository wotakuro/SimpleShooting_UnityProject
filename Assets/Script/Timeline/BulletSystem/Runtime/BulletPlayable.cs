using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TimelineExtention
{
    public class BulletPlayable : PlayableBehaviour
    {

        public BulletManager.PlayerMode playMode;
        private BulletManager bulletManager;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            if (bulletManager == null)
            {
                bulletManager = playerData as BulletManager;
            }
            if( bulletManager != null)
            {
                bulletManager.SetPlayMode(this.playMode);
            }
        }
    }
}