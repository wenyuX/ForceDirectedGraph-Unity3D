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
    private Hashtable colors;
    
    private int nodeCount = 0;
    private int linkCount = 0;

    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "GraphController";

    private float springK = 50f;
    private float originalLen = 10f;
    private float c = 5000f;

    private float randomSign()
    {
        System.Random rd = new System.Random();
        double tmp = rd.NextDouble();
        if (tmp > 0.5) return 1;
        return -1;
    }

    private void LoadLayout()
    {
        string dataJsonPath = Application.dataPath + "/Data/data.json";
        System.IO.StreamReader sr = new System.IO.StreamReader(dataJsonPath);
        string jsonStr = sr.ReadToEnd();
        JsonDataClass jsonDataClass = JsonUtility.FromJson<JsonDataClass>(jsonStr);

  
        System.Random rd = new System.Random();
        for (int i = 0; i < jsonDataClass.nodes.Length; i++)
        {
            int range = 50;
            float x = rd.Next(-range,range);
            float y = rd.Next(-range, range);
            float z = rd.Next(-range, range);

            Node node = Instantiate(nodePrefab,new Vector3(x,y,z),Quaternion.identity) as Node;
          
            node.id = i;
            node.name = jsonDataClass.nodes[i].name;
            node.group = jsonDataClass.nodes[i].group;
            node.GetComponent<MeshRenderer>().material.SetColor("_Color", (Color)colors[node.group]);

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
        colors = new Hashtable();

        colors.Add(0,new Color(0.8f,0.25f,0.5f)); //Purple
        colors.Add(1,new Color(0.25f, 0.8f, 1f)); //Light blue 
        colors.Add(2,new Color(0,0.25f,0)); //Dark green
        colors.Add(3, Color.gray);
        colors.Add(4, new Color(0f, 0.25f, 0.5f)); //Dark blue
        colors.Add(5, new Color(1f,0.25f,0.25f)); //Red
        colors.Add(6, new Color(1,0.5f,0f)); //Orange
        colors.Add(7, new Color(0.25f,0.8f,0.25f));
        colors.Add(8, new Color(1,0.75f,0.25f)); // Yellow
        colors.Add(9, new Color(0f,0,0f)); // Empty
        colors.Add(10, new Color(1,0.8f,0.8f));
        
        LoadLayout();
    }

    private void Update()
    {
        //Coulomb force
        foreach(Node i in nodes.Values)
        {
            i.force = Vector3.zero;
            foreach(Node j in nodes.Values)
            {
                if (i.id == j.id) continue;
                Vector3 dis = i.transform.position - j.transform.position;
                i.force += dis.normalized / (dis.magnitude * dis.magnitude) * c;
            }
        }
        //Spring force
        foreach(Link link in links.Values)
        {
            Node source = link.source;
            Node target = link.target;
            Vector3 dis = source.transform.position - target.transform.position;
            Vector3 springForceToSource = (dis.magnitude - originalLen) * springK * -dis.normalized;
            Vector3 springForceToTarget = -springForceToSource;
            source.force += springForceToSource;
            target.force += springForceToTarget;
        } 
        
    }
}
