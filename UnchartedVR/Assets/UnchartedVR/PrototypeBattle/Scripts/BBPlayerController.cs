using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBPlayerController : BBCharacterController 
{
    public BBHandController hand;

    void Start()
    {
        hand.Setup();
    }

    public void PressActionButton()
    {
        NextWeapon();
    }

    private void NextWeapon()
    {
        if (hand != null)
        {
            hand.NextWeapon();
        }
    }

    public void PressTriggerButton()
    {
        FireCurrentWeapon();
    }

    public void FireCurrentWeapon()
    {
        if (hand != null)
        {
            hand.Fire();
        }
    }

    internal void AimAt(Vector3 position)
    {
        if (hand != null)
        {
            hand.AimAt(position);
        }
    }
}
