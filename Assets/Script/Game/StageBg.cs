using UnityEngine;
using System.Collections;

public class StageBg : MonoBehaviour
{
    public float lineTm = -2.0f;
    private int lineID;
    // Use this for initialization
    void Start()
    {
        this.lineID = Shader.PropertyToID("_FieldLineLight");
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat(this.lineID, lineTm);
    }
}
