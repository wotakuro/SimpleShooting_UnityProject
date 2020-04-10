using UnityEngine;
using System.Collections;

public class TimerDisappear : MonoBehaviour {
    public float limitSecond = 1.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        limitSecond -= Time.deltaTime;
        if (limitSecond < 0.0f) {
            Destroy(this.gameObject);
        }
	}
}
