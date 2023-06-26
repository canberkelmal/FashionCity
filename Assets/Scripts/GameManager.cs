using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float rows, cols;
    public Slider rowSlider, colSlider;
    public GameObject objPrefab, vibBut, setPanel;
    public Transform objs;
    public Transform board;
    public LayerMask objLayerMask;
    public Color colorA, colorB, colorC;

    [NonSerialized]
    public int clickedObjId = 0;
    [NonSerialized]
    public Transform[] selectables = new Transform[0];

    private GameObject clickedObj;
    private GameObject[] selectedObjs = new GameObject[0];
    private bool vibrating = false;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 2;
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
        if (vibrating)
        {
            Handheld.Vibrate();
        }

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
        Instantiate(objPrefab, refPoint + Vector3.up * (rows/2), Quaternion.identity, objs);
    }

    private void Initialize()
    {
        float objXPos = -(cols - 1) * 0.25f;
        objs.GetChild(0).position = board.Find("Buttom").position + new Vector3(objXPos, 0.35f, 0);

        Vector2 targetSpawnPoint = objs.GetChild(0).position + (Vector3.right * 0.5f);
        float startX = objs.GetChild(0).position.x;

        for (int i = 0; i < rows;  i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if(i == 0 && j==0)
                {
                    j++;
                }
                Instantiate(objPrefab, targetSpawnPoint, Quaternion.identity, objs);
                targetSpawnPoint += Vector2.right * 0.5f;
            }
            targetSpawnPoint += Vector2.up * 0.6F;
            targetSpawnPoint.x = startX;
        }
    }

    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Vibrating()
    {
        vibrating = !vibrating;
        vibBut.transform.Find("CondTx").GetComponent<Text>().text = vibrating ? "On" : "Off";
        if(vibrating)
        {
            Handheld.Vibrate();
        }
    }

    public void SetRows()
    {
        rows = rowSlider.value;
        setPanel.transform.Find("RowTx").GetComponent<Text>().text = "Rows: " + rows.ToString(); 
        board.GetComponent<BoardSc>().Initialize();
        float objXPos = -(cols - 1) * 0.25f;
        objs.GetChild(0).position = board.Find("Buttom").position + new Vector3(objXPos, 0.35f, 0);
    }
    public void SetColumns()
    {
        cols = colSlider.value;
        setPanel.transform.Find("ColTx").GetComponent<Text>().text = "Columns: " + cols.ToString();
        board.GetComponent<BoardSc>().Initialize();
        float objXPos = -(cols - 1) * 0.25f;
        objs.GetChild(0).position = board.Find("Buttom").position + new Vector3(objXPos, 0.35f, 0);
    }

    public void StartGame()
    {
        setPanel.SetActive(false);
        Initialize();
    }
}
