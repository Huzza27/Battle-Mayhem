using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DirectionData : MonoBehaviour
{
    bool isRight;
    Movement movement;

    private void Update()
    {
        isRight = movement.facingRight;
    }

    public bool IsRight()
    {
        return isRight;
    }


}
