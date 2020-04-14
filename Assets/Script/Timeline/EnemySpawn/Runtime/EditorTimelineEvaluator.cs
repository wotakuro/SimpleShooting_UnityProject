using UnityEngine.Playables;
using System.Collections.Generic;
#if UNITY_EDITOR

namespace TimelineExtention
{
    // なんかPlayableのTimeが正しく渡らないことがあるので、何回か評価します。
    public class EditorTimelineEvaluator
    {
        private PlayableDirector director;
        private double evaluateTime;
        private int cnt;
        private System.Action callBack;

        private static List<EditorTimelineEvaluator> evaluators = new List<EditorTimelineEvaluator>();

        public static void Clear()
        {
            evaluators.Clear();
        }

        public static void Evaluate(PlayableDirector director)
        {
            var obj = new EditorTimelineEvaluator(director,evaluators.Count);
            if(director) { director.Evaluate(); }
            evaluators.Add(obj);

            UnityEditor.EditorApplication.update += obj.Update;
        }


        public static void Evaluate(PlayableDirector director, 
            double time, System.Action act )
        {
            var obj = new EditorTimelineEvaluator(director, evaluators.Count, time,act);
            evaluators.Add(obj);
            UnityEditor.EditorApplication.update += obj.UpdateWithTime;
        }

        public EditorTimelineEvaluator(PlayableDirector d,int delayIndex)
        {
            this.director = d;
            this.cnt = -delayIndex * 4;
        }
        public EditorTimelineEvaluator(PlayableDirector d, int delayIndex,double time,System.Action act  )
        {
            this.director = d;
            this.evaluateTime = time;
            this.cnt = -delayIndex * 4;
            this.callBack = act;
        }

        public void UpdateWithTime()
        {
            if( cnt < 0)
            {
                cnt++;
                return;
            }
            if( cnt == 0)
            {
                this.director.time = this.evaluateTime;
            }
            this.director.Evaluate();

            cnt++;
            if ( cnt >= 3)
            {
                if( this.callBack != null) { callBack(); }
                evaluators.Remove(this);
                UnityEditor.EditorApplication.update -= this.UpdateWithTime;
            }
        }

        public void Update()
        {
            ++cnt;
            if(cnt <= 0)
            {
                return;
            }
            if (this.director)
            {
                this.director.Evaluate();
            }
            if (cnt > 4)
            {
                evaluators.Remove(this);
                UnityEditor.EditorApplication.update -= this.Update;
            }
        }
    }
}
#endif