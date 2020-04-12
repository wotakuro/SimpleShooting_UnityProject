using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour
{
    public enum PlayerMode :int
    {
        Non=0 ,
        Normal = 1, 
        MachinGun = 2
    }
    public static readonly string[] PlayModeText ={
        "撃てません",
        "普通のガン",
        "マシンガン"
    };
    public AudioClip noGunAudio;
    public AudioClip normalGunAudio;
    public AudioClip machineGunAudio;


    public GameObject bulletPrefab;


    public Renderer target;
    private PlayerMode playMode;

    private PlayerMode previewPlayMode;
    private Camera mainCamera;
    private float btnTime = 0.0f;

    private MaterialPropertyBlock materialPropertyBlock;

    public void SetPlayMode(PlayerMode mode)
    {
        this.playMode = mode;
    }

    void Start()
    {
        previewPlayMode = playMode;
        playMode = PlayerMode.Normal;
        this.mainCamera = this.GetComponent<Camera>();
        materialPropertyBlock = new MaterialPropertyBlock();
    }

    // カメラ自体を動かす可能性も考慮して敢えて…
    void LateUpdate () {
        bool inputFlag = false;
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPosition = ray.origin + ray.direction * 10;
        target.transform.position = targetPosition;

        if (previewPlayMode != playMode)
        {

            switch (playMode)
            {
                case PlayerMode.Non:
                    materialPropertyBlock.SetColor("_Color", Color.black);
                    if (noGunAudio) { AudioSource.PlayClipAtPoint(noGunAudio, transform.position); }
                    break;
                case PlayerMode.Normal:
                    materialPropertyBlock.SetColor("_Color", Color.white);
                    if (normalGunAudio) { AudioSource.PlayClipAtPoint(normalGunAudio, transform.position); }
                    break;
                case PlayerMode.MachinGun:
                    materialPropertyBlock.SetColor("_Color", Color.red);
                    if (machineGunAudio) { AudioSource.PlayClipAtPoint(machineGunAudio, transform.position); }
                    break;
            }
            target.SetPropertyBlock(materialPropertyBlock);
        }

        previewPlayMode = playMode;

        // マシンガンモードの時の入力処理
        switch (playMode)
        {
            case PlayerMode.MachinGun:
                {
                    bool flag = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Return);
                    if (flag)
                    {
                        btnTime += Time.deltaTime;
                        if (btnTime > 0.1f)
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
                break;
            case PlayerMode.Normal:
                inputFlag = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return);
                break;
        }

        


        if ( inputFlag )
        {
            GameObject obj = Instantiate(bulletPrefab, transform.position + ray.direction *2, transform.rotation)as GameObject;
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.delta = (targetPosition - transform.position ).normalized * 50;
        }
	}
}
