using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBHandController : MonoBehaviour {
    public Transform weaponSlot;
    public List<BBWeapon> weapons;

    private BBWeapon currentWeapon;

    internal void Setup()
    {
        currentWeapon = weapons[weapons.Count - 1];
        NextWeapon();
    }

    public void SetWeapon(BBWeapon weapon)
    {
        foreach (BBWeapon w in weapons)
        {
            w.gameObject.SetActive(w == weapon);
        }
        currentWeapon = weapon;
    }

    internal void NextWeapon()
    {
        SetWeapon(weapons[(weapons.IndexOf(currentWeapon) + 1) % weapons.Count]);
    }

    public void Fire()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Fire();
        }
    }

    internal void AimAt(Vector3 position)
    {
        transform.LookAt(position);
    }
}
