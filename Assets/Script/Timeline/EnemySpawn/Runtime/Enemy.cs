using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public static readonly int LAYER = 8;
    private static readonly Color whiteCol = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private static readonly Color redCol = new Color(1.0f, 0.0f, 0.0f, 0.8f);
    private static readonly Color blackCol = new Color(0.0f, 0.0f, 0.0f, 0.0f);

    public GameObject bombPrefab;

    private bool bombFlag = false;
    private float bombTimer = 0.0f;

    private float timer = 0.0f;

    private int addScore = 1;
    private SpriteRenderer spriteRenderer;
    private int colorProp;
    // Timelineから設定される項目
    private bool isExplosion = true;
    private bool isSizeChange = true;
    private bool isColorChange = true;
    private Vector3 startPosition;
    private Vector3 endPosition;


#if UNITY_EDITOR
    private Animator animator;
#endif

    private void Awake()
    {
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }

    // Use this for initialization
    public void SetPosition (Vector3 startPos,Vector3 endPos) {
        this.transform.position = startPos;

        this.startPosition = startPos;
        this.endPosition = endPos;

        this.transform.LookAt(endPos);
	}
    public void SetFlags(bool explosion,bool size,bool color )
    {
        this.isExplosion = explosion;
        this.isSizeChange = size;
        this.isColorChange = color;
    }
	
	// Update is called once per frame
	void Update () {
        if (bombFlag)
        {
            bombTimer -= Time.deltaTime;
            if (bombTimer < 0)
            {
                Die();
            }
        }
	}

    public void TimeStart()
    {
        this.gameObject.SetActive(true);
    }

    public void TimeOver()
    {
        this.gameObject.SetActive(false);
    }

    public void SetTime(float tm,float length)
    {
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
        // 座標移動
        this.transform.position = Vector3.Lerp(startPosition, endPosition, tm / length);
        // 色チェンジ
        if (isColorChange)
        {
            SetSpriteColorByTime(timer, length);
        }

        // サイズ変更
        if (isSizeChange)
        {
            SetSizeByTime(timer, length);
        }
        timer = tm;
    }

    private void SetSpriteColorByTime(float timer,float length)
    {
        float beginSmallerStart = length - 0.5f;
        float normalTime = length - 1.7f;
        Color color = Color.white;
        if (timer < normalTime)
        {
            color = whiteCol;
        }
        else if (timer < beginSmallerStart)
        {
            float tmpP = (timer - normalTime) / (beginSmallerStart - normalTime);
            color = Color.Lerp(whiteCol, redCol, tmpP);
        }
        else if (timer < length)
        {
            color = Color.Lerp(redCol, blackCol, Mathf.Clamp01((timer - 4.5f) * 3.0f));
        }
        if (spriteRenderer)
        {
            spriteRenderer.color = color;
        }

    }

    private void SetSizeByTime(float timer,float length)
    {
        float beginSmallerStart = length - 0.5f;

        if( timer < 0.5f)
        {
            this.transform.localScale = Vector3.one * timer * 2.0f;
        }
        else if (timer > beginSmallerStart)
        {
            this.transform.localScale = Vector3.one * (( length - timer) *2.0f);
        }
        else
        {
            this.transform.localScale = Vector3.one;
        }
    }

    private void SetBomb(float distance) {
        if (bombFlag)
        {
            return;
        }
        bombFlag = true;
        bombTimer = Mathf.Sqrt( distance ) * 0.2f;
    }

    public void OnHitBullet()
    {
        Die();
    }

    // 死亡時処理
    private void Die()
    {
        // 誘爆ロジック
        if (isExplosion)
        {
            float radius = this.transform.localScale.magnitude * 2.8f;
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1 << LAYER);
            foreach (var hit in colliders)
            {
                Enemy ene = hit.gameObject.GetComponent<Enemy>();
                if (ene != this)
                {
                    ene.SetBomb((hit.transform.position - transform.position).magnitude);
                    ene.addScore = this.addScore + 1;
                }
            }
            if (bombPrefab)
            {
                GameObject obj = Instantiate(bombPrefab, transform.position, transform.rotation) as GameObject;
                TimerDisappear timer = obj.AddComponent<TimerDisappear>();
                timer.limitSecond = 1.0f;
            }
        }
        //
        ScoreSystem.AddScore(addScore );
        Destroy(this.gameObject);
    }
    
}
