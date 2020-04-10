using UnityEngine;
using System.Collections;

public class TitleTap : MonoBehaviour
{
    public GameObject bombPrefab;

    private bool bombFlag = false;
    private float bombTimer = 0.0f;

    private float timer = 0.0f;

    private int addScore = 1;
    private Material mat;

	// Use this for initialization
	void Start () {
        Renderer render = this.GetComponent<Renderer>();
        this.mat = render.material;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void SetBomb(float distance) {
        if (bombFlag)
        {
            return;
        }
        bombFlag = true;
        bombTimer = Mathf.Sqrt( distance ) * 0.2f;
    }

    void OnWatchStart()
    {
        if (this.mat != null) {
            this.mat.color = Color.yellow;
        }
    }
    void OnWatching(float time)
    {
    }
    void OnWatchEnd()
    {
        if (this.mat != null)
        {
            this.mat.color = Color.white;
        }
    }

    void Bomb()
    {
        //
        GameObject obj = Instantiate(bombPrefab, transform.position, transform.rotation) as GameObject;
//        scaler.particleScale = transform.localScale.magnitude;

        TimerDisappear timer = obj.AddComponent<TimerDisappear>();
        timer.limitSecond = 1.0f;
        GameObject.Find("Title").SendMessage("ChangeScene");
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision collision) {
    }
}
