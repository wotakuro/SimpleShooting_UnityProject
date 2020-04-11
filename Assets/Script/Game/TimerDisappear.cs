using UnityEngine;
using System.Collections;

public class TimerDisappear : MonoBehaviour {
    public float limitSecond = 1.0f;

	// Update is called once per frame
	void Update () {
        limitSecond -= Time.deltaTime;
        if (limitSecond < 0.0f) {
            Destroy(this.gameObject);
        }
	}
}
