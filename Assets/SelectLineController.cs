using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLineController : MonoBehaviour
{
    LineRenderer lineRenderer;

    public void SetSelectLine(Vector3[] points)
    {
        lineRenderer.SetPositions(points);
    }
}
