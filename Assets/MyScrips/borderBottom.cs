using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class borderBottom : MonoBehaviour
{
    public GameObject player;
    GameObject mainCamera;
    gameControl cameraScript;

    Transform playerTrans;
    Rigidbody2D playerRigid;
    Vector2 playerOrigin;


    private void Start()
    {
        playerTrans = player.GetComponent<Transform>();
        playerOrigin = playerTrans.position;
        playerRigid = player.GetComponent<Rigidbody2D>();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraScript = mainCamera.GetComponent<gameControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "playerTag")
        {
            playerTrans.position = playerOrigin;
            playerRigid.velocity = new Vector2(0, 0);
            cameraScript.direction = true;
        }
    }
}
