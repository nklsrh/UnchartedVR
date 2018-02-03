using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBAIController : BBCharacterController 
{
    public MeshRenderer[] meshDamagedIndicator;
    Material[] thisGuysMaterial;
    Color[] originalColor;

    public override void Setup()
    {
        base.Setup();

        health.OnDamage += Damaged;

        thisGuysMaterial = new Material[meshDamagedIndicator.Length];
        originalColor = new Color[meshDamagedIndicator.Length];
        for (int i = 0; i < meshDamagedIndicator.Length; i++)
        {
            thisGuysMaterial[i] = meshDamagedIndicator[i].material;
            originalColor[i] = thisGuysMaterial[i].color;
        }
    }

    private void Damaged()
    {
        for (int i = 0; i < thisGuysMaterial.Length; i++)
        {
            thisGuysMaterial[i].color = Color.white;
        }
    }

    public override void Logic()
    {
        base.Logic();

        for (int i = 0; i < thisGuysMaterial.Length; i++)
        {
            if (thisGuysMaterial[i] != null)
            {
                thisGuysMaterial[i].color = Color.Lerp(thisGuysMaterial[i].color, originalColor[i], Time.deltaTime);
            }
        }
    }

	public override void Respawn()
	{
        base.Respawn();
        
        BBAIMover ai = mover as BBAIMover;

        ai.Respawn();
	}
}
