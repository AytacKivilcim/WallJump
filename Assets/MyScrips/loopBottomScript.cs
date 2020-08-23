using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopBottomScript : MonoBehaviour
{
    Transform objectTrans;
    Vector2 vec1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "wallDownTag1" || collision.tag == "wallDownTag2")
        {
            objectTrans = collision.GetComponent<Transform>();
            float x = objectTrans.position.x;
            float y = objectTrans.position.y;
            vec1 = new Vector2(x, y+40);
            objectTrans.position = vec1;
        }


    }
}
