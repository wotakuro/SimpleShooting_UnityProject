using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour
{

    public GameObject bulletPrefab;


    public Transform target;
    public bool isMachineGun;
    private Camera mainCamera;
    private float btnTime = 0.0f;

    void Start()
    {
        this.mainCamera = this.GetComponent<Camera>();
    }
	
    // カメラ自体を動かす可能性も考慮して敢えて…
	void LateUpdate () {
        bool inputFlag = false;

        // マシンガンモードの時の入力処理
        if (isMachineGun)
        {
            bool flag = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Return);
            if( flag)
            {
                btnTime += Time.deltaTime;
                if(btnTime > 0.1f)
                {
                    inputFlag = true;
                    btnTime = 0.0f;
                }
            }
            else
            {
                btnTime = 0.0f;
            }
        }
        // 通常時の処理
        else
        {
            inputFlag = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return);
        }

        var ray = mainCamera.ScreenPointToRay( Input.mousePosition );
        target.position = ray.origin + ray.direction * 10;


        if ( inputFlag )
        {
            GameObject obj = Instantiate(bulletPrefab, transform.position + ray.direction *2, transform.rotation)as GameObject;
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.delta = (target.position - transform.position ).normalized * 50;
        }
	}
}
