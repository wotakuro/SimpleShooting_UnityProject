using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public GameObject hitPrefab;

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

        if (Physics.Raycast(ray, out hitInfo, moved.magnitude * 2.0f,1<<Enemy.LAYER))
        {
            // 敵にヒットしたことを伝えます
            var enemy = hitInfo.transform.gameObject.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.OnHitBullet();
            }
            else
            {
                // タイトル画面用の処理
                var titleTap = hitInfo.transform.gameObject.GetComponent<TitleTap>();
                if(titleTap)
                {
                    titleTap.ToGame();
                }
            }
            // ヒットエフェクト
            if(hitPrefab)
            {
                GameObject.Instantiate(hitPrefab, transform.position, transform.rotation);
            }
            Destroy(this.gameObject);
            return;
        }

        transform.position = transform.position + moved;
        time += Time.deltaTime;
        if (time > 2.0f) {
            Destroy(this.gameObject);
        }
	}

}
