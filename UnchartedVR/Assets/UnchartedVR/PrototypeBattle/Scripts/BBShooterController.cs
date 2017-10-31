using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBShooterController : MonoBehaviour 
{
    int ammoMax = 12;
    int ammoCurrent;


    public float timeBetweenShots = 0.2f;

    public float ammoRegenCooldown = 0.5f; // how long to wait after firing before regen

    private float currentTimeBetweenShots = 0;
    private float currentTimeSinceFiring = 0;


    public void Setup()
    {
        ammoCurrent = ammoMax;
    }

    public bool FireAt(Vector3 targetPoint)
    {
        bool couldFire = false;

        if (HasAmmo() && CanFireRate())
        {
            couldFire = true;

            ammoCurrent--;
            currentTimeBetweenShots = timeBetweenShots;
        }

        currentTimeSinceFiring = 0;
        return couldFire;
    }

    public bool HasAmmo()
    {
        return ammoCurrent >= 1;
    }

    public void Logic()
    {
        if (currentTimeBetweenShots > 0)
        {
            currentTimeBetweenShots -= Time.deltaTime;
        }
        if (currentTimeSinceFiring < ammoRegenCooldown)
        {
            currentTimeSinceFiring += Time.deltaTime;
        }
        RegenLogic();
    }

    void RegenLogic()
    {
        if (currentTimeSinceFiring >= ammoRegenCooldown && ammoCurrent < ammoMax)
        {
            ammoCurrent = ammoMax;
        }
    }

    public bool CanFireRate()
    {
        return currentTimeBetweenShots <= 0;
    }
}
