using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBAIMover : BBCharacterMover 
{
	BBLevelAIData levelData;

	Vector3 target;
	bool hasTarget = false;

	public override void Setup()
	{
		base.Setup();
	}

	public void SetupAI(BBLevelAIData level)
	{
		this.levelData = level;
	}

	public override void Logic()
	{
		if (!hasTarget)
		{
			SetTarget(GetNextTarget());
		}

		if (nav.desiredVelocity.sqrMagnitude < 0.1f)
		{
			hasTarget = false;
		}

		base.Logic();
	}

	public void Respawn()
	{
		TeleportTo(GetNextTarget().position);

		//// TODO - Snap AI to closest point on the NavMesh
		// NavMeshHit hit;
		// if (NavMesh.SamplePosition(GetNextTarget().position, out hit, 1.0f, NavMesh.AllAreas)) 
		// {
		// 	TeleportTo(hit.point);
		// }
		//// nav.Move(Vector3.down * 1.0f);

		hasTarget = false;
	}

	private Transform GetNextTarget()
	{
		return levelData.aiTargetNodes[Random.Range(0, levelData.aiTargetNodes.Count - 1)];
	}

    private void SetTarget(Transform transform)
    {
		hasTarget = true;
		target = transform.position;
		MoveTo(target);
    }
}
