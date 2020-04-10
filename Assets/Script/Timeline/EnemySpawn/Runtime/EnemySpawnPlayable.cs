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

        public void SetEnemyPrefab(GameObject enemyPrefab, Vector3 startPosition, Vector3 endPosition)
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
                enemy = instanceGmo.GetComponent<Enemy>();
                enemy.SetPosition(startPosition, endPosition);
                instanceGmo.transform.LookAt(Vector3.zero);
                instanceGmo.SetActive(false);
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
            if (instanceGmo)
            {
                instanceGmo.SetActive(true);
            }
        }


        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);
            if (instanceGmo)
            {
                instanceGmo.SetActive(false);
            }
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);
            if (this.instanceGmo)
            {
                this.enemy.SetTime((float)playable.GetTime(), (float)playable.GetDuration());
            }
        }
    }
}