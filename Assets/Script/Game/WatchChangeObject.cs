using UnityEngine;
using System.Collections;
using System;
public class WatchChangeObject : MonoBehaviour
{

    public Renderer boardRenderer;
    public Renderer questionRenderer;

    public bool isWatch = false;

    public float pow = 0.0f;
    private Material boardMaterial;
    private Material questionMaterial;


    /// <summary>
    /// 見つかったかどうかを判定して返します
    /// </summary>
    public bool IsFound {
        get {
            return (this.pow > 1.5f);
        }
    }

    /// <summary>
    ///  見つけたときに呼び出す
    /// </summary>
    public Action<WatchChangeObject> OnFindObject
    {
        private get;
        set;
    }
    
    void Start()
    {
        this.isWatch = false;
        this.boardMaterial = this.boardRenderer.material;
        this.questionMaterial = this.questionRenderer.material;
    }
    void Update()
    {
        float oldPow = pow;
        if (!isWatch && this.pow < 0.5f)
        {
            this.pow -= Time.deltaTime;
        }
        else {
            this.pow += Time.deltaTime;
        }
        if (oldPow <= 0.9f && 0.9f < pow) {
            if (this.OnFindObject != null) {
                this.OnFindObject(this);
            }
        }
        this.pow = Mathf.Clamp(pow, 0.0f, 2.5f);
        this.setQuestionPower(this.pow * 2.0f);
        this.setBoardPower( this.pow - 0.6f );
//        this.boardMaterial.SetFloat("_LightPow", pow);
    }
    void OnApplicationPause(bool flag) {
        this.pow = 0.0f;
    }

    private void setQuestionPower(float p) {
        p = Mathf.Clamp(p, 0.0f, 1.0f);
        this.questionMaterial.SetFloat("_DirSize" , ( p - 1.0f ) * Mathf.PI * 2.0f );
    }
    private void setBoardPower(float p) {
        if( p < 0.01f ){
            this.boardRenderer.enabled = false;
            return;
        }
        this.boardRenderer.enabled = true;
        this.boardMaterial.SetFloat("_LightPow", Mathf.Clamp(p, 0.0f, 2.0f));
    }

    void OnWatchStart()
    {
        isWatch = true;
    }
    void OnWatching(float time)
    {
    }
    void OnWatchEnd()
    {
        isWatch = false;
    }
}
