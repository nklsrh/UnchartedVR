using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BBAIController : BBCharacterController 
{
    public MeshRenderer[] meshDamagedIndicator;
    Material[] thisGuysMaterial;
    Color[] originalColor;

    public TextMeshPro txtHealthDamagePrototype;
    List<HealthThing> listDamageText = new List<HealthThing>();
    int currentHealthDmaage = 0;

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

        for (int i = 0; i < 4; i++)
        {
            TextMeshPro tx = Instantiate(txtHealthDamagePrototype);
            tx.transform.position = transform.position + Vector3.up * 3.5f + (UnityEngine.Random.onUnitSphere * 1.5f);

            HealthThing ht = new HealthThing();
            ht.timeLeft = 0;
            ht.text = tx;
            listDamageText.Add(ht);
        }
    }

    private void Damaged(float amount)
    {
        for (int i = 0; i < thisGuysMaterial.Length; i++)
        {
            thisGuysMaterial[i].color = Color.white;
        }

        listDamageText[currentHealthDmaage].timeLeft = 0.25f;
        listDamageText[currentHealthDmaage].text.gameObject.SetActive(true);
        listDamageText[currentHealthDmaage].text.text = "-" + amount;

        currentHealthDmaage = (currentHealthDmaage + 1) % listDamageText.Count;
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

        for (int i = 0; i < listDamageText.Count; i++)
        {
            if (listDamageText[i].text.gameObject.activeSelf)
            {
                if (listDamageText[i].timeLeft <= 0)
                {
                    listDamageText[i].text.gameObject.SetActive(false);
                }
                else
                {
                    listDamageText[i].timeLeft -= Time.deltaTime;
                }
                listDamageText[i].text.transform.forward = Camera.main.transform.forward;
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

public class HealthThing
{
    public TextMeshPro text;
    public float timeLeft;
}