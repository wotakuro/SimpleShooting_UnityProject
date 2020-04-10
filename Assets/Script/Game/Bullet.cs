using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    private float time = 0.0f;

    public Vector3 delta;
	// Use this for initialization
	void Start () {
        transform.LookAt( transform.position + delta);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 moved = delta * Time.deltaTime;

        Ray ray = new Ray(transform.position - moved, moved * 2.0f);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, moved.magnitude * 2.0f,1<<8))
        {
            hitInfo.transform.gameObject.SendMessage("Bomb");
            Destroy(this.gameObject);
            return;
        }

        transform.position = transform.position + moved;
        time += Time.deltaTime;
        if (time > 1.0f) {
            Destroy(this.gameObject);
        }
	}

}
