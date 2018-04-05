using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class HUDScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    //RectTransform of production panel
    RectTransform panelPosition;

    public bool active = false;
    public bool cursorReset;

    //Preset positions of panel
    Vector2 activePosition = new Vector2(0, 0);
    Vector2 inactivePosition = new Vector2(0, -260);
    
    //Linked cursor textures
    public Texture2D cyanPointer;
    public Texture2D cyanCrosshair;

    //Cursor texture offsets
    Vector2 textureOffsetPointer = new Vector2(11, 10);
    Vector2 textureOffsetCH = new Vector2(20, 20);

    //Lerp timer
    float lerpTimer;

    //Called when cursor enters panel
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Moves panel to active position
        //panelPosition.anchoredPosition = activePosition;

        active = true;
        cursorReset = true;

        //Switches to pointer
        Cursor.SetCursor(cyanPointer, textureOffsetPointer, CursorMode.Auto);
    }
    
    //Called when cursor exits panel
    public void OnPointerExit(PointerEventData eventData)
    {
        //Moves panel to inactive position
        //panelPosition.anchoredPosition = inactivePosition;

        active = false;
        cursorReset = true;

        Cursor.SetCursor(cyanCrosshair, textureOffsetCH, CursorMode.Auto);
    }

	// Use this for initialization
	void Start () {

        //Reference to panel RectTransform
        panelPosition = gameObject.GetComponent<RectTransform>();

        //Adjusts position shift amount based on screen resolution; Used with Constant Pixel Size for Canvas UI Scale Mode
        //inactivePosition.y = -260 * Screen.height / gameObject.GetComponentInParent<CanvasScaler>().referenceResolution.y;

        //Sets panel to initial inactive position
        panelPosition.anchoredPosition = inactivePosition;
	}
	
	// Update is called once per frame
	void Update () {
		
        //Exits to start screen
        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadSceneAsync("StartScreen");
        }

        //Conditional controlling panel movement
        if (active && lerpTimer < 1)
        {
            //Rate of activation
            lerpTimer += Time.deltaTime * 5;

            //Interpolates position based on timer
            panelPosition.anchoredPosition = Vector2.Lerp(inactivePosition, activePosition, lerpTimer);
        }
        else if (active == false && lerpTimer > 0)
        {
            //Rate of deactivation
            lerpTimer -= Time.deltaTime * 1;

            //Interpolates position based on timer
            panelPosition.anchoredPosition = Vector2.Lerp(inactivePosition, activePosition, lerpTimer);
        }
	}
}
