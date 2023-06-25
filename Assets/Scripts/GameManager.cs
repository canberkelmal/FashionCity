using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject objPrefab;
    public Transform objs;
    public LayerMask objLayerMask;
    public Color colorA, colorB, colorC;

    [NonSerialized]
    public int clickedObjId = 0;
    [NonSerialized]
    public Transform[] selectables = new Transform[0];

    private Camera mainCamera;
    private GameObject clickedObj;
    private GameObject[] selectedObjs = new GameObject[0];


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Time.timeScale = 2;
        Initialize();
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

        // Restart the game when the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
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

    private void AddSelectedObjsArray(GameObject obj)
    {
        SetSelectables(obj.transform);
        int newSize = (selectedObjs != null) ? selectedObjs.Length + 1 : 1;
        Array.Resize(ref selectedObjs, newSize);

        selectedObjs[newSize - 1] = obj;
    }

    private void ReleaseInput()
    {
        clickedObj = null;
        clickedObjId = 0;
        ResetSelectedObjsArray();
    }

    private void ResetSelectedObjsArray()
    {
        foreach (GameObject obj in selectedObjs)
        {
            obj.GetComponent<ObjSc>().SetCondition(0);
        }

        if(selectedObjs.Length >= 2)
        {
            DestroySelecteds();
        }
        else
        {
            selectedObjs = new GameObject[0];
        }
        
    }

    private void DestroySelecteds()
    {
        foreach (GameObject obj in selectedObjs)
        {
            SpawnObj(obj.transform.position);
            Destroy(obj);
        }

        selectedObjs = new GameObject[0];
    }

    private void SetSelectables(Transform checkedObj)
    {
        Collider2D[] selectablesColliders = Physics2D.OverlapCircleAll(checkedObj.position, 0.3f, objLayerMask);
        selectables = new Transform[selectablesColliders.Length];
        for (int i = 0; i < selectablesColliders.Length; i++)
        {
            selectables[i] = selectablesColliders[i].transform;
        }
    }

    public bool CheckSelectable(Transform obj)
    {
        bool r = false;
        foreach(Transform checkingObj  in selectables)
        {
            if(checkingObj == obj)
            {
                r = true;
            }
        }
        return r;
    }

    private void SpawnObj(Vector3 refPoint)
    {
        Instantiate(objPrefab, refPoint + Vector3.up * 6f, Quaternion.identity, objs);

    }

    private void Initialize()
    {
        Vector2 targetSpawnPoint = objs.GetChild(0).position + Vector3.up*0.5f;
        float startX = targetSpawnPoint.x;
        for (int i = 0; i < 10;  i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Instantiate(objPrefab, targetSpawnPoint, Quaternion.identity, objs);
                targetSpawnPoint += Vector2.right * 0.5f;
            }
            targetSpawnPoint += Vector2.up * 0.5f;
            targetSpawnPoint.x = startX;
        }
        //Instantiate(objPrefab,);
    }

    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
