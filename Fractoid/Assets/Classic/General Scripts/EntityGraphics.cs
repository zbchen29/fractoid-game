using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGraphics : MonoBehaviour
{

    public class GlowPulse : MonoBehaviour
    {
        //SpriteRenderer variable type
        public SpriteRenderer spriteColor;

        //Color container for alpha changes in coroutine loops
        Color currentAlpha = new Color(1f, 1f, 1f, 0.2f);

        /*
        IEnumerator GlowPulse()
        {

            if (glowGrow == true)
            {
                for (float alpha = 0.2f; alpha <= 0.5f; alpha = alpha + 0.01f)
                {
                    currentAlpha.a = alpha;
                    colorGlow.color = currentAlpha;

                    Debug.Log("Growing" + alpha);

                    yield return null;
                }

                glowGrow = false;

            }

            if (glowGrow == false)
            {
                for (float alpha = 0.5f; alpha >= 0.2f; alpha = alpha - 0.01f)
                {
                    currentAlpha.a = alpha;
                    colorGlow.color = currentAlpha;

                    Debug.Log("Shrinking" + alpha);

                    yield return null;
                }

                glowGrow = true;
            }

            StartCoroutine("GlowPulse");

        }
        */

        //Increasing opacity
        public IEnumerator GlowGrow(float min, float max)
        {
            //For loop that increases opacity every frame up to limit
            for (float alpha = min; alpha <= max; alpha += (max - min) / 30)
            {
                //Assigns alpha data to existing color
                currentAlpha.a = alpha;
                //Glow alpha updated
                spriteColor.color = currentAlpha;

                yield return null;
            }

            //Calls coroutine for decreasing opacity
            StartCoroutine(GlowShrink(min, max));
        }

        //Decreasing opacity
        public IEnumerator GlowShrink(float min, float max)
        {
            //For looop that decreases opacity every frame down to limit
            for (float alpha = max; alpha >= min; alpha -= (max - min) / 30)
            {
                //Assigns alpha data to existing color
                currentAlpha.a = alpha;
                //Glow alpha updated
                spriteColor.color = currentAlpha;

                yield return null;
            }

            //Calls coroutine for increasing opacity
            StartCoroutine(GlowGrow(min, max));
        }
    }

    //Declares object with GlowPulse type
    GlowPulse GlowEffectObject;
    SelectionTracker SelfSelect;

    //Min and max alpha values for glow pulse
    public float minGlow = 0.2f;
    public float maxGlow = 0.5f;

    Color selectedColor = new Color(1, 1, 1, 0.7f);

    //Cloacked variables
    public bool cloaked = false;
    Color cloakedColor = new Color(1, 1, 1, 0.1f);


    // Use this for initialization
    void Start()
    {
        //Initializes gameobject with GlowPulse class derived from Monobehavior
        GlowEffectObject = gameObject.AddComponent<GlowPulse>();

        //Gets Spriterenderer component for GlowEffectObject public variable
        GlowEffectObject.spriteColor = gameObject.GetComponent<SpriteRenderer>();
        //Sets initial sprite alhpa at cycle minimum
        GlowEffectObject.spriteColor.color = new Color(1f, 1f, 1f, 0.2f);

        //Begins GlowEffectObject coroutine for increasing opacity
        StartCoroutine(GlowEffectObject.GlowGrow(minGlow, maxGlow));

        //Get SelectionTracker component for parent object
        SelfSelect = transform.parent.GetComponent<SelectionTracker>();

	}
	
	// Update is called once per frame
	void LateUpdate()
    {
        //Set invisible if cloaked
        if (cloaked)
        {
            GlowEffectObject.spriteColor.color = cloakedColor;
        }
        //Set solid glow color if selected
        else if (SelfSelect.selectActive == true)
        {
            GlowEffectObject.spriteColor.color = selectedColor;
        }
	}
}
