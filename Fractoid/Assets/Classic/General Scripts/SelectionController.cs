using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {

    SelectionTracker selectedEntity;

    Vector2 mousePoint2D;

    Collider2D targetCollider;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update() {

        if (Input.GetMouseButtonDown(0))
        {
            mousePoint2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            targetCollider = Physics2D.OverlapPoint(mousePoint2D);

            if (targetCollider == null && selectedEntity != null)
            {
                selectedEntity.SelectSelfToggleOff();
            }
            else if (targetCollider != null && (targetCollider.gameObject.tag == "Ally" || targetCollider.gameObject.tag == "Player" || targetCollider.gameObject.tag == "Enemy"))
            {
                if (selectedEntity != null)
                {
                    selectedEntity.SelectSelfToggleOff();

                    selectedEntity = targetCollider.gameObject.GetComponent<SelectionTracker>();
                    selectedEntity.SelectSelfToggleOn();

                    //Debug.Log("hit successive");
                }
                else if (selectedEntity == null)
                {
                    selectedEntity = targetCollider.gameObject.GetComponent<SelectionTracker>();
                    selectedEntity.SelectSelfToggleOn();

                    //Debug.Log("hit first");
                }
            }
        }

        if (selectedEntity != null && Input.GetKeyDown("space"))
        {
            selectedEntity.SelectSelfToggleOff();
        }

	}
}
