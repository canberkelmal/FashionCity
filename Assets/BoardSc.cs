using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSc : MonoBehaviour
{
    private GameManager gameManager;
    private Transform buttom, top, left, right;
    private float rows,cols;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        buttom = transform.Find("Buttom");
        top = transform.Find("Top");
        left = transform.Find("Left");
        right = transform.Find("Right");
        rows = gameManager.rows;
        cols = gameManager.cols;
        Initialize();
    }
    public void Initialize()
    {
        rows = gameManager.rows;
        cols = gameManager.cols;
        buttom.localScale = new Vector3 (cols/2 + 0.2f , 0.1f, 1);
        top.localScale = buttom.localScale;

        right.localScale = new Vector3(rows/2 + 0.3f, 0.1f, 1);
        left.localScale = right.localScale;


        top.localPosition = buttom.localPosition + (Vector3.up * (rows / 2 + 0.2f)) - Vector3.forward * 0.2f;
        right.localPosition = new Vector3(cols * 0.25f + 0.07f, rows / 4 + 0.1f, -0.2f);
        left.localPosition = new Vector3(-cols * 0.25f - 0.07f, rows / 4 + 0.1f, -0.2f);
    }
}
