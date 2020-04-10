using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public GameObject bombPrefab;

    private bool bombFlag = false;
    private float bombTimer = 0.0f;

    private float timer = 0.0f;

    private int addScore = 1;
    private Renderer[] renders;
    private MaterialPropertyBlock materialPropertyBlock;
    private int colorProp;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Color whiteCol = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color redCol = new Color(1.0f, 0.0f, 0.0f, 0.8f);
    private Color blackCol = new Color(0.0f, 0.0f, 0.0f, 0.0f);

#if UNITY_EDITOR
    private Animator animator;
#endif


    // Use this for initialization
    public void SetPosition (Vector3 startPos,Vector3 endPos) {
        this.transform.position = startPos;

        this.startPosition = startPos;
        this.endPosition = endPos;
	}
	
	// Update is called once per frame
	void Update () {
        if (bombFlag)
        {
            bombTimer -= Time.deltaTime;
            if (bombTimer < 0)
            {
                Bomb();
            }
        }
	}

    public void SetTime(float tm,float length)
    {
        if(this &&  this.renders == null)
        {
            this.renders = this.GetComponentsInChildren<Renderer>();
            if(this.renders == null) { this.renders = new Renderer[0]; }
            this.materialPropertyBlock = new MaterialPropertyBlock();
            colorProp = Shader.PropertyToID("_Color");
        }

#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            if (this.animator == null)
            {
                this.animator = GetComponentInChildren<Animator>();
            }
            if (this.animator != null)
            {
                this.animator.Rebind();
                this.animator.Update(tm);
            }
        }
#endif
        
        this.transform.position = Vector3.Lerp(startPosition, endPosition, tm / length);

        float beginSmallerStart = length - 0.5f;
        timer = tm;
        float normalTime = length - 1.7f;
        if (timer < normalTime)
        {
            materialPropertyBlock.SetColor(colorProp, whiteCol);
        }
        else if (timer < beginSmallerStart)
        {
            float tmpP = (timer - normalTime) / (beginSmallerStart- normalTime);
            materialPropertyBlock.SetColor( colorProp , Color.Lerp(whiteCol, redCol, tmpP));
        }
        else if (timer < length)
        {
            materialPropertyBlock.SetColor(colorProp, Color.Lerp(redCol, blackCol, Mathf.Clamp01((timer - 4.5f) * 3.0f)));
        }
        foreach (var render in this.renders)
        {
            if (render)
            {
                //render.SetPropertyBlock(this.materialPropertyBlock);
            }
        }

        if (timer < beginSmallerStart)
        {
            this.transform.localScale = Vector3.one * Mathf.Sqrt(timer);
        }
        else
        {
            this.transform.localScale = Vector3.one * (Mathf.Sqrt(beginSmallerStart) * ((length - timer) * 2.0f));
        }
    }

    public void SetBomb(float distance) {
        if (bombFlag)
        {
            return;
        }
        bombFlag = true;
        bombTimer = Mathf.Sqrt( distance ) * 0.2f;
    }


    void Bomb()
    {
        float radius = this.transform.localScale.magnitude * 1.8f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1 << 8);
        foreach (var hit in colliders)
        {
            Enemy ene = hit.gameObject.GetComponent<Enemy>();
            ene.SetBomb((hit.transform.position - transform.position).magnitude);
            ene.addScore = 2;
        }
        //
        if (bombPrefab)
        {
            GameObject obj = Instantiate(bombPrefab, transform.position, transform.rotation) as GameObject;
            //        scaler.particleScale = transform.localScale.magnitude * 0.4f;

            TimerDisappear timer = obj.AddComponent<TimerDisappear>();
            timer.limitSecond = 1.0f;
        }
        ScoreSystem.AddScore(addScore );
        Destroy(this.gameObject);
    }
    
}
