using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TimelineExtention
{
	public class EnemySpawnPlayable : PlayableBehaviour
	{

        private GameObject instanceGmo;
        private Enemy enemy;

        public void InitEnemyFromPrefab(GameObject enemyPrefab, Vector3 startPosition, Vector3 endPosition)
        {
            if (enemyPrefab != null)
            {
                instanceGmo = GameObject.Instantiate(enemyPrefab);
                instanceGmo.name += "[Timelne]";
#if UNITY_EDITOR
                if (UnityEditor.EditorApplication.isPlaying)
                {
                    instanceGmo.hideFlags = HideFlags.DontSave;
                }
                else {
                    instanceGmo.hideFlags = HideFlags.HideAndDontSave;
                }
#endif
                this.enemy = instanceGmo.GetComponent<Enemy>();
                enemy.SetPosition(startPosition, endPosition);
                instanceGmo.SetActive(false);
            }
        }

        public void SetEnemyFlags(bool explode,bool size,bool color)
        {
            if( this.enemy != null)
            {
                this.enemy.SetFlags(explode, size, color);
            }
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            if (instanceGmo)
            {
                GameObject.DestroyImmediate(instanceGmo);
            }
            base.OnPlayableDestroy(playable);
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
            if (instanceGmo && this.enemy)
            {
                this.enemy.TimeStart();
            }
        }


        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);
            if (instanceGmo && this.enemy)
            {
                this.enemy.TimeOver();
            }
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);
            if (this.instanceGmo && this.enemy)
            {
                this.enemy.SetTime((float)playable.GetTime(), (float)playable.GetDuration());
            }
        }
    }
}