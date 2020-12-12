using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidCreator : MonoBehaviour
{
    private List<Transform> pyramids = new List<Transform>();
    private float timeOffset = 0f;

    private void Start()
    {
        CreatePyramidsStack(10);
        foreach (Transform trans in pyramids)
        {
            float min = trans.position.x >= 0 ? trans.position.x * -1f : trans.position.x;
            float max = trans.position.x >= 0 ? trans.position.x : trans.position.x * -1f;
            Color color = new Color(Mathf.Lerp(0, 1, GetScaledValue(trans.position.x, min, max)),
                Mathf.Lerp(0, 1, GetScaledValue(trans.position.y, 0f, 9f)),
                Mathf.Lerp(0, 1, GetScaledValue(trans.position.z, min, max)
                ));
            trans.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        }
    }

    /// <summary>
    /// 값의 범위를 0 ~ 1 로 축소 시키는 함수.
    /// </summary>
    private float GetScaledValue(float rawValue, float min, float max)
    {
        return (rawValue - min) / (max - min);
    }

    private void CreatePyramidsStack(int stackSize)
    {
        GameObject pyramid = new GameObject("root");
        float xy_offset = (float)stackSize * 0.5f - 0.5f;
        for (int y = 0; y < stackSize; ++y)
        {
            for (int x = 0; x < stackSize - y; ++x)
            {
                for (int z = 0; z < stackSize - y; ++z)
                {
                    pyramids.Add(CreatePyramid(new Vector3(x - xy_offset + (float)y * 0.5f, y, z - xy_offset + (float)y * 0.5f), pyramid.transform));
                }
            }
        }
    }

    /// <summary>
    /// 정사각뿔 오브젝트 생성.
    /// </summary>
    /// <returns></returns>
    private Transform CreatePyramid(Vector3 worldPosition, Transform parent)
    {
        GameObject pyramid = new GameObject("Pyramid");
        MeshRenderer renderer = pyramid.AddComponent<MeshRenderer>();
        MeshFilter filter = pyramid.AddComponent<MeshFilter>();

        Vector3[] vertices =
        {
            //Bottom Square vertices. 
            new Vector3(-0.5f, 0f, -0.5f),
            new Vector3(0.5f, 0f, -0.5f),
            new Vector3(0.5f, 0f, 0.5f),
            new Vector3(-0.5f, 0f, 0.5f),

            //Top vertex.
            new Vector3(0f, 1f, 0f),
        };

        Vector3[] normals =
        {
            Vector3.down,
            Vector3.down,
            Vector3.down,
            Vector3.down,
            Vector3.up,
        };

        int[] indices =
        {
            //Front face.
            2, 4, 3,
            //Right face,
            1, 4, 2,
            //Back face,
            0, 4, 1,
            //Left face,
            3, 4, 0,
            //Bottom faces
            0, 1, 3,
            3, 1, 2
        };

        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);
        mesh.RecalculateNormals();
        filter.sharedMesh = mesh;

        renderer.material = new Material(Shader.Find("Standard"));

        pyramid.transform.SetParent(parent);
        pyramid.transform.position = worldPosition;
        return pyramid.transform;
    }
}


