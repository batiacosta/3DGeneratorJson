using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine.UI;

public class GeometryManager : MonoBehaviour
{
    [SerializeField] private string url = "https://api.jsonbin.io/v3/b/68b7786043b1c97be934ead9";
    [SerializeField] private Material opaqueMaterial;
    [SerializeField] private Material transluscentMaterial;
    
    private JsonTemplate _jsonTemplate = new JsonTemplate();
    private List<GameObject> _generatedObjects = new List<GameObject>();

    private void Start()
    {
        ViewControllerWindow.Instance.OnDropdown += Dropdown;
    }

    private void OnDestroy()
    {
        ViewControllerWindow.Instance.OnDropdown -= Dropdown;
    }

    private void Dropdown(string value)
    {
        switch (value)
        {
            case "Display all":
                SetAllOpaque();
                break;
            case "Translucent all":
                SetAllTransluscent();
                break;
            default:
                SetAllTransluscent();
                SetVisible(value);
                break;
        }
    }

    public void GenerateFromURL(string jsonURL)
    {
        NetworkManager.Instance.Get(jsonURL, ParseJson);
    }
    private void ParseJson(string jsonString)
    {
        
        Debug.Log($"I got this from URL: {jsonString}");
        _jsonTemplate = JsonConvert.DeserializeObject<JsonTemplate>(jsonString);
        Debug.Log($"Json is getting parsed as {_jsonTemplate.record == null}");
        var holderGameObject = new GameObject(_jsonTemplate.record.name);
        holderGameObject.AddComponent<MeshFilter>();
        holderGameObject.AddComponent<MeshRenderer>();
        holderGameObject.GetComponent<MeshRenderer>().material = opaqueMaterial;
        holderGameObject.transform.SetParent(transform);
        var mesh = CreateMesh();
        mesh.name = _jsonTemplate.record.name;
        holderGameObject.GetComponent<MeshFilter>().mesh = mesh;
        _generatedObjects.Add(holderGameObject);
        ViewControllerWindow.Instance.UpdateDropdown(holderGameObject);
    }

    private Mesh CreateMesh()
    {
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
        mesh.triangles = triangles.ToArray();
        return mesh;
    }

    public void SetVisible(string targetObjectName)
    {
        _generatedObjects.Find(x => x.gameObject.name == targetObjectName).GetComponent<MeshRenderer>().material = opaqueMaterial;
    }

    public void SetTransluscent(string targetObjectName)
    {
        _generatedObjects.Find(x => x.gameObject.name == targetObjectName).GetComponent<MeshRenderer>().material = transluscentMaterial;
    }

    public void SetAllTransluscent()
    {
        foreach (var generatedObject in _generatedObjects)
        {
            generatedObject.GetComponent<MeshRenderer>().material = transluscentMaterial;
        }
    }
    public void SetAllOpaque()
    {
        foreach (var generatedObject in _generatedObjects)
        {
            generatedObject.GetComponent<MeshRenderer>().material = opaqueMaterial;
        }
    }
}
