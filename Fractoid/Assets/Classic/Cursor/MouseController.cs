using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    public Texture2D cyanPointer;
    public Texture2D cyanCrosshair;

    Vector2 textureOffsetCH = new Vector2(20, 20);
    //Vector2 textureOffsetPointer = new Vector2(11, 10);

    // Use this for initialization
    void Start () {

        Cursor.SetCursor(cyanCrosshair, textureOffsetCH, CursorMode.Auto);

	}

    // Update is called once per frame
    void Update () {

	}
}
