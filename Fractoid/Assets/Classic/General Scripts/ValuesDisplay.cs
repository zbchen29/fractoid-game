using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValuesDisplay : MonoBehaviour {

    //Dictates display information; 1-Currency, 2-Points
    public int displayInfo;

    //Variables used for detecting change in value
    int currencyCached;
    int pointsCached;

    Text textDisplay;

    //Coroutine variables
    bool flashRunning;
    Coroutine flashCoroutine;

    //IEnumerator for flash; duration in frames
    IEnumerator ColorFlashRecovery(Color normal, Color flash, float duration)
    {
        for (int i = 0; i <= duration; i++)
        {
            textDisplay.color = Color.Lerp(flash, normal, i / duration);
            Debug.Log(i / duration);
            yield return null;
        }

        flashRunning = false;
    }

    public void RedFlash()
    {
        if (displayInfo == 1)
        {
            if (flashRunning)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = StartCoroutine(ColorFlashRecovery(Color.green, Color.red, 60f));
            flashRunning = true;
        }
    }

	// Use this for initialization
	void Start () {

        textDisplay = gameObject.GetComponent<Text>();

        currencyCached = ValueManager.currency;
        pointsCached = ValueManager.points;

        if(displayInfo == 1)
        {
            textDisplay.text = currencyCached.ToString();
        }

        if (displayInfo == 2)
        {
            textDisplay.text = pointsCached.ToString();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
        if (displayInfo == 1 && currencyCached != ValueManager.currency)
        {
            currencyCached = ValueManager.currency;
            textDisplay.text = currencyCached.ToString();
        }
        else if (displayInfo == 2 && pointsCached != ValueManager.points)
        {
            pointsCached = ValueManager.points;
            textDisplay.text = pointsCached.ToString();
        }
	}
}
