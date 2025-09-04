using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GeometryManager : MonoBehaviour
{
    [SerializeField] private string url = "https://api.jsonbin.io/v3/b/68b7786043b1c97be934ead9";
    [SerializeField] private Material material;
    
    private JsonTemplate _jsonTemplate = new JsonTemplate();
    private void Start()
    {
        NetworkManager.Instance.Get(url, ParseJson);
        // Parse JsonFile
        
        
    }

    private void ParseJson(string text)
    {
        Debug.Log($"I got this from URL: {text}");
        _jsonTemplate = JsonConvert.DeserializeObject<JsonTemplate>(text);
        ViewControllerWindow.Instance.SetInfoText(JsonConvert.SerializeObject(_jsonTemplate));
        Debug.Log($"Json is getting parsed as {_jsonTemplate.record == null}");
        var holderGameObject = new GameObject("MeshHolder");
        holderGameObject.AddComponent<MeshFilter>();
        holderGameObject.AddComponent<MeshRenderer>();
        holderGameObject.GetComponent<MeshRenderer>().material = material;
        holderGameObject.transform.SetParent(transform);
        var mesh = CreateMesh();
        mesh.name = "Generated mesh";
        holderGameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    private Mesh CreateMesh()
    {
       Debug.Log("Creating mesh");
        var model3DJson = _jsonTemplate.record;
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[model3DJson.vertices.Length];
        var verticesVector3 = new Vector3[model3DJson.vertices.Length];
        for (int i = 0; i < model3DJson.vertices.Length; i++)
        {
            verticesVector3[i] = new Vector3(model3DJson.vertices[i][0], model3DJson.vertices[i][1], model3DJson.vertices[i][2]);
        }
        mesh.SetVertices(verticesVector3);
        
        Debug.Log($"{model3DJson.faces.Length * 3}");

        List<int> triangles = new List<int>();
        foreach (var face in model3DJson.faces)
        {
            if (face.Length < 3) continue;
            for (int i = 0; i < face.Length - 1; i++)
            {
                triangles.Add(face[0]);
                triangles.Add(face[i]);
                triangles.Add(face[i + 1]);
            }
        }
        // var vertex1 = new Vector3(model3DJson.vertices[0][1], model3DJson.vertices[0][1], model3DJson.vertices[0][2]);
        // var vertex2 = new Vector3(model3DJson.vertices[1][1], model3DJson.vertices[1][1], model3DJson.vertices[1][2]);
        // var vertex3 = new Vector3(model3DJson.vertices[2][1], model3DJson.vertices[2][1], model3DJson.vertices[2][2]);
        // mesh.triangles =  new int[]{0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7, 0, 1, 5, 0, 5, 4};
        mesh.triangles = triangles.ToArray();
        return mesh;
    }
}
