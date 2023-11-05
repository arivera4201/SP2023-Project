using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullAutoWeapon : BaseWeapon
{
    private bool isFiring;
    
    //Begin the firing loop for fully automatic firing
    public override void StartFiring()
    {
        isFiring = true;
        FiringLoop();
    }

    //Continuously fire as long as player is holding left click
    void FiringLoop()
    {
        if (isFiring)
        {
            Fire();
            Invoke("FiringLoop", rateOfFire);
        }
    }

    //Stop firing once left click is released
    public override void StopFiring()
    {
        isFiring = false;
    }
}
