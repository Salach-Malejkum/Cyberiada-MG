using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckPointManager : MonoBehaviour
{
    private GameObject checkPointInRange;

    public void OnTalk(InputAction.CallbackContext inputAction)
    {
        if(checkPointInRange != null)
        {
            checkPointInRange.GetComponent<CheckPoint>().Interact();
        }
    }

    public void setCheckPointInRage(GameObject checkPoint)
    {
        checkPointInRange = checkPoint;
    }
    public void removeCheckPointInRage()
    {
        checkPointInRange = null;
    }
}
