using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMovement : MonoBehaviour {

    /*
    public class Selection : MonoBehaviour
    {
        public bool selectActive = false;

        public void SelectSelfToggle()
        {
            if (selectActive == true)
            {
                selectActive = false;
            }
            else if (selectActive == false)
            {
                selectActive = true;
            }
        }
    }
    */

    public SelectionTracker SelfSelect;

    Rigidbody2D allyRB;

    bool motion = false;

    Vector2 movementTarget;
    Vector2 clickedLocation;
    Vector2 direction;

    public float speed;

    Collider2D clickedCollider;

    // Use this for initialization
    void Awake()
    {
        SelfSelect = gameObject.GetComponent<SelectionTracker>();

        allyRB = gameObject.GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D()
    {
        allyRB.velocity = Vector2.zero;
        motion = false;
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetMouseButtonDown(1) && SelfSelect.selectActive == true)
        {
            clickedLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            clickedCollider = Physics2D.OverlapPoint(clickedLocation);

            if (clickedCollider == null)
            {
                movementTarget = clickedLocation;

                if (Mathf.Pow(transform.position.x - movementTarget.x, 2) + Mathf.Pow(transform.position.y - movementTarget.y, 2) >= 0.1)
                {
                    direction.x = movementTarget.x - transform.position.x;
                    direction.y = movementTarget.y - transform.position.y;

                    //allyRB.AddForce(direction / direction.magnitude * speed);
                    motion = true;
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (motion == true)
        {
            allyRB.AddForce(direction / direction.magnitude * speed);

            if (Mathf.Pow(transform.position.x - movementTarget.x, 2) + Mathf.Pow(transform.position.y - movementTarget.y, 2) < 0.1)
            {
                allyRB.velocity = Vector2.zero;
                motion = false;
            }
        }
    }
}
