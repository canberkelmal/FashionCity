using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ObjSc : MonoBehaviour
{
    // Id is a 4 digit(XYZT) integer. XY represents class, ZT represents level. Class starts from 10 and level starts from 1. 
    public int objId = 0;
    public int objClass = 0;
    public int objLevel = 0;
    public float moveSpeed = 1;
    public Color emptyColor, clickedColor, selectedColor;


    [NonSerialized]
    public bool isSelected = false;
    private bool movingToFirst = false;
    private bool shuffling = false;
    private GameManager gameManager;
    private Color defColor;
    private Text lvTx;
    private SpriteRenderer objIcon;
    private Transform targetObj, firstObj;
    private Vector3 shuffleTargetPos;

    void Awake()
    {
        //lvTx = transform.Find("Canvas").Find("LevelTx").GetComponent<Text>();
        objIcon = transform.Find("ObjIcon").GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetObjRandom();
    }

    private void Update()
    {
        if(movingToFirst)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetObj.position, moveSpeed * Time.deltaTime);
            if(transform.position == firstObj.position)
            {
                movingToFirst = false;
                gameManager.CheckMergeDone();
            }
        }

        else if(shuffling)
        {
            transform.position = Vector3.MoveTowards(transform.position, shuffleTargetPos, moveSpeed * 2 * Time.deltaTime);
            if (transform.position == shuffleTargetPos)
            {
                shuffling = false;
                gameManager.CheckShuffleDone();
            }
        }
    }

    public void MoveToFirst(Transform target, Transform first)
    {
        targetObj = target;
        firstObj = first;
        movingToFirst = true;
    }
    public void ShuffleTo(Vector3 target)
    {
        shuffleTargetPos = target;
        shuffling = true;
    }

    public bool IsMovingToFirst()
    {
        return movingToFirst;
    }
    public bool IsShuffling()
    {
        return shuffling;
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
        else if(isSelected)
        {
            gameManager.RewindSelecteds(gameObject);
        }
    }

    public void SetObjRandom()
    {
        objClass = UnityEngine.Random.Range(10, gameManager.itemMaxClass+1);
        objLevel = UnityEngine.Random.Range(1, gameManager.itemSpawnMaxLevel + 1);
        objId = objClass * 100 + objLevel;

        SetObjIcon();
    }
     
    public void IncreaseObjLevel(int addLevel)
    {
        if(objLevel + addLevel >= gameManager.itemMaxLevel)
        {
            objId = objClass * 100 + gameManager.itemMaxLevel;
        }
        else
        {
            objId += addLevel;
        }
        objLevel = objId % 100;
        SetCondition(0);
        SetObjIcon();
    }

    public void SetObjIcon()
    {
        objIcon.sprite = gameManager.GetIcon(objId);
    }

    public void SetCondition(int condition)
    {
        transform.Find("Stroke").gameObject.SetActive(false);
        switch (condition)
        {
            case 0: // Default state
                isSelected = false;
                GetComponent<SpriteRenderer>().color = emptyColor;
                break;
            case 1: // Clicked state
                isSelected = true;
                GetComponent<SpriteRenderer>().color = clickedColor;
                break;
            case 2: // Selected state
                isSelected = true;
                break;
                //Color setColor = clickedColor / gameManager.selectedObjs.Length;
                //setColor.a = 1;
                //GetComponent<SpriteRenderer>().color = setColor;
        }
    }
}
