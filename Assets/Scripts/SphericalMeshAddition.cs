using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalMeshAddition : MonoBehaviour
{
    [SerializeField]
    Transform Enemy;

    GameObject sphere;

    
    // Start is called before the first frame update
    void Start()
    {
        SphereGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy != null)
            sphere.transform.position = Enemy.position;
    }

    object SphereGeneration()
    {
        Vector3 center = Enemy.position + Vector3.up;

        sphere = Instantiate(Resources.Load("TransparentFOVSphere"), center, Quaternion.identity) as GameObject;

        return sphere;
    }
}
