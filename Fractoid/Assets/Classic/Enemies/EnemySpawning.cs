using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemySpawning : MonoBehaviour {

    //Linked enemy prefabs
    public Transform standardPF;
    public Transform densePF;
    public Transform volatilePF;
    public Transform scopedPF;
    public Transform clandestinePF;
    public Transform imperviousPF;

    //List of levels
    List<Action> stageList = new List<Action>();

    //Current level, beginning with zero
    int currentLevel = 0;

    //Random position vector variable for reducing garbage
    Vector3 spawnPosition = new Vector3(11, 0, 0);

    //Counters number of enemies spawned
    int standardCount;
    int denseCount;
    int volatileCount;
    int scopedCount;
    int clandestineCount;
    int imperviousCount;

    //Timers controlling spawn frequency
    float standardTimer;
    float denseTimer;
    float volatileTimer;
    float scopedTimer;
    float clandestineTimer;
    float imperviousTimer;

    //Variables controlling advanced spawn behavior
    float endlessSpawnMaxTime = 1f;
    float tempTimer;
    List<Vector3> spawnPositionList1 = new List<Vector3>();

    //Controls pauses between levels
    bool interlude = true;

    //Ensures single reset call
    bool reset = false;

    //Button and panel script components
    public Button nextButton;
    public Image nextPanel;
    public Text nextText;

    //Panel color
    Color nextPanelColor = new Color(0.2f, 0.925f, 1f, 1f);

    //Exits interlude by button
    public void ExitInterlude()
    {
        //Runs during interlude
        if (interlude)
        {
            if (currentLevel < stageList.Count)
            {
                //Deactivates button
                nextButton.interactable = false;
                nextPanel.color = Color.clear;
                nextText.color = Color.clear;

                interlude = false;
            }
            else
            {
                Debug.Log("Max Level");
            }
        }
    }

    //Resets counters and timers
    public void ResetControllers(float standardDelay, float denseDelay, float volatileDelay, float scopedDelay, float clandestineDelay, float imperviousDelay)
    {
        standardCount = 0;
        denseCount= 0;
        volatileCount = 0;
        scopedCount = 0;
        clandestineCount = 0;
        imperviousCount = 0;

        standardTimer = standardDelay;
        denseTimer = denseDelay;
        volatileTimer = volatileDelay;
        scopedTimer = scopedDelay;
        clandestineTimer = clandestineDelay;
        imperviousTimer = imperviousDelay;

        //Resets temporary timer
        tempTimer = 0;

        //Clears spawn lists
        spawnPositionList1.Clear();
    }

    //Stage functions
    public void Stage1()
    {
        //Confirms first reset call
        if (reset == false)
        {
            Debug.Log("Stage 1");

            //Resets counters and timers
            ResetControllers(1f, 0, 0, 0, 0, 0);
            
            //Prevents further reset calls in this level
            reset = true;
        }

        //Decrements timers
        standardTimer -= Time.deltaTime;

        //Spawns enemies based on count and timer
        if (standardCount < 8 && standardTimer <= 0)
        {
            //Generates random spawn position
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            //Instantiates clone of enemy
            Instantiate(standardPF, spawnPosition, Quaternion.identity, gameObject.transform);

            //Resets spawn timer
            standardTimer = 2f;

            //Increments spawn counter
            standardCount++;

            Debug.Log(standardCount);
        }

        //Conditions for level end
        if (standardCount >= 8 && transform.childCount == 0)
        {
            //Starts interlude
            interlude = true;

            //Enables next reset
            reset = false;

            //Increments current level
            currentLevel++;

            //Activates next button
            nextButton.interactable = true;
            nextPanel.color = nextPanelColor;
            nextText.color = Color.white;

            Debug.Log("Stopped");
        }
    }
    public void Stage2()
    {
        //Confirms first reset call
        if (reset == false)
        {
            Debug.Log("Stage 2");

            //Resets counters and timers
            ResetControllers(1f, 0, 0, 0, 0, 0);

            //Prevents further reset calls in this level
            reset = true;
        }

        //Decrements timers
        standardTimer -= Time.deltaTime;

        //Spawns enemies based on count and timer
        if (standardCount < 15 && standardTimer <= 0)
        {
            //Generates random spawn position
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            //Instantiates clone of enemy
            Instantiate(standardPF, spawnPosition, Quaternion.identity, gameObject.transform);

            //Resets spawn timer
            standardTimer = 1f;

            //Increments spawn counter
            standardCount++;

            Debug.Log(standardCount);
        }

        //Conditions for level end
        if (standardCount >= 15 && transform.childCount == 0)
        {
            //Starts interlude
            interlude = true;

            //Enables next reset
            reset = false;

            //Increments current level
            currentLevel++;

            //Activates next button
            nextButton.interactable = true;
            nextPanel.color = nextPanelColor;
            nextText.color = Color.white;

            Debug.Log("Stopped");
        }
    }
    public void Stage3()
    {
        //Confirms first reset call
        if (reset == false)
        {
            Debug.Log("Stage 3");

            //Resets counters and timers
            ResetControllers(1f, 0, 0, 0, 0, 0);

            //Prevents further reset calls in this level
            reset = true;
        }

        //Decrements timers
        standardTimer -= Time.deltaTime;

        //Spawns enemies based on count and timer
        if (standardCount < 7 && standardTimer <= 0)
        {
            //Generates random spawn position
            spawnPosition.y = 0;

            //Instantiates clone of enemy
            Instantiate(standardPF, spawnPosition, Quaternion.identity, gameObject.transform);

            //Resets spawn timer
            standardTimer = 0.8f;

            //Increments spawn counter
            standardCount++;

            Debug.Log(standardCount);
        }
        else if (standardCount < 14 && standardTimer <= 0)
        {
            //Generates random spawn position
            spawnPosition.y = 5;

            //Instantiates clone of enemy
            Instantiate(standardPF, spawnPosition, Quaternion.identity, gameObject.transform);

            //Resets spawn timer
            standardTimer = 0.8f;

            //Increments spawn counter
            standardCount++;

            Debug.Log(standardCount);
        }
        else if (standardCount < 21 && standardTimer <= 0)
        {
            //Generates random spawn position
            spawnPosition.y = -5;

            //Instantiates clone of enemy
            Instantiate(standardPF, spawnPosition, Quaternion.identity, gameObject.transform);

            //Resets spawn timer
            standardTimer = 0.8f;

            //Increments spawn counter
            standardCount++;

            Debug.Log(standardCount);
        }
        else if (standardCount >= 21 && standardCount < 31)
        {
            tempTimer += Time.deltaTime;

            if (tempTimer >= 4.5)
            {
                for (int enemy = 0; enemy < 5; enemy++)
                {
                    spawnPosition.y = -5f + enemy * 2.5f;
                    Instantiate(standardPF, spawnPosition, Quaternion.identity, gameObject.transform);

                    standardCount++;
                }

                Debug.Log(standardCount);

                tempTimer = 0;
            }
        }

        //Conditions for level end
        if (standardCount >= 31 && transform.childCount == 0)
        {
            //Starts interlude
            interlude = true;

            //Enables next reset
            reset = false;

            //Increments current level
            currentLevel++;

            //Activates next button
            nextButton.interactable = true;
            nextPanel.color = nextPanelColor;
            nextText.color = Color.white;

            Debug.Log("Stopped");
        }
    }
    public void Stage4()
    {
        //Confirms first reset call
        if (reset == false)
        {
            Debug.Log("Stage 4");

            //Resets counters and timers
            ResetControllers(0, 1f, 0, 0, 0, 0);

            //Prevents further reset calls in this level
            reset = true;
        }

        //Decrements timers
        denseTimer -= Time.deltaTime;

        //Spawns enemies based on count and timer
        if (denseCount < 6 && denseTimer <= 0)
        {
            //Generates random spawn position
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            //Instantiates clone of enemy
            Instantiate(densePF, spawnPosition, Quaternion.identity, gameObject.transform);

            //Resets spawn timer
            denseTimer = 3f;

            //Increments spawn counter
            denseCount++;

            Debug.Log(denseCount);
        }

        //Conditions for level end
        if (denseCount >= 6 && transform.childCount == 0)
        {
            //Starts interlude
            interlude = true;

            //Enables next reset
            reset = false;

            //Increments current level
            currentLevel++;

            //Activates next button
            nextButton.interactable = true;
            nextPanel.color = nextPanelColor;
            nextText.color = Color.white;

            Debug.Log("Stopped");
        }
    }
    public void Stage5()
    {
        //Confirms first reset call
        if (reset == false)
        {
            Debug.Log("Stage 5");

            //Resets counters and timers
            ResetControllers(0, 1f, 0, 0, 0, 0);

            //Prevents further reset calls in this level
            reset = true;
        }

        //Decrements timers
        denseTimer -= Time.deltaTime;

        //Spawns enemies based on count and timer
        if (denseCount < 15 && denseTimer <= 0)
        {
            for (int enemy = 0; enemy < 3; enemy++)
            {
                //Generates spawn position
                spawnPosition.y = -3 + enemy * 3;

                //Instantiates clone of enemy
                Instantiate(densePF, spawnPosition, Quaternion.identity, gameObject.transform);

                //Increments enemy counter
                denseCount++;
            }

            //Resets spawn timer
            denseTimer = 6f;

            Debug.Log(denseCount);
        }

        //Conditions for level end
        if (denseCount >= 15 && transform.childCount == 0)
        {
            //Starts interlude
            interlude = true;

            //Enables next reset
            reset = false;

            //Increments current level
            currentLevel++;

            //Activates next button
            nextButton.interactable = true;
            nextPanel.color = nextPanelColor;
            nextText.color = Color.white;

            Debug.Log("Stopped");
        }
    }
    public void Stage6()
    {
        //Confirms first reset call
        if (reset == false)
        {
            Debug.Log("Stage 6");

            //Resets counters and timers
            ResetControllers(4f, 1f, 0, 0, 0, 0);

            //Prevents further reset calls in this level
            reset = true;
        }

        //Decrements timers
        standardTimer -= Time.deltaTime;
        denseTimer -= Time.deltaTime;

        //Spawns enemies based on count and timer
        if (denseCount < 10 && denseTimer <= 0)
        {
            //Generates random spawn position
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            //Adds spawn positions to list
            spawnPositionList1.Add(spawnPosition);

            //Instantiates clone of enemy
            Instantiate(densePF, spawnPosition, Quaternion.identity, gameObject.transform);

            //Resets spawn timer
            denseTimer = 2.5f;

            //Increments spawn counter
            denseCount++;

            Debug.Log(denseCount);
        }

        //Spawns enemies based on count and timer
        if (standardCount < 10 && standardTimer <= 0)
        {
            //Instantiates clone of enemy
            Instantiate(standardPF, spawnPositionList1[standardCount], Quaternion.identity, gameObject.transform);

            //Resets spawn timer
            standardTimer = 2.5f;

            //Increments spawn counter
            standardCount++;

            Debug.Log(standardCount);
        }

        //Conditions for level end
        if (standardCount >= 10 &&
            denseCount >= 10 &&
            transform.childCount == 0)
        {
            //Starts interlude
            interlude = true;

            //Enables next reset
            reset = false;

            //Increments current level
            currentLevel++;

            //Activates next button
            nextButton.interactable = true;
            nextPanel.color = nextPanelColor;
            nextText.color = Color.white;

            Debug.Log("Stopped");
        }
    }
    public void Endless()
    {
        //Confirms first reset call
        if (reset == false)
        {
            Debug.Log("Endless");

            //Resets counters and timers
            ResetControllers(1f, 1f, 1f, 1f, 1f, 20f);

            //Temp timer for controlling waves
            tempTimer = 180;

            //Prevents further reset calls in this level
            reset = true;
        }

        //Decrements timers
        standardTimer -= Time.deltaTime;
        denseTimer -= Time.deltaTime;
        volatileTimer -= Time.deltaTime;
        scopedTimer -= Time.deltaTime;
        clandestineTimer -= Time.deltaTime;
        imperviousTimer -= Time.deltaTime;

        //Decrements endless timer
        if (endlessSpawnMaxTime > 0)
        {
            endlessSpawnMaxTime -= Time.deltaTime / 180;
        }
        
        if (tempTimer > 0)
        {
            tempTimer -= Time.deltaTime;
        }
        else
        {
            tempTimer = 40;

            for (int enemy = 0; enemy < 5; enemy++)
            {
                spawnPosition.y = -5f + enemy * 2.5f;
                Instantiate(imperviousPF, spawnPosition, Quaternion.identity, gameObject.transform);
            }

            for (int enemy = 0; enemy < 11; enemy++)
            {
                spawnPosition.y = -5f + enemy * 1f;
                Instantiate(standardPF, spawnPosition, Quaternion.identity, gameObject.transform);
                Instantiate(densePF, spawnPosition, Quaternion.identity, gameObject.transform);
                Instantiate(volatilePF, spawnPosition, Quaternion.identity, gameObject.transform);
                Instantiate(scopedPF, spawnPosition, Quaternion.identity, gameObject.transform);
                Instantiate(clandestinePF, spawnPosition, Quaternion.identity, gameObject.transform);
            }
        }

        if (standardTimer <= 0)
        {
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            Instantiate(standardPF, spawnPosition, Quaternion.identity, gameObject.transform);

            standardTimer = UnityEngine.Random.Range(0.25f + endlessSpawnMaxTime * 2, 0.5f + endlessSpawnMaxTime * 5);
        }

        if (denseTimer <= 0)
        {
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            Instantiate(densePF, spawnPosition, Quaternion.identity, gameObject.transform);

            denseTimer = UnityEngine.Random.Range(0.5f + endlessSpawnMaxTime * 4, 1f + endlessSpawnMaxTime * 7);
        }

        if (volatileTimer <= 0)
        {
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            Instantiate(volatilePF, spawnPosition, Quaternion.identity, gameObject.transform);

            volatileTimer = UnityEngine.Random.Range(0.25f + endlessSpawnMaxTime * 3, 0.75f + endlessSpawnMaxTime * 5);
        }

        if (scopedTimer <= 0)
        {
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            Instantiate(scopedPF, spawnPosition, Quaternion.identity, gameObject.transform);

            scopedTimer = UnityEngine.Random.Range(1.5f + endlessSpawnMaxTime * 5, 3f + endlessSpawnMaxTime * 8);
        }

        if (clandestineTimer <= 0)
        {
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            Instantiate(clandestinePF, spawnPosition, Quaternion.identity, gameObject.transform);

            clandestineTimer = UnityEngine.Random.Range(1.5f + endlessSpawnMaxTime * 4, 3f + endlessSpawnMaxTime * 7);
        }

        if (imperviousTimer <= 0)
        {
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            Instantiate(imperviousPF, spawnPosition, Quaternion.identity, gameObject.transform);

            imperviousTimer = UnityEngine.Random.Range(8f + endlessSpawnMaxTime * 30, 15f + endlessSpawnMaxTime * 60);
        }
    }


    // Use this for initialization
    void Start () {

        //Adds stages to list sequentially
        stageList.Add(Stage1);
        stageList.Add(Stage2);
        stageList.Add(Stage3);
        stageList.Add(Stage4);
        stageList.Add(Stage5);
        stageList.Add(Stage6);
        
        stageList.Add(Endless);
	}
	
	// Update is called once per frame
	void Update () {

        //Exits interlude on pressing enter
        if (interlude && Input.GetKeyDown(KeyCode.Return))
        {
            if (currentLevel < stageList.Count)
            {
                //Deactivates button
                nextButton.interactable = false;
                nextPanel.color = Color.clear;
                nextText.color = Color.clear;

                interlude = false;
            }
            else
            {
                Debug.Log("Max Level");
            }
        }

        //Runs current level
        if (interlude == false)
        {
            stageList[currentLevel]();
        }

        /*
        standardTimer -= Time.deltaTime;
        denseTimer -= Time.deltaTime;
        volatileTimer -= Time.deltaTime;
        scopedTimer -= Time.deltaTime;
        clandestineTimer -= Time.deltaTime;

        if (standardTimer <= 0)
        {
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            Instantiate(standardPF, spawnPosition, Quaternion.identity, gameObject.transform);

            standardTimer = UnityEngine.Random.Range(1f, 1.8f);
        }

        if (denseTimer <= 0)
        {
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            Instantiate(densePF, spawnPosition, Quaternion.identity, gameObject.transform);

            denseTimer = UnityEngine.Random.Range(3f, 6f);
        }
        
        if (volatileTimer <= 0)
        {
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            Instantiate(volatilePF, spawnPosition, Quaternion.identity, gameObject.transform);

            volatileTimer = UnityEngine.Random.Range(1f, 3f);
        }

        if (scopedTimer <= 0)
        {
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            Instantiate(scopedPF, spawnPosition, Quaternion.identity, gameObject.transform);

            scopedTimer = UnityEngine.Random.Range(5f, 8f);
        }

        if (clandestineTimer <= 0)
        {
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            Instantiate(clandestinePF, spawnPosition, Quaternion.identity, gameObject.transform);

            clandestineTimer = UnityEngine.Random.Range(5f, 8f);
        }

        if (imperviousTimer <= 0)
        {
            spawnPosition.y = UnityEngine.Random.Range(-5f, 5f);

            Instantiate(imperviousPF, spawnPosition, Quaternion.identity, gameObject.transform);

            imperviousTimer = UnityEngine.Random.Range(15f, 30f);
        }

        */
    }
}
