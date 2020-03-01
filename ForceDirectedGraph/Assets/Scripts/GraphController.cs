using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeData
{
    public string name;
    public int group;
}

[System.Serializable]
public class LinkData
{
    public int source;
    public int target;
    public int value;
}

[System.Serializable]
public class JsonDataClass
{
    public NodeData[] nodes;
    public LinkData[] links;
}

public class GraphController : MonoBehaviour
{
    public Node nodePrefab;
    public Link linkPrefab;

    private Hashtable nodes;
    private Hashtable links;
    
    private int nodeCount = 0;
    private int linkCount = 0;

    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "GraphController";

    private void LoadLayout()
    {
        string dataJsonPath = Application.dataPath + "/Data/data.json";
        System.IO.StreamReader sr = new System.IO.StreamReader(dataJsonPath);
        string jsonStr = sr.ReadToEnd();
        JsonDataClass jsonDataClass = JsonUtility.FromJson<JsonDataClass>(jsonStr);

        //logger.Log(kTAG, jsonDataClass.links[0].source);
        System.Random rd = new System.Random();
        for (int i = 0; i < jsonDataClass.nodes.Length; i++)
        {
            float x = rd.Next(1, 50);
            float y = rd.Next(1, 50);
            float z = rd.Next(1, 50);
            Node node = Instantiate(nodePrefab,new Vector3(x,y,z),Quaternion.identity) as Node;
            node.id = i;
            node.name = jsonDataClass.nodes[i].name;
            node.group = jsonDataClass.nodes[i].group;

            nodes.Add(node.id, node);
            nodeCount++;
        }
        for (int i = 0; i < jsonDataClass.links.Length; i++)
        {
            Node source = nodes[jsonDataClass.links[i].source] as Node;
            Node target = nodes[jsonDataClass.links[i].target] as Node;
            Link link = Instantiate(linkPrefab, new Vector3(0, 0, 0), Quaternion.identity) as Link;
            link.id = i;
            link.source = source;
            link.target = target;
            link.value = jsonDataClass.links[i].value;

            links.Add(link.id,link);
            linkCount++;
        }
           
    }
    
    void Start()
    {
        nodes = new Hashtable();
        links = new Hashtable();

        LoadLayout();
        
    }
}
