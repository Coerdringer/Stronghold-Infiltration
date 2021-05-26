using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalGizmo : MonoBehaviour
{
    [SerializeField]
    float radius;
    
    [SerializeField]
    Color meshColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = meshColor;
        Gizmos.DrawSphere(transform.position, radius);

        //colour transparency A = 22
    }
}
