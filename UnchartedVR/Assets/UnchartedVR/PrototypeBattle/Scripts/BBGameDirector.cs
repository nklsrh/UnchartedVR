using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBGameDirector : MonoBehaviour {

	public List<BBPlayerController> ;
	
	// Use this for initialization
	void Start () 
	{
		Setup();	
	}

	public void Setup()
	{
		player.Setup();
	}
	
	// Update is called once per frame
	void Update () 
	{
		player.Logic();	
	}
}
