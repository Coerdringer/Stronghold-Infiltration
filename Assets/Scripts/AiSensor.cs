using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSensor : MonoBehaviour
{
    [SerializeField]
    float distance = 10;

    [SerializeField]
    [Range(0,360)]
    float angle = 30;

    
    //angled top and bottom
    [SerializeField]
    [Range(0, 360)]
    float verAngle = 90;

    [SerializeField]
    float height = 1.0f;

    [SerializeField]
    Color meshColor = Color.red;

    Mesh mesh;

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int numTriangles = 11;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        Vector3 topCenterAngled = bottomCenter + Vector3.up * (height/2);
        Vector3 topCenterAngledTop = Quaternion.Euler(-verAngle, 0, 0) * Vector3.forward * distance;
        Vector3 topCenterAngledBottom = Quaternion.Euler(verAngle, 0, 0) * Vector3.forward * distance;

        int vert = 0;

        // left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        // far side
        vertices[vert++] = bottomLeft;
        vertices[vert++] = bottomRight;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = topLeft;
        vertices[vert++] = bottomLeft;

        // top
        vertices[vert++] = topCenter;
        vertices[vert++] = topLeft; ;
        vertices[vert++] = topRight;

        // bottom
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomLeft;

        // vertically
        vertices[vert++] = topCenterAngled;
        vertices[vert++] = topCenterAngledTop;
        vertices[vert++] = topCenterAngledBottom;


        for (int i = 0; i < numVertices; i++) {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }
    }
}
