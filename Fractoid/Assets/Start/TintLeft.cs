using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TintLeft : MonoBehaviour
{

    SpriteRenderer colorInstance;
    Color tint = new Color(1, 1, 1, 1);
    Color clear = new Color(0, 0, 0, 0);

    Vector2 textureOffsetCH = new Vector2(20, 20);
    Vector2 textureOffsetPointer = new Vector2(11, 10);

    public Texture2D cyanPointer;
    public Texture2D cyanCrosshair;

    // Use this for initialization
    void Start()
    {

        colorInstance = GetComponent<SpriteRenderer>();
        colorInstance.color = clear;

    }

    void OnMouseEnter()
    {

        colorInstance.color = tint;
        Cursor.SetCursor(cyanPointer, textureOffsetPointer, CursorMode.Auto);

    }

    void OnMouseExit()
    {

        colorInstance.color = clear;
        Cursor.SetCursor(cyanCrosshair, textureOffsetCH, CursorMode.Auto);

    }

    void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Next Level");
            SceneManager.LoadSceneAsync("Classic");
        }

    }

    // Update is called once per frame
    void Update()
    {
		
	}
}
