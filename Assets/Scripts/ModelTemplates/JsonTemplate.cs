using UnityEngine;

[System.Serializable]
public class JsonTemplate
{
    public Model3D[] record;
}

public class Model3D
{
    public string name;
    public string type;
    public float[][] vertices;
    public int[][] faces;
}

 