using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float rows, cols;
    public Slider rowSlider, colSlider;
    public GameObject objPrefab, addScorePrefab, vibBut, setPanel;
    public Text scoreTx;
    public Transform objs;
    public Transform board;
    public LayerMask objLayerMask;
    public Color colorA, colorB, colorC;
    public int score = 0;
    public int addScoreMinMultiplier = 3;
    public int itemSpawnMaxLevel = 4;
    public int itemMaxLevel = 4;
    public int itemMaxClass = 11;
    public Sprite[] class10Items = new Sprite[0];
    public Sprite[] class11Items = new Sprite[0];
    public Sprite[] class12Items = new Sprite[0];
    public LineRenderer lineRenderer;



    [NonSerialized]
    public int clickedObjId = 0;
    [NonSerialized]
    public Transform[] selectables = new Transform[0];

    private GameObject clickedObj;
    public GameObject[] selectedObjs = new GameObject[0];
    private bool vibrating = false;
    private int selectedCount = 0;

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
        lineRenderer.enabled = true;
        AddSelectedObjsArray(obj);
        obj.GetComponent<ObjSc>().SetCondition(2);
    }

    private void AddSelectedObjsArray(GameObject obj)
    {
        SetSelectables(obj.transform);
        int newSize = (selectedObjs != null) ? selectedObjs.Length + 1 : 1;
        Array.Resize(ref selectedObjs, newSize);

        selectedObjs[newSize - 1] = obj;
        SetSelectLine();
    }
     
    public void RewindSelecteds(GameObject onMouseObj)
    {
        SetSelectables(onMouseObj.transform);

        GameObject[] tempSelectedObjs = new GameObject[0];
        bool foundTheObj = false;
        for(int i = 0; i < selectedObjs.Length; i++)
        {
            GameObject obj = selectedObjs[i];
            if(foundTheObj)
            {
                obj.GetComponent<ObjSc>().SetCondition(0);
            }
            else
            {
                int newTempSize = tempSelectedObjs.Length+1;
                Array.Resize(ref tempSelectedObjs, newTempSize);

                tempSelectedObjs[newTempSize - 1] = obj;
            }

            if (obj == onMouseObj)
            {
                foundTheObj = true;
            }
        }
        selectedObjs = tempSelectedObjs;
        SetSelectLine();
    }

    private void ReleaseInput()
    {
        clickedObj = null;
        clickedObjId = 0;
        ResetSelectedObjsArray();
        lineRenderer.enabled = false;
    }

    private void ResetSelectedObjsArray()
    {
        for (int i = 1; i < selectedObjs.Length; i++)
        {
            selectedObjs[i].GetComponent<ObjSc>().SetCondition(0);
        }

        if (selectedObjs.Length >= 3)
        {
            //SetScore(selectedObjs.Length);
            selectedCount = selectedObjs.Length;
            StartMerge();
        }
        else
        {
            for (int i = 0; i < selectedObjs.Length; i++)
            {
                selectedObjs[i].GetComponent<ObjSc>().SetCondition(0);
            }
            selectedObjs = new GameObject[0];
        }        
    }

    private void StartMerge()
    {        
        // Create new obj instead of mergeds.
        for (int i = 1; i < selectedObjs.Length; i++)
        {
            SpawnObj(selectedObjs[i].transform.position);
        }
        SetObjKinematic(true);

        // Close colliders and start merge movement of objects.
        for (int i = 1; i < selectedObjs.Length; i++)
        {
            GameObject obj = selectedObjs[i];
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            obj.GetComponent<ObjSc>().MoveToFirst(selectedObjs[i-1].transform, selectedObjs[0].transform); 
        }
    }

    public void CheckMergeDone()
    {        
        bool isDone = true;
        for (int i = 1; i < selectedObjs.Length; i++)
        {
            if (selectedObjs[i].GetComponent<ObjSc>().IsMovingToFirst())
            {
                isDone = false;
                break;
            }
        }

        if (isDone)
        {
            DestroySelecteds();
        }
    }

    private void DestroySelecteds()
    {
        for (int i = 1; i < selectedObjs.Length; i++)
        {
            GameObject obj = selectedObjs[i];
            Destroy(obj);
        }

        SetMergedObjLevel();

        SetObjKinematic(false);
    }

    private void SetMergedObjLevel()
    {
        // Set level for the merged obj
        int addLevelCount;
        if (selectedCount >= 7)
        {
            addLevelCount = 3;
        }
        else if (selectedCount >= 5)
        {
            addLevelCount = 2;
        }
        else
        {
            addLevelCount = 1;
        }

        selectedObjs[0].GetComponent<ObjSc>().IncreaseObjLevel(addLevelCount);
        ResetSelecteds();
    }

    private void SetObjKinematic(bool kin)
    {
        foreach(Transform obj in objs)
        {
            obj.gameObject.GetComponent<Rigidbody2D>().simulated = !kin; 
        }
    }

    // Set selected objs parameters to default
    private void ResetSelecteds()
    {
        selectedCount = 0;
        selectedObjs = new GameObject[0];
    }

    // Set score scripts
    /*private void SetScore(int numberOfObj)
    {
        int addScore = numberOfObj * (numberOfObj % addScoreMinMultiplier);
        score += addScore;
        PlayerPrefs.SetInt("Score", score);
        scoreTx.text = score.ToString();
        SpawnScoreObj(addScore);
    }

    private void SpawnScoreObj(int addScore)
    {
        float randomAngle = UnityEngine.Random.Range(210f, 330f);
        float radianAngle = randomAngle * Mathf.Deg2Rad;

        Vector3 centerPosition = scoreTx.gameObject.GetComponent<RectTransform>().position;
        Vector3 spawnPosition = centerPosition + new Vector3(Mathf.Cos(radianAngle) * 144, Mathf.Sin(radianAngle) * 144, 0);

        GameObject spawnedText = Instantiate(addScorePrefab, spawnPosition, Quaternion.identity, scoreTx.gameObject.transform);

        spawnedText.GetComponent<Text>().text = "+" + addScore.ToString();
    }*/

    private void SetSelectables(Transform checkedObj)
    {
        Collider2D[] selectablesColliders = Physics2D.OverlapCircleAll(checkedObj.position, 0.3f, objLayerMask);
        selectables = new Transform[selectablesColliders.Length];
        for (int i = 0; i < selectablesColliders.Length; i++)
        {
            selectables[i] = selectablesColliders[i].transform;
        }
    }
    public void SetSelectLine()
    {
        Vector3[] points = new Vector3[selectedObjs.Length];
        for(int i = 0; i < points.Length; i++)
        { 
            points[i] = selectedObjs[i].transform.position;
        }
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
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
        GameObject spawnedObj = Instantiate(objPrefab, refPoint + Vector3.up * (rows/2), Quaternion.identity, objs);
    }

    public Sprite GetIcon(int id)
    {
        Sprite returnIcon = null;

        if( id / 100 == 10 )
        {
            returnIcon = class10Items[(id%100)-1];
        }
        else if( id / 100 == 11)
        {
            returnIcon = class11Items[(id % 100) - 1];
        }

        return returnIcon;
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
