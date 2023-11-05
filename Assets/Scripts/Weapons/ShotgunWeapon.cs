using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunWeapon : BaseWeapon
{
    public int pellets = 20;
    private List<Zombie> zombiesHit;

    //New Start function required to declare list of zombies hit by one shot
    private void Start()
    {
        cameraTransform = transform.parent.transform;
        zombiesHit = new List<Zombie>();
    }

    //Single fire once left click is pressed
    public override void StartFiring()
    {
        Fire();
    }

    public override void StopFiring()
    {

    }

    //Perform several raycasts based on number of pellets, save each zombie hit, apply damage to each
    public override void Fire()
    {
        if (currentAmmo > 0)
        {
            for (int i = 0; i < pellets; ++i)
            {
                Vector3 offset = new Vector3(Random.Range(-.1f, .1f), Random.Range(-.1f, .1f), Random.Range(-.1f, .1f));
                muzzleFlash.Play();
                RaycastHit hit;
                if (Physics.Raycast(cameraTransform.position, (cameraTransform.TransformDirection(Vector3.forward) + offset) * 100, out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(transform.position, (cameraTransform.TransformDirection(Vector3.forward) + offset) * hit.distance, Color.yellow);
                    if (hit.collider.gameObject.tag == "Zombie")
                    {
                        Zombie zombie = hit.collider.gameObject.GetComponent<Zombie>();
                        if (!zombiesHit.Contains(zombie)) zombiesHit.Add(zombie);
                        Debug.Log(zombie);
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, (cameraTransform.TransformDirection(Vector3.forward) + offset) * 100, Color.white);
                }
            }
            foreach (Zombie zombie in zombiesHit) zombie.TakeDamage(damage);
            zombiesHit.Clear();
            currentAmmo--;
            if (currentAmmo <= 0) StartReload();
        }
    }
}
