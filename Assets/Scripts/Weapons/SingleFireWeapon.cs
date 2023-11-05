using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFireWeapon : BaseWeapon
{
    //Fire a single shot when left click is pressed
    public override void StartFiring()
    {
        Fire();
    }

    public override void StopFiring()
    {

    }
}
