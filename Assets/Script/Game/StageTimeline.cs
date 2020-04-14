using UnityEngine;
using UnityEngine.Playables;

public class StageTimeline : MonoBehaviour
{
    private PlayableDirector playableDirector;
    // Start is called before the first frame update
    void Start()
    {
        playableDirector = this.GetComponent<PlayableDirector>();
        playableDirector.Stop();
        playableDirector.timeUpdateMode = DirectorUpdateMode.GameTime;
        playableDirector.Play();
        playableDirector.stopped += ChangeLevel;
    }
    
    private void ChangeLevel(PlayableDirector pd)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.SceneManagement.SceneManager.LoadScene("title");
#endif
    }
}
