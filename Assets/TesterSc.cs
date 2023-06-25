using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterSc : MonoBehaviour
{
    public Transform testObj1;
    public Collider2D[] colliders;
    public LayerMask layerMask;
    public float radius = 0.6f;
    // Start is called before the first frame update
    void Update()
    {
        colliders = Physics2D.OverlapCircleAll(testObj1.position, radius, layerMask);
    }
}
