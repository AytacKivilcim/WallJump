using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameControl : MonoBehaviour
{
    public GameObject player;
    public Image energyBar;
    public Sprite[] energyImages;
    public Text restartText;
    public Image healthBar;
    public Sprite[] healthImages;
    public bool direction = true, keepSpeedUp, gameOver;
    public float ballSpeed, difficulty, yWallSpeed, speedUp, maxSpeedUp;
    public int foodStartAmount, foodCountOnField, energyCount, maxEnergy, healthCount = 3;
    GameObject food;
    GameObject[] spikes1, spikes2, spikes3, spikes4, walls1, walls2, walls3, walls4;
    Rigidbody2D playerRigid, wallRigid, spikeRigid;
    Vector2[] vecs = new Vector2[4];
    bool gameStart;
    float realTime1, energyRestoreDelay, difficultyScale;

    private void Start()
    {
        healthBar.sprite = healthImages[healthCount];
        restartText.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("playerTag");
        playerRigid = player.GetComponent<Rigidbody2D>();

        walls1 = GameObject.FindGameObjectsWithTag("wallUpTag1");
        walls3 = GameObject.FindGameObjectsWithTag("wallUpTag2");
        walls2 = GameObject.FindGameObjectsWithTag("wallDownTag1");
        walls4 = GameObject.FindGameObjectsWithTag("wallDownTag2");
        spikes1 = GameObject.FindGameObjectsWithTag("spikeUpTag1");
        spikes3 = GameObject.FindGameObjectsWithTag("spikeUpTag2");
        spikes2 = GameObject.FindGameObjectsWithTag("spikeDownTag1");
        spikes4 = GameObject.FindGameObjectsWithTag("spikeDownTag2");
        food = GameObject.FindGameObjectWithTag("foodTag");

        maxEnergy = energyImages.Length - 1;
        energyCount = maxEnergy;
        if (!gameStart)
            difficultyScale = difficulty * 0.2f;
        if (energyRestoreDelay<2f)
            energyRestoreDelay = difficulty * difficultyScale;
        //Debug.Log("energyRestoreDelay : " + energyRestoreDelay);

        for (int i = 0; i < vecs.Length; i++)
        {
            yWallSpeed = UnityEngine.Random.Range(2, 5) * difficultyScale * 2;
            if (yWallSpeed > 8)
                yWallSpeed = 8;
            vecs[i] = new Vector2(0, yWallSpeed * (float)Math.Pow(-1, i));
            //Debug.Log("Vec" + i + " = " + yWallSpeed * Math.Pow(-1, i));
        }
        for (int i = 0; i < walls1.Length; i++)
        {
            wallRigid = walls1[i].GetComponent<Rigidbody2D>();
            wallRigid.velocity = vecs[0];
            wallRigid = walls2[i].GetComponent<Rigidbody2D>();
            wallRigid.velocity = vecs[1];
            wallRigid = walls3[i].GetComponent<Rigidbody2D>();
            wallRigid.velocity = vecs[2];
            wallRigid = walls4[i].GetComponent<Rigidbody2D>();
            wallRigid.velocity = vecs[3];
        }
        for (int i = 0; i < spikes1.Length; i++)
        {
            spikeRigid = spikes1[i].GetComponent<Rigidbody2D>();
            spikeRigid.velocity = vecs[0];
            spikeRigid = spikes2[i].GetComponent<Rigidbody2D>();
            spikeRigid.velocity = vecs[1];
            spikeRigid = spikes3[i].GetComponent<Rigidbody2D>();
            spikeRigid.velocity = vecs[2];
            spikeRigid = spikes4[i].GetComponent<Rigidbody2D>();
            spikeRigid.velocity = vecs[3];
        }
        difficultyScale += 0.1f;
        createFood();
        gameStart = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (energyCount > 0 && !gameOver)
            {
                keepSpeedUp = false;
                changeEnergyBar(-1);
                if (direction)
                {
                    playerRigid.velocity = new Vector2(-ballSpeed, 0);
                    direction = !direction;
                }
                else
                {
                    playerRigid.velocity = new Vector2(ballSpeed, 0);
                    direction = !direction;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gameOver)
            {
                SceneManager.LoadScene("GameScene");
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
        if (foodCountOnField == 0 && gameStart)
        {
            Start();
            changeEnergyBar(maxEnergy-energyCount);
            healthCount = 3;
            healthBar.sprite = healthImages[healthCount];
        }
        if (energyCount < maxEnergy)
        {
            realTime1 += Time.deltaTime;
            if (energyRestoreDelay < realTime1)
            {
                realTime1 = 0;
                changeEnergyBar(1);
            }
        }
    }

    private void createFood()
    {
        int minY=-6, maxY=6;
        for (int i = 0; i < foodStartAmount; i++)
        {
            Vector2 vec1 = new Vector2(UnityEngine.Random.Range(0, 2)*16 -8, UnityEngine.Random.Range(minY, maxY)*2);
            Instantiate(food, vec1, Quaternion.identity);
            foodCountOnField++;
        }
    }
    public void changeEnergyBar(int change)
    {
        energyCount += change;
        energyBar.sprite = energyImages[energyCount];
    }

    public void speedUpWall(string tag, bool isDead)
    {
        //Debug.Log("speeding up " + speedUp);
        char[] charArray = tag.ToCharArray();
        Vector2 tempVec = new Vector2(0, speedUp);
        if (charArray[4].Equals('U'))
        {
            if (charArray[charArray.Length - 1].Equals('1'))
            {
                for (int i = 0; i < walls1.Length; i++)
                {
                    wallRigid = walls1[i].GetComponent<Rigidbody2D>();
                    wallRigid.velocity = vecs[0] + tempVec;
                    if(!isDead)
                        playerRigid.velocity = wallRigid.velocity;
                }
                for (int i = 0; i < spikes1.Length; i++)
                {
                    spikeRigid = spikes1[i].GetComponent<Rigidbody2D>();
                    spikeRigid.velocity = vecs[0] + tempVec;
                }
            }
            else if (charArray[charArray.Length - 1].Equals('2'))
            {
                for (int i = 0; i < walls3.Length; i++)
                {
                    wallRigid = walls3[i].GetComponent<Rigidbody2D>();
                    wallRigid.velocity = vecs[2] + tempVec;
                    if (!isDead)
                        playerRigid.velocity = wallRigid.velocity;
                }
                for (int i = 0; i < spikes3.Length; i++)
                {
                    spikeRigid = spikes3[i].GetComponent<Rigidbody2D>();
                    spikeRigid.velocity = vecs[2] + tempVec;
                }
            }
        }
        else if (charArray[4].Equals('D'))
        {
            if (charArray[charArray.Length - 1].Equals('1'))
            {
                for (int i = 0; i < walls2.Length; i++)
                {
                    wallRigid = walls2[i].GetComponent<Rigidbody2D>();
                    wallRigid.velocity = vecs[1] - tempVec;
                    if (!isDead)
                        playerRigid.velocity = wallRigid.velocity;
                }
                for (int i = 0; i < spikes2.Length; i++)
                {
                    spikeRigid = spikes2[i].GetComponent<Rigidbody2D>();
                    spikeRigid.velocity = vecs[1] - tempVec;
                }
            }
            else if (charArray[charArray.Length - 1].Equals('2'))
            {
                for (int i = 0; i < walls4.Length; i++)
                {
                    wallRigid = walls4[i].GetComponent<Rigidbody2D>();
                    wallRigid.velocity = vecs[3] - tempVec;
                    if(!isDead)
                    playerRigid.velocity = wallRigid.velocity;
                }
                for (int i = 0; i < spikes4.Length; i++)
                {
                    spikeRigid = spikes4[i].GetComponent<Rigidbody2D>();
                    spikeRigid.velocity = vecs[3] - tempVec;
                }
            }
        }
    }
}
