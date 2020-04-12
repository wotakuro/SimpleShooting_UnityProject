using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAudio : MonoBehaviour
{
    public float time = 0.5f;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if( time == float.NaN) { return; }
        time -= Time.deltaTime;
        if (source != null && time < 0.0f)
        {
            source.Play();
            time = float.NaN;
        }
    }
}
