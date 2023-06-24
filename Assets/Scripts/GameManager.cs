using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameManager : MonoBehaviour
{
    public Transform objs;

    [NonSerialized]
    public int clickedObjId = 0;

    private Camera mainCamera;
    private GameObject clickedObj;
    private GameObject[] selectedObjs = new GameObject[0];

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
    }

    private void InputManager()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseInput();
        }
    }

    public void OnObjClicked(GameObject obj)
    {
        clickedObj = obj;
        clickedObjId = clickedObj.GetComponent<ObjSc>().objId;
        AddSelectedObjsArray(obj);
        clickedObj.GetComponent<ObjSc>().SetCondition(1);
    }
    public void OnObjSelected(GameObject obj)
    {
        AddSelectedObjsArray(obj);
        obj.GetComponent<ObjSc>().SetCondition(2);
    }

    private void ReleaseInput()
    {
        clickedObj = null;
        clickedObjId = 0;
        ResetSelectedObjsArray();
    }

    private void AddSelectedObjsArray(GameObject obj)
    {
        int newSize = (selectedObjs != null) ? selectedObjs.Length + 1 : 1;
        Array.Resize(ref selectedObjs, newSize);

        selectedObjs[newSize - 1] = obj;
    }

    private void ResetSelectedObjsArray()
    {
        foreach (GameObject obj in selectedObjs)
        {
            obj.GetComponent<ObjSc>().SetCondition(0);
        }
        
        selectedObjs = new GameObject[0];
    }
}
