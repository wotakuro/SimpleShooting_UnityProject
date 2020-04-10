using UnityEngine;
using System.Collections;

public class ChildAttache : MonoBehaviour
{
    //! 親オブジェクトの指定
    public Transform target;

	// Use this for initialization
	void Start () {
        transform.parent = target;
	}
	
}
