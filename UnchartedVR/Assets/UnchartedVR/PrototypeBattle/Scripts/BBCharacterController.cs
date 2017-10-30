using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBCharacterController : MonoBehaviour {

	public BBShooterController shooter;
	public BBCharacterMover mover;
	public BBHealthController health;
	
	public virtual void Setup () 
	{
		// shooter.Setup();
		mover.Setup();
		// health.Setup();
	}

	public virtual void Logic()
	{
		mover.Logic();
		// shooter.Logic();
		// health.Logic();
	}
}
