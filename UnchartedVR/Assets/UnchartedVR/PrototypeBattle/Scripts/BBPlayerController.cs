using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBPlayerController : BBCharacterController 
{
	public override void Logic()
	{
		#if UNITY_EDITOR
		// if (Input.GetMouseButtonDown(0))
		// {
		// 	if (Input.GetKey(KeyCode.LeftShift))
		// 	{
		// 		if (ClickAt())
		// 		{
		// 			if (shooter.FireAt(hit.point))
		// 			{
		// 				BBHealthController healthHit = hit.collider.GetComponent<BBHealthController>();
		// 				if (healthHit != null)
		// 				{
		// 					healthHit.Damage(100);
		// 				}
		// 			}
		// 		}
		// 	}
		// 	else
		// 	{
		// 		// move to spot
		// 		if (ClickAt())
		// 		{
		// 			mover.TeleportTo(hit.point);
		// 		}
		// 	}
		// }
		#endif
		
		base.Logic();
	} 

	RaycastHit hit;
	Ray ray;

	bool ClickAt()
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		return (Physics.Raycast(ray, out hit, 10000));
	}
}
