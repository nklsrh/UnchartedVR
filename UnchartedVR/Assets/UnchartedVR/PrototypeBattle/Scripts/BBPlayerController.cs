using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBPlayerController : BBCharacterController 
{
	public override void Logic()
	{

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 10000))
			{
				mover.MoveTo(hit.point);
			}
		}
		
		base.Logic();
	} 
}
