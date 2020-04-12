using UnityEngine;
using System.Collections;

public class TitleTap : MonoBehaviour
{
    public GameObject bombPrefab;



    public void ToGame()
    {
        //
        GameObject obj = Instantiate(bombPrefab, transform.position, transform.rotation) as GameObject;
//        scaler.particleScale = transform.localScale.magnitude;
        GameObject.Find("Title").SendMessage("ChangeScene");
        Destroy(this.gameObject);
    }

}
