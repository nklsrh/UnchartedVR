using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BBAIController : BBCharacterController 
{
    public MeshRenderer[] meshDamagedIndicator;
    Material[] thisGuysMaterial;
    Color[] originalColor;

    public TextMeshPro txtHealthDamagePrototype;
    List<HealthThing> listDamageText = new List<HealthThing>();
    int currentHealthDmaage = 0;

    public Image imgHealth;
    public Image imgHealthBackground;

    public ParticleSystem particleSparks;

    public override void Setup()
    {
        base.Setup();

        health.OnDamage += Damaged;

        SetupHealthUI();
    }

    private void SetupHealthUI()
    {
        thisGuysMaterial = new Material[meshDamagedIndicator.Length];
        originalColor = new Color[meshDamagedIndicator.Length];
        for (int i = 0; i < meshDamagedIndicator.Length; i++)
        {
            thisGuysMaterial[i] = meshDamagedIndicator[i].material;
            originalColor[i] = thisGuysMaterial[i].color;
        }

        listDamageText.Clear();

        for (int i = 0; i < 4; i++)
        {
            TextMeshPro tx = Instantiate(txtHealthDamagePrototype);
            SetupText(tx);

            HealthThing ht = new HealthThing();
            ht.timeLeft = 0;
            ht.text = tx;
            listDamageText.Add(ht);
        }

        txtHealthDamagePrototype.gameObject.SetActive(false);
        imgHealthBackground.CrossFadeAlpha(0, 0.2f, false);
    }

    void SetupText(TextMeshPro tx)
    {
        tx.transform.position = transform.position + Vector3.up * 3.5f + (UnityEngine.Random.onUnitSphere * 1.5f);
    }

    private void Damaged(float amount, float healthPercent)
    {
        for (int i = 0; i < thisGuysMaterial.Length; i++)
        {
            thisGuysMaterial[i].color = Color.white;
        }

        SetupText(listDamageText[currentHealthDmaage].text);

        listDamageText[currentHealthDmaage].timeLeft = 0.25f;
        listDamageText[currentHealthDmaage].text.gameObject.SetActive(true);
        listDamageText[currentHealthDmaage].text.text = "-" + amount;
        listDamageText[currentHealthDmaage].text.transform.DOMove(new Vector3((UnityEngine.Random.value - 0.5f) * 2f, 1, 0), 0.5f).SetRelative();
        listDamageText[currentHealthDmaage].text.DOFade(0, 0.15f).SetDelay(0.25f);

        currentHealthDmaage = (currentHealthDmaage + 1) % listDamageText.Count;

        imgHealth.fillAmount = healthPercent;
        
        imgHealthBackground.CrossFadeAlpha(1, 0.25f, false);
        imgHealth.gameObject.SetActive(true);

        DOVirtual.DelayedCall(1.0f, () =>
        {
            imgHealth.gameObject.SetActive(false);
            imgHealthBackground.CrossFadeAlpha(0, 0.25f, false);
        });

        if (particleSparks != null)
        {
            particleSparks.Play();
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
        imgHealth.canvas.transform.LookAt(-Camera.main.transform.forward);
    }

	public override void Respawn()
	{
        base.Respawn();
        
        BBAIMover ai = mover as BBAIMover;

        ai.Respawn();

        SetupHealthUI();
	}
}

public class HealthThing
{
    public TextMeshPro text;
    public float timeLeft;
}