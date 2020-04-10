using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {

    public static int score = 0;
    private static ScoreSystem instance = null;

	// Use this for initialization
	void Start () {
        score = 0;
        instance = this;
	}
    void OnDestroy(){
    }
	

    public static void AddScore(int p)
    {
        score += p;
        Text text = instance.gameObject.GetComponent<Text>();
        text.text = "score " + score;
    }
}
