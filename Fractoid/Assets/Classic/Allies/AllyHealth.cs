using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyHealth : MonoBehaviour {

    public int health;

    //Hit graphics valriables
    public Color hitColor = new Color(0.0f, 0.0f, 0.0f);
    //Color normalColor = new Color(1f, 1f, 1f);        //Used in instant color recovery
    float normalizedColorValue;                             //Used in gradual color recovery

    float hitTimer;
    public float tintTime = 0.2f;     //Seconds tinted

    //Controls timer decrement after hit; not needed in gradual color recovery approach
    //bool hitReset;

    //SpriteRenderer object
    public SpriteRenderer SpriteRendererObject;
    public SpriteRenderer SprialSpriteRendererObject;

    public void HitOccur(int damage, Color hitTint, float tintDuration)
    {
        health -= damage;
        SpriteRendererObject.color = hitTint;
        SprialSpriteRendererObject.color = hitTint;

        hitTimer = tintDuration;

        //Used in instant color recovery
        //hitReset = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "StandardBullet")
        {
            HitOccur(5, hitColor, tintTime);
        }

        if (other.gameObject.tag == "ScopedBullet")
        {
            HitOccur(10, hitColor, tintTime);
        }

        if (other.gameObject.tag == "ClandestineBullet")
        {
            HitOccur(10, hitColor, tintTime);
        }
    }

    // Use this for initialization
    void Start()
    {
        SpriteRendererObject = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
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
            SprialSpriteRendererObject.color = new Color(normalizedColorValue, normalizedColorValue, normalizedColorValue);
        }

    }
}
