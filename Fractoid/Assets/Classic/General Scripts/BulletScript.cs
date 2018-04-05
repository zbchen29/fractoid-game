using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    int damageAmount;
    bool damageBuff;

    public void SetBullet (Vector3 aim, int speed, string tag, int dmg, float decay, bool buff)
    {
        gameObject.tag = tag;

        damageAmount = dmg;

        bulletRB.velocity = aim * speed;

        if (buff)
        {
            damageAmount *= 2;
        }

        Destroy(gameObject, decay);
    }

    Rigidbody2D bulletRB;

    void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.GetComponent<Animator>().SetInteger("BreakType", Random.Range(1, 4));

        Destroy(gameObject.GetComponent<CircleCollider2D>());

        bulletRB.velocity = Vector2.zero;

        Destroy(gameObject, 0.4f);
    }

    // Use this for initialization
    void Awake () {

        bulletRB = gameObject.GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {

	}
}
