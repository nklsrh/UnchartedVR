using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBCharacterController : MonoBehaviour {

	public BBShooterController shooter;
	public BBCharacterMover mover;
	public BBHealthController health;
	
	const float RESPAWN_WAIT = 3.0f;

	protected bool isActive = false;
	float deathWait = RESPAWN_WAIT;

	public virtual void Setup () 
	{
		shooter.Setup();
		mover.Setup();
		health.Setup(Death);

		isActive = true;
	}

	public virtual void Logic()
	{
		if (isActive)
		{
			mover.Logic();
			shooter.Logic();
		}	
		else
		{
			if (!isActive)
			{
				deathWait -= Time.deltaTime;
				if (deathWait <= 0)
				{
					Respawn();
				}
			}	
		}
	}

	public virtual void Respawn()
	{
        mover.rigidbody.velocity = Vector3.zero;
        mover.rigidbody.angularVelocity = Vector3.zero;
		deathWait = RESPAWN_WAIT;
		isActive = true;
		mover.Reset();
		health.Reset();
	}

	public virtual void Death()
	{
		isActive = false;
		mover.Ragdoll(Camera.main.transform.forward + Vector3.up * 0.25f);
	}
}
