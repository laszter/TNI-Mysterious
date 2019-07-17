using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Node : MonoBehaviour {

    public enum nodeLocation
    {
        FrontRoomB201, FrontOffice , Office , Other , Exit , Toilet , Storage
    }

    public enum sideLookAt
    {
        North , South , East , West
    }

    public nodeLocation location;

    public sideLookAt side;
    
    public Vector3 GetSide()
    {
        switch (side)
        {
            case sideLookAt.North:
                return new Vector3(0, 90f, 0);
            case sideLookAt.East:
                return new Vector3(0, 180f, 0);
            case sideLookAt.South:
                return new Vector3(0, 270f, 0);
            case sideLookAt.West:
                return new Vector3(0, 0, 0);
        }
        return Vector3.zero;
    }

}
