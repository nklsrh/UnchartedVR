using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileEnemy : MonoBehaviour
{
    Rigidbody rig;

    //Vector3 velocity;
    //Vector3 acceleration;
    //public float gravityAffect = 8.91f;
    //public float accelerationDrag = 0.98f;
    //public float velocityDrag = 0.99f;
    public float fireSpeed = 10;
    public float pitchUp = 0.5f;


    float timeAlive = 0;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

	void Update ()
    {
        //velocity *= velocityDrag;

        //velocity += acceleration;
        //acceleration -= Vector3.up * gravityAffect;

        //acceleration *= accelerationDrag;

        //transform.position += velocity;
        timeAlive += Time.deltaTime;
        if (timeAlive > 5.0f)
        {
            Destroy(this.gameObject);
        }
	}

    void OnCollisionEnter(Collision other)
    {
        BBPlayerController p = other.gameObject.GetComponent<BBPlayerController>();
        if (p != null)
        {
            Debug.Log("PLAYERHIT!");
        }
    }

    internal void FireAt(Vector3 position)
    {
        //velocity = Vector3.zero;
        //acceleration = Vector3.zero;

        Vector3 delta = position - transform.position;

        //Vector3 direction = (Vector3.forward + Vector3.up).normalized;
        ////direction = Vector3.RotateTowards(direction, position, 1000, 10000);


        //acceleration = direction * fireSpeed;
        if (rig == null)
        {
            rig = GetComponent<Rigidbody>();
        }

        rig.AddForce(delta + Vector3.up * (delta.sqrMagnitude * pitchUp) * fireSpeed, ForceMode.Impulse);
    }
}
