using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviour {

    public static int currency;

    public static int points;

    static float pointMultiplierTimer;

    static int pointMultiplier = 1;

    public static void IncrementValues(int pointValue, int currencyValue, bool multiplierEffect)
    {
        if (multiplierEffect)
        {
            points += pointValue * pointMultiplier;
            currency += currencyValue;

            pointMultiplier += 1;
            pointMultiplierTimer = 3f;
        }
        else
        {
            points += pointValue;
            currency += currencyValue;
        }
    }

    public static void Purchase(int costValue, int spawnType, string objectTag = null, Transform parent = null)
    {
        switch (spawnType)
        {
            case 1:
                int count = 0;

                foreach (Transform child in parent)
                {
                    if (child.gameObject.name.Contains(objectTag))
                    {
                        count++;
                    }
                }

                currency -= (costValue * (int)Mathf.Pow(2, count));
                break;

            case 2:
                currency -= costValue;
                break;

            case 3:
                currency -= costValue;
                break;
        }
    }

    public static bool SufficientCurrency(int costValue, int spawnType, string objectTag = null, Transform parent = null)
    {
        int count= 0;

        switch (spawnType)
        {
            case 1:
                foreach (Transform child in parent)
                {
                    if (child.gameObject.name.Contains(objectTag))
                    {
                        count++;
                    }
                }
                return ValueManager.currency >= (costValue * Mathf.Pow(2, count));

            case 2:
                return ValueManager.currency >= costValue;

            case 3:
                return ValueManager.currency >= costValue;

            default:
                return false;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (pointMultiplierTimer > 0)
        {
            pointMultiplierTimer -= Time.deltaTime;
        }
        else
        {
            pointMultiplier = 1;
        }
	}
}
