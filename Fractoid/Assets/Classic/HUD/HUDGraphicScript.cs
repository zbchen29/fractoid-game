using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class HUDGraphicScript : MonoBehaviour {

    Vector3 activePosition = new Vector3(0, -4, 0);
    Vector3 inactivePosition = new Vector3(0, -6.5f, 0);

    Collider2D HUDCollider;
    Vector2 mouseLocation;

    public Texture2D cyanPointer;
    public Texture2D cyanCrosshair;

    Vector2 textureOffsetCH = new Vector2(20, 20);
    Vector2 textureOffsetPointer = new Vector2(11, 10);

    void OnMouseEnter ()
    {
        transform.position = activePosition;
        Cursor.SetCursor(cyanPointer, textureOffsetPointer, CursorMode.Auto);
    }

    // Use this for initialization
    void Start () {

        HUDCollider = gameObject.GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position != inactivePosition)
        {
            mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (HUDCollider.OverlapPoint(mouseLocation) == false)
            {
                transform.position = inactivePosition;
                Cursor.SetCursor(cyanCrosshair, textureOffsetCH, CursorMode.Auto);
            }
        }

        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadSceneAsync("StartScreen");
        }
	}
}
