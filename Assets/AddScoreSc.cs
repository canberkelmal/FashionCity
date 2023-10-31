using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddScoreSc : MonoBehaviour
{
    public float t = 1f;
    
    private float dt = 0f;

    private Vector3 startPos;
    private void Awake()
    {
        startPos = transform.localPosition;
    }
    private void FixedUpdate()
    {
        dt += Time.deltaTime;

        if (dt < t)
        {
            float tNormalized = dt / t;
            Vector3 newPosition = Vector3.Lerp(startPos, Vector3.zero, tNormalized);
            transform.localPosition = newPosition;


            float lerpedX = Mathf.Lerp(1.0f, 0.0f, tNormalized);
            Color currentClr = GetComponent<Text>().color;
            currentClr.a = lerpedX;
            GetComponent<Text>().color = currentClr;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
