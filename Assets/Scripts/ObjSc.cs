using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSc : MonoBehaviour
{
    public int objId = 0;

    [NonSerialized]
    public bool isSelected = false;
    private GameManager gameManager;
    private Color defColor;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        defColor = GetComponent<SpriteRenderer>().color;
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
        if(gameManager.clickedObjId == objId && !isSelected)
        {
            gameManager.OnObjSelected(gameObject);
        }
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
