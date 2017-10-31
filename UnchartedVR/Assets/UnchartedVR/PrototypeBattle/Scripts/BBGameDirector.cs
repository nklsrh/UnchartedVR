﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBGameDirector : MonoBehaviour {

	public List<BBCharacterController> characters;
	public BBLevelAIData level;

	public BBVRController vrController;

	void Start () 
	{
		Setup();	
	}

	public void Setup()
	{
		level.Setup();
		
		foreach (BBCharacterController c in characters)
		{
			c.Setup();

			if (c.mover is BBAIMover)
			{
				(c.mover as BBAIMover).SetupAI(level);
			}
		}

		vrController.Setup();
	}
	
	void Update () 
	{
		for (int i = 0; i < characters.Count; i++)
		{
			characters[i].Logic();
		}

		vrController.Logic();
	}
}
