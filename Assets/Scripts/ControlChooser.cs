using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlChooser : MonoBehaviour
{
    private static bool toggledLeft;
    private static bool toggledRight;

    
    

    public void LeftControllerMode()
    {
        toggledLeft = !toggledLeft;
        toggledRight = false;
        ControlScheme();


    }
    public void RightControllerMode()
    {
        toggledRight = !toggledRight;
        toggledLeft = false;
        ControlScheme();
    }
    void ControlScheme()
    {
        if (toggledLeft)
        {
            Debug.Log("s�tt in left controller control scheme h�r");
        }
        if (toggledRight)
        {
            Debug.Log("s�tt in right controller control scheme h�r");
            
        }
        if(!toggledRight && !toggledLeft)
        {
            Debug.Log("S�tt in normal kotroller h�r");
        }
    }
}
