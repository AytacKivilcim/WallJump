using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    public GameObject deneme1, deneme2;
    public Text scoreTable, deathTable;
    public AudioClip[] sounds;
    GameObject mainCamera, currentWall, previousWall;
    Transform deneme1t, deneme2t, wallTrans1, wallTrans2, playerTrans;
    Rigidbody2D playerRigid, wallRigid;
    SpriteRenderer currentWallSprite, previousWallSprite;
    AudioSource sound;
    Vector2 deneme1v, deneme2v;
    Vector3 playerOrigin;
    gameControl cameraScript;
    float currentColorValue, previousColorValue, colorChangeScale, realTime2;
    int score, deathCount;
    bool first, second, third, fourth, fifth;

    private void Start()
    {
        canSpeedUp(true);
        sound = this.gameObject.GetComponent<AudioSource>();

        deneme1t = deneme1.GetComponent<Transform>();
        deneme2t = deneme2.GetComponent<Transform>();
        deneme1v = deneme1t.position;
        deneme2v = deneme2t.position;

        playerRigid = this.gameObject.GetComponent<Rigidbody2D>();
        playerTrans = this.gameObject.GetComponent<Transform>();
        playerOrigin = playerTrans.position;
        
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraScript = mainCamera.GetComponent<gameControl>();

        colorChangeScale = cameraScript.difficulty * 0.1f;
    }

    private void Update()
    {
        realTime2 += Time.deltaTime;
        if (currentWall != null)
        {
            if (currentColorValue <= 1)
            {
                currentColorValue += Time.deltaTime * colorChangeScale;
                //Debug.Log(currentColorValue);
                currentWallSprite.color = new Color(currentColorValue, currentColorValue, currentColorValue, 255f);
            }
        }
        if (currentWall && cameraScript.keepSpeedUp)
        {
            if (1f <= currentColorValue)
            {
                if (first)
                {
                    first = !first; second = true; third = true; fourth = true; fifth = true;
                    cameraScript.speedUp = cameraScript.maxSpeedUp;
                    cameraScript.speedUpWall(currentWall.tag, false);
                }
            }
            else if (0.75f <= currentColorValue && currentColorValue < 1f)
            {
                if (second)
                {
                    first = true; second = !second; third = true; fourth = true; fifth = true;
                    cameraScript.speedUp = 0.75f * cameraScript.maxSpeedUp;
                    cameraScript.speedUpWall(currentWall.tag, false);
                }
            }
            else if (0.5f <= currentColorValue && currentColorValue < 0.75f)
            {
                if (third)
                {
                    first = true; second = true; third = !third; fourth = true; fifth = true;
                    cameraScript.speedUp = 0.5f * cameraScript.maxSpeedUp;
                    cameraScript.speedUpWall(currentWall.tag, false);
                }
            }
            else if (0.25f <= currentColorValue && currentColorValue < 0.5f)
            {
                if (fourth)
                {
                    first = true; second = true; third = true; fourth = !fourth; fifth = true;
                    cameraScript.speedUp = 0.25f * cameraScript.maxSpeedUp;
                    cameraScript.speedUpWall(currentWall.tag, false);
                }
            }
            else
            {
                if (fifth)
                {
                    first = true; second = true; third = true; fourth = true; fifth = !fifth;
                    cameraScript.speedUp = 0f;
                    cameraScript.speedUpWall(currentWall.tag, false);
                }
            }
        }

        if (previousWall != null && previousWall != currentWall)
        {
            if (previousColorValue > 0)
            {
                previousColorValue -= Time.deltaTime * colorChangeScale;
                previousWallSprite.color = new Color(previousColorValue, previousColorValue, previousColorValue);
            }
        }
        if (currentWall != null && previousWall != null && currentWall != previousWall)
        {
            deneme1t.position = wallTrans1.position;
            deneme2t.position = wallTrans2.position;
        }
        if (currentWall != null)
        {
            deneme2t.position = wallTrans2.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("There is a collision on tag = " + collision.tag + " with time " + realTime2);
        if (realTime2 > 0.1f)
        {
            realTime2 = 0f;
            this.gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
            if (collision.tag == "spikeUpTag1" ||
                collision.tag == "spikeUpTag2" ||
                collision.tag == "spikeDownTag1" ||
                collision.tag == "spikeDownTag2" ||
                collision.tag == "borderLeftTag" ||
                collision.tag == "borderRightTag" ||
                collision.tag == "borderTopTag" ||
                collision.tag == "borderBottomTag")
            {
                //Debug.Log("SPIKE");
                //1. collision
                playerTrans.position = playerOrigin;
                playerRigid.velocity = new Vector2(0, 0);
                cameraScript.keepSpeedUp = false;
                cameraScript.direction = true;
                cameraScript.healthCount--;
                //cameraScript.healthCount++;
                cameraScript.healthBar.sprite = cameraScript.healthImages[cameraScript.healthCount];
                if (cameraScript.healthCount == 0)
                {
                    cameraScript.restartText.gameObject.SetActive(true);
                    cameraScript.gameOver = true;
                    //Destroy(this.gameObject);
                    //this.gameObject.SetActive(false);
                    Destroy(this.gameObject.GetComponent<SpriteRenderer>());
                }
                deathCount++;
                deathTable.text = "Deaths = " + deathCount;
                int tempChange = cameraScript.maxEnergy - cameraScript.energyCount;
                cameraScript.changeEnergyBar(tempChange);
                sound.clip = sounds[2];
                sound.volume = 0.2f;
                sound.Play();
                //+3 collision
                if (previousWall != null && previousWall != currentWall)
                    previousWallSprite.color = new Color(0, 0, 0);
                //2. collision
                if (currentWall != null)
                {
                    currentWallSprite.color = new Color(0, 0, 0);
                    previousWall = currentWall;
                    deneme1t.position = deneme1v;
                    deneme2t.position = deneme2v;
                    cameraScript.speedUp = 0f;
                    cameraScript.speedUpWall(currentWall.tag, true);
                    canSpeedUp(false);
                }
                currentWall = null;
            }
            else if (collision.tag == "wallUpTag1" ||
                collision.tag == "wallUpTag2" ||
                collision.tag == "wallDownTag1" ||
                collision.tag == "wallDownTag2")
            {
                //Debug.Log("WALL");
                cameraScript.keepSpeedUp = true;
                canSpeedUp(true);
                //+3 collision
                if (previousWall != null && previousWall != currentWall)
                    previousWallSprite.color = new Color(0, 0, 0);
                //2. collision
                if (currentWall != null)
                {
                    cameraScript.speedUp = 0f;
                    cameraScript.speedUpWall(currentWall.tag, false);
                    previousWall = currentWall;
                    wallTrans1 = previousWall.GetComponent<Transform>();
                    deneme1t.position = wallTrans1.position;
                    previousWallSprite = previousWall.GetComponent<SpriteRenderer>();
                    previousColorValue = currentColorValue;
                }
                //1. collision
                currentWall = collision.gameObject;
                wallRigid = currentWall.GetComponent<Rigidbody2D>();
                playerRigid.velocity = wallRigid.velocity;
                wallTrans2 = currentWall.GetComponent<Transform>();
                deneme2t.position = wallTrans2.position;
                currentWallSprite = currentWall.GetComponent<SpriteRenderer>();
                currentColorValue = currentWallSprite.color.r;
            }
            else if (collision.tag == "foodTag")
            {
                Destroy(collision.gameObject);
                score++;
                scoreTable.text = "Score = " + score;
                cameraScript.foodCountOnField--;
                if (cameraScript.energyCount < cameraScript.maxEnergy)
                {
                    cameraScript.changeEnergyBar(1);
                }
                sound.clip = sounds[1];
                sound.volume = 1f;
                sound.Play();
            }
            this.gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        }
    }
    private void canSpeedUp(bool input)
    {
        if (input)
        {
            first = true; second = true; third = true; fourth = true; fifth = true;
        }
        else
        {
            first = false; second = false; third = false; fourth = false; fifth = false;
        }

    }
}
