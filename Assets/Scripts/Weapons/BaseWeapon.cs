using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public Transform cameraTransform;
    public int currentAmmo = 8;
    public int reservedAmmo = 32;
    public int maxCurrentAmmo = 8;
    public int maxReservedAmmo = 32;
    public float rateOfFire;
    public float damage = 75.0f;
    public float additionalDamage = 75.0f;
    public float reloadTime = 0.5f;
    public ParticleSystem muzzleFlash;
    public bool isReloading;

    //Save transform of camera, used for firing
    private void Start()
    {
        cameraTransform = transform.parent.transform;
    }

    //Perform a raycast that checks for zombies and removes one bullet, if zombie is hit, apply damage
    public virtual void Fire()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            muzzleFlash.Play();
            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward) * 100, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, cameraTransform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                if (hit.collider.gameObject.tag == "Zombie")
                {
                    Zombie zombie = hit.collider.gameObject.GetComponent<Zombie>();
                    zombie.TakeDamage(damage);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, cameraTransform.TransformDirection(Vector3.forward) * 100, Color.white);
            }
            if (currentAmmo <= 0) StartReload();
        }
    }

    //Initiates when player presses left click
    public abstract void StartFiring();

    //Initiates when player releases left click
    public abstract void StopFiring();

    //Starts reloading, after a delay, reload gun
    public void StartReload()
    {
        if (currentAmmo != maxCurrentAmmo && reservedAmmo > 0)
        {
            isReloading = true;
            Invoke("Reload", reloadTime);
        }
    }

    //Take from reserve ammo and fill up current ammo
    void Reload()
    {
        reservedAmmo = reservedAmmo - (maxCurrentAmmo - currentAmmo);
        currentAmmo = maxCurrentAmmo;
        if (reservedAmmo < 0)
        {
            currentAmmo += reservedAmmo;
            reservedAmmo = 0;
        }
        isReloading = false;
    }
}
