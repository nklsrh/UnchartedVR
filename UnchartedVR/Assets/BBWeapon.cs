using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBWeapon : MonoBehaviour
{
    public LineRenderer lineLaserPointer;

    public float timeBetweenShots = 0.25f;
    public Vector3 kickback = Vector3.back * 0.05f;
    public float kickbackLerpBack = 5f;

    public float damagePerHit = 10.0f;

    public ParticleSystem muzzleFlash;
    public ParticleSystem hitSparks;

    public float physicsForce = 1000;

    float lastTimeShot = -1000;
    
	void Update ()
    {
		if (lineLaserPointer)
        {
            lineLaserPointer.SetPositions(new Vector3[] { lineLaserPointer.transform.position, lineLaserPointer.transform.position + this.transform.forward * 100 });
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, kickbackLerpBack * Time.deltaTime);
	}

    internal void Fire()
    {
        if (Time.time - lastTimeShot > timeBetweenShots)
        {
            transform.localPosition += kickback;
            lastTimeShot = Time.time;

            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                BBHealthController healtu = hit.collider.gameObject.GetComponent<BBHealthController>();
                if (healtu != null)
                {
                    healtu.Damage(damagePerHit);
                }

                Rigidbody rig = hit.collider.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    rig.AddForceAtPosition(-hit.normal * physicsForce, hit.point, ForceMode.Impulse);
                }
                SuccessfulHit(hit.point);
            }

        }
    }

    void SuccessfulHit(Vector3 hitPos)
    {
        if (hitSparks)
        {
            hitSparks.transform.parent = null;
            hitSparks.transform.position = hitPos;
            hitSparks.transform.localScale = Vector3.one;
            hitSparks.transform.forward = -transform.forward;
            hitSparks.Play();
        }
    }
}
