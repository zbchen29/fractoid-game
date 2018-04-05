using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionTracker : MonoBehaviour {

    public bool selectActive = false;

    public void SelectSelfToggleOn()
    {
        selectActive = true;
    }

    public void SelectSelfToggleOff()
    {
        selectActive = false;
    }
}
