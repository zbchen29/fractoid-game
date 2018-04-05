using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionScript : MonoBehaviour {

    //Parent controllers for spawns
    Transform spawnParent;
    public Transform allyController;

    //Spawn prefabs
    public Transform loyalPF;
    public Transform resolutePF;

    //Variables controlling spawning behaviors
    public bool spawnReady;         //Bool indicating active spawn selection
    Vector3 spawnLocation;          //Vector3 for spawn location
    Transform currentSpawn;         //Container for current spawn transform
    Collider2D clickedCollider;     //Collider container for ensuring unoccupied spawn location
    int spawnType;                  //Indicates spawn type:  1-Ally, 2-Wall, 3-Trap
    string spawnName;               //Tags for spawns used in SetSpawn()

    //Cost variable
    int spawnCost;

    //Costs
    public int loyalCost;
    public int resoluteCost;

    //Values display object for graphical effects during insuffient funds
    public ValuesDisplay ValuesDisplayObject;

    //Production objects
    public Transform DecalTransformObject;
    public SpriteRenderer DecalSpriteRendererObject;
    public HUDScript HUDScriptObject;

    //Production sprites
    public Sprite currentSprite;
    public Sprite loyalSprite;
    public Sprite resoluteSprite;

    //Transparency of decal sprite
    Color clearColor = new Color(1, 1, 1, 0);
    Color normalColor = new Color(1, 1, 1, 1f);

    //Mouse position for sprite
    Vector2 mouseLocation;

    public void SetSpawn(Transform spawnPrefab, Sprite spawnSprite, int spawnTypeArg, int spawnCostArg, string spawnNameArg, Transform spawnParentArg)
    {
        if (!spawnReady)
        {
            currentSpawn = spawnPrefab;
            spawnReady = true;

            //Sets sprite and offset
            currentSprite = spawnSprite;

            HUDScriptObject.cursorReset = true;

            spawnType = spawnTypeArg;
            spawnCost = spawnCostArg;
            spawnName = spawnNameArg;
            spawnParent = spawnParentArg;
        }
        else if (spawnReady == true && currentSpawn != loyalPF)
        {
            currentSpawn = spawnPrefab;

            //Sets sprite and offset
            currentSprite = spawnSprite;

            HUDScriptObject.cursorReset = true;

            spawnType = spawnTypeArg;
            spawnCost = spawnCostArg;
            spawnName = spawnNameArg;
            spawnParent = spawnParentArg;
        }
        else
        {
            ResetSpawn();
        }
    }

    void ResetSpawn()
    {
        spawnReady = false;
        currentSpawn = null;

        //Clears sprite
        currentSprite = null;

        HUDScriptObject.cursorReset = true;

        spawnType = 0;
        spawnCost = 0;
        spawnName = null;
        spawnParent = null;
    }

    public void SetLoyal()
    {
        SetSpawn(loyalPF, loyalSprite, 1, loyalCost, "LoyalPF", allyController);
        /*
        if (spawnReady == false)
        {
            currentSpawn = loyalPF;
            spawnReady = true;

            //Sets sprite and offset
            currentSprite = loyalSprite;
            //currentOffset = mediumOffset;

            HUDScriptObject.cursorReset = true;

            spawnType = 1;
            spawnCost = loyalCost;
        }
        else if (spawnReady == true && currentSpawn != loyalPF)
        {
            currentSpawn = loyalPF;

            //Sets sprite and offset
            currentSprite = loyalSprite;
            //currentOffset = mediumOffset;

            HUDScriptObject.cursorReset = true;

            spawnType = 1;
            spawnCost = loyalCost;
        }
        else
        {
            spawnReady = false;
            currentSpawn = null;

            //Clears sprite
            currentSprite = null;

            HUDScriptObject.cursorReset = true;

            spawnType = 0;
            spawnCost = 0;
        }
        */
    }

    public void SetResolute()
    {
        SetSpawn(resolutePF, resoluteSprite, 1, resoluteCost, "ResolutePF", allyController);

        /*
        if (spawnReady == false)
        {
            currentSpawn = resolutePF;
            spawnReady = true;

            //Sets sprite
            currentSprite = resoluteSprite;

            HUDScriptObject.cursorReset = true;

            spawnType = 1;
            spawnCost = resoluteCost;
        }
        else if (spawnReady == true && currentSpawn != resolutePF)
        {
            currentSpawn = resolutePF;

            //Sets sprite
            currentSprite = resoluteSprite;

            HUDScriptObject.cursorReset = true;

            spawnType = 1;
            spawnCost = resoluteCost;
        }
        else
        {
            spawnReady = false;
            currentSpawn = null;

            //Clears sprite
            currentSprite = null;

            HUDScriptObject.cursorReset = true;

            spawnType = 0;
            spawnCost = 0;
        }
        */
    }

	// Use this for initialization
	void Start () {

        //Gets SpriteRenderer in child; already directly linked
        DecalSpriteRendererObject = gameObject.GetComponentInChildren<SpriteRenderer>();

	}

    // Update is called once per frame
    void Update() {

        if (Input.GetMouseButtonDown(0) && spawnReady)
        {
            if(ValueManager.SufficientCurrency(spawnCost, spawnType, spawnName, spawnParent))
            {
                spawnLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spawnLocation.z = 0;

                clickedCollider = Physics2D.OverlapPoint(spawnLocation);

                if (clickedCollider == null && !HUDScriptObject.active)
                {
                    ValueManager.Purchase(spawnCost, spawnType, spawnName, spawnParent);

                    Instantiate(currentSpawn, spawnLocation, Quaternion.identity, spawnParent);

                    /*
                    if (spawnType == 1)
                    {
                        Instantiate(currentSpawn, spawnLocation, Quaternion.identity, allyController);
                    }
                    /*
                    else if (spawnType == 2)
                    {
                        Instantiate(currentSpawn, spawnLocation, Quaternion.identity, wallController);
                    }
                    else if (spawnType == 3)
                    {
                        Instantiate(currentSpawn, spawnLocation, Quaternion.identity, trapController);
                    }
                    */

                    ResetSpawn();
                }
            }
            else
            {
                Debug.Log("Too expensive");
                ValuesDisplayObject.RedFlash();
            }
        }

        if (Input.GetKeyDown("space"))
        {
            ResetSpawn();
        }
    }

    void LateUpdate()
    {
        //Runs when events affecting cursor appearance occur
        if (HUDScriptObject.cursorReset)
        {
            //Runs when in build mode
            if (spawnReady)
            {
                //Turns cursor off if pointing at game field; turns cursor on if pointing at HUD
                Cursor.visible = HUDScriptObject.active;

                //Decal visibility
                if (HUDScriptObject.active)
                {
                    DecalSpriteRendererObject.color = clearColor;
                }
                else
                {
                    DecalSpriteRendererObject.color = normalColor;
                }
            }
            else
            {
                //Turns cursor when not in build mode
                Cursor.visible = true;
            }

            //Updates decal sprite
            DecalSpriteRendererObject.sprite = currentSprite;

            //Prevents continuous calls
            HUDScriptObject.cursorReset = false;
        }

        //Decal follows mouse position
        if (spawnReady)
        {
            mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            DecalTransformObject.position = mouseLocation;
        }
    }
}
