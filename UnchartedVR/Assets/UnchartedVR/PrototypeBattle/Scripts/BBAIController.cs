using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBAIController : BBCharacterController 
{

    public override void Logic()
    {
        base.Logic();
    }

	public override void Respawn()
	{
        base.Respawn();
        
        BBAIMover ai = mover as BBAIMover;

        ai.Respawn();
	}
}
