using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ObjSc : MonoBehaviour
{
    public int objId = 0;

    [NonSerialized]
    public bool isSelected = false;
    private GameManager gameManager;
    private Color defColor;
    private Text tx;

    void Awake()
    {
        tx = transform.Find("Canvas").Find("Text").GetComponent<Text>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetObj();
    }
    private void FixedUpdate()
    { 
        GetComponent<BoxCollider2D>().layerOverridePriority = -(int)transform.position.y * 5; 
    }
    private void OnMouseDown()
    {
        if(!isSelected)
        {
            gameManager.OnObjClicked(gameObject);
        }
    }

    private void OnMouseOver()
    {
        if(gameManager.clickedObjId == objId && !isSelected && gameManager.CheckSelectable(transform))
        {
            gameManager.OnObjSelected(gameObject);
        }
    }

    public void SetObj()
    {
        int a = UnityEngine.Random.Range(0, 3);
        tx.text = ((char)('A' + a)).ToString();
        switch (tx.text)
        {
            case "A":
                objId = 1;
                GetComponent<SpriteRenderer>().color = gameManager.colorA;
                break;
            case "B":
                objId = 2;
                GetComponent<SpriteRenderer>().color = gameManager.colorB;
                break;
            case "C":
                objId = 3;
                GetComponent<SpriteRenderer>().color = gameManager.colorC;
                break;
        }
        defColor = GetComponent<SpriteRenderer>().color;
    }

    public void SetCondition(int condition)
    {
        switch (condition)
        {
            case 0: // Default state
                isSelected = false;
                GetComponent<SpriteRenderer>().color = defColor;
                break;
            case 1: // Clicked state
                isSelected = true;
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 2: // Selected state
                isSelected = true;
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
        }
    }
}
