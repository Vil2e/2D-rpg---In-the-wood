using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleState
{
    float health;

    public float Health
    {
        get { return health; }

        set
        {
            if (value < 0) health = 0;
            health = value;
        }

    }

    //public RoleState(string json)
    //{

    //}


}
