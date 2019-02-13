using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lattice : Controllable
{

    public Vector3Int dimensions = new Vector3Int(1, 1, 1);
    public float margin = 0.0f;
    public Shader shader;

    // Use this for initialization
    void Start()
    {

        Vector3[] centers = CubeCenters();

        foreach (Vector3 center in centers)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = gameObject.transform;
            cube.transform.position = center;

            if (shader)
            {
                Renderer cubeRenderer = cube.GetComponent<Renderer>();
                cubeRenderer.material.shader = shader;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.white;

        Vector3[] centers = CubeCenters();

        foreach (Vector3 center in centers)
        {
            Gizmos.DrawWireCube(center, Vector3.one);
        }
    }

    Vector3[] CubeCenters()
    {

        List<Vector3> centers = new List<Vector3>();

        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                for (int z = 0; z < dimensions.z; z++)
                {
                    centers.Add(CubeCenterForIndex(new Vector3Int(x, y, z)));
                }
            }
        }

        return centers.ToArray();
    }

    public Vector3 CubeCenterForIndex(Vector3Int index)
    {

        Vector3 centerDimensions = dimensions - new Vector3(1, 1, 1);
        Vector3 centerHalfDimensions = centerDimensions / 2.0f;

        float fX = (float)index.x;
        float fY = (float)index.y;
        float fZ = (float)index.z;

        float cX = fX.Remap(0, centerDimensions.x, -centerHalfDimensions.x, centerHalfDimensions.x);
        float cY = fY.Remap(0, centerDimensions.y, -centerHalfDimensions.y, centerHalfDimensions.y);
        float cZ = fZ.Remap(0, centerDimensions.z, -centerHalfDimensions.z, centerHalfDimensions.z);

        return new Vector3(cX, cY, cZ) * (1.0f + margin);
    }

    public override void SetRotation(Quaternion rotation)
    {
        Debug.Log(rotation);
        gameObject.transform.rotation = rotation;
    }
}
