using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyPreviewDrawer : System.IDisposable
{
    const int PREVIEW_LAYER = 31;

    public Vector3 groundPos = Vector3.zero;
    private Camera camera;
    private GameObject prefab;
    private GameObject instanceObject;
    public RenderTexture renderTexture { 
        get {
            if (!_renderTextureData)
            {
                _renderTextureData = new RenderTexture(renderSize.x, renderSize.y, 24);
                this.Render();
            }
            return _renderTextureData;
        }
        private set {
            _renderTextureData = value;
        } 
    }
    private double previewTime;

    private Animator[] animators;
    private RenderTexture _renderTextureData;
    private Vector2Int renderSize;

    public EnemyPreviewDrawer()
    {
        this.renderSize = new Vector2Int(256, 256);
        previewTime = EditorApplication.timeSinceStartup;
    }

    public void SetPrefab(GameObject obj)
    {
        if( this.prefab == obj) { return; }
        if (instanceObject != null)
        {
            GameObject.DestroyImmediate(instanceObject);
        }
        instanceObject = null;

        this.prefab = obj;
    }

    public void SetRenderSize(int w , int h)
    {
        if(w == this.renderSize.x && h == this.renderSize.y)
        {
            return;
        }
        if (this.camera != null)
        {
            var rt = camera.targetTexture;
            this.camera.targetTexture = null;
            if( rt != null) { rt.Release(); }
            this.renderTexture = new RenderTexture(w, h, 24);
            this.camera.targetTexture = this.renderTexture;
        }

        this.renderSize.x = w;
        this.renderSize.y = h;
    }

    public void Render()
    {
        HideFlags hideFlag = HideFlags.HideAndDontSave;
        // setup camera
        if ( camera == null)
        {
            var cameraObj = new GameObject("PrevCamera");
            cameraObj.hideFlags = hideFlag;
            cameraObj.transform.position = new Vector3(0, 0, -2.5f) + groundPos;
            cameraObj.transform.rotation = Quaternion.identity;
            cameraObj.layer = PREVIEW_LAYER;

            this.camera = cameraObj.AddComponent<Camera>();
            this.renderTexture = new RenderTexture(renderSize.x, renderSize.y, 24);
            this.camera.cullingMask = 1 << PREVIEW_LAYER;
            camera.targetTexture = renderTexture;
            camera.fieldOfView = 45;
            camera.clearFlags = CameraClearFlags.Color;
        }
        // setup instance obj
        if( instanceObject == null)
        {
            instanceObject = GameObject.Instantiate(prefab, groundPos, Quaternion.LookRotation(Vector3.back, Vector3.up) );
            instanceObject.hideFlags = hideFlag;
            SetLayerRecursive(instanceObject.transform, PREVIEW_LAYER);
            SetUpForAnimator(instanceObject);
        }
        //アクティブ化
        this.camera.gameObject.SetActive(true);
        this.instanceObject.gameObject.SetActive(true);

        UpdateAnimators();
        this.camera.Render();
        // 非アクティブ化
        this.camera.gameObject.SetActive(false);

        this.instanceObject.gameObject.SetActive(false);
    }

    // 強制的に計算するためにSkinnMeshセットアップ
    private void SetUpForAnimator(GameObject gmo)
    {
        this.animators = gmo.GetComponentsInChildren<Animator>();
        var skinMeshes = gmo.GetComponentsInChildren<SkinnedMeshRenderer>();
        if(skinMeshes == null) { return; }
        foreach( var skin in skinMeshes)
        {
            skin.forceMatrixRecalculationPerRender = true;
            skin.updateWhenOffscreen = true;
        }
        foreach( var anim in animators)
        {
            anim.keepAnimatorControllerStateOnDisable = true;
        }
    }
    private void UpdateAnimators()
    {
        if( this.animators == null) { return; }
        double current = EditorApplication.timeSinceStartup;
        float time = (float)(current- previewTime );
        foreach ( var anim in animators)
        {
            anim.Update(time);
        }
        previewTime = current;
    }

    private void SetLayerRecursive(Transform trs , int layer)
    {
        for (int i = 0; i < trs.childCount; ++i) {
            SetLayerRecursive(trs.GetChild(i), layer);
        }
        trs.gameObject.layer = layer;

    }


    public void Dispose()
    {
        if( this.camera != null)
        {
            this.camera.targetTexture = null;
            GameObject.DestroyImmediate(camera.gameObject);
        }
        this.camera = null;
        if (instanceObject != null)
        {
            GameObject.DestroyImmediate(instanceObject);
        }
        instanceObject = null;
        if( this.renderTexture != null)
        {
            this.renderTexture.Release();
        }
        this.renderTexture = null;
    }
}
