using UnityEngine;
using System.Collections;

public class TitleScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.8f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("game");
    }
}
