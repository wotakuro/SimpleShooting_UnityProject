using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour
{

    public GameObject bulletPrefab;


    public Transform target;
    public Camera mainCamera;

	
    // カメラ自体を動かす可能性も考慮して敢えて…
	void LateUpdate () {
        bool inputFlag = false;

        inputFlag = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return);

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
