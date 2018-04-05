using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    //Health
    public int health;

    //Currency and point values
    public int pointValue;
    public int currencyValue;

    //Player hit bool for determining point multiplier
    bool playerHit;

    //Hit graphics valriables
    Color hitColor = new Color(0.0f, 0.0f, 0.0f);
    //Color normalColor = new Color(1f, 1f, 1f);        //Used in instant color recovery
    float normalizedColorValue;                             //Used in gradual color recovery
    
    float hitTimer;
    float tintTime = 0.2f;     //Seconds tinted

    //Controls timer decrement after hit; not needed in gradual color recovery approach
    //bool hitReset;

    //SpriteRenderer object
    public SpriteRenderer SpriteRendererObject;

    void HitOccur(int damage, bool playerAttack, Color hitTint, float tintDuration)
    {
        health -= damage;
        playerHit = playerAttack;
        SpriteRendererObject.color = hitTint;

        hitTimer = tintDuration;

        //Used in instant color recovery
        //hitReset = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "LoyalBullet")
        {
            HitOccur(10, false, hitColor, tintTime);
        }

        if (other.gameObject.tag == "ResoluteBullet")
        {
            HitOccur(5, false, hitColor, tintTime);
        }

        if (other.gameObject.tag == "PlayerBullet")
        {
            HitOccur(5, true, hitColor, tintTime);
        }
    }

	// Use this for initialization
	void Start () {
        SpriteRendererObject = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if (health <= 0)
        {
            ValueManager.IncrementValues(pointValue, currencyValue, playerHit);
            Destroy(gameObject);
        }

        //Instant color recovery after duration following hit
        /*
        if (hitReset)
        {
            hitTimer -= Time.deltaTime;

            if (hitTimer < 0)
            {
                SpriteRendererObject.color = normalColor;
                Debug.Log("Clear");
                hitReset = false;
            }

        }
        */

        //Gradual color recovery over duration following hit
        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;

            normalizedColorValue = (1 / tintTime) * (tintTime - hitTimer);
            SpriteRendererObject.color = new Color(normalizedColorValue, normalizedColorValue, normalizedColorValue);
        }
    }
}
