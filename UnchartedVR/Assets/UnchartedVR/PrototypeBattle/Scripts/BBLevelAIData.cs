using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBLevelAIData : MonoBehaviour 
{
	public List<Transform> aiTargetNodes;

	public void Setup()
	{
		aiTargetNodes = new List<Transform>();
		
		GameObject[] go = GameObject.FindGameObjectsWithTag("AINode");
		for	(int i = 0;  i < go.Length; i++)
		{
			aiTargetNodes.Add(go[i].transform);
		}
	}
}
