using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loopTopScript : MonoBehaviour
{
    Transform objectTrans;
    Vector2 vec1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "wallUpTag1" || collision.tag == "wallUpTag2")
        {
            objectTrans = collision.GetComponent<Transform>();
            float x = objectTrans.position.x;
            float y = objectTrans.position.y;
            vec1 = new Vector2(x, y-40);
            objectTrans.position = vec1;
        }

        
    }
}
