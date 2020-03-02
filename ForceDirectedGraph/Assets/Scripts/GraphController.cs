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

    private float springK = 5f;
    private float originalLen = 10f;
    private float c = 40f;

    private void LoadLayout()
    {
        /*
        Node node1 = Instantiate(nodePrefab, new Vector3(-4, 0, 0), Quaternion.identity) as Node;
        Node node2 = Instantiate(nodePrefab, new Vector3(4, 0, 0), Quaternion.identity) as Node;
        nodes.Add(1,node1);
        nodes.Add(2,node2);

        Link link = Instantiate(linkPrefab, new Vector3(0, 0, 0), Quaternion.identity) as Link;
        link.id = 1;
        link.source = nodes[1] as Node;
        link.target = nodes[2] as Node;

        links.Add(1,link);
        */
        
        string dataJsonPath = Application.dataPath + "/Data/data.json";
        System.IO.StreamReader sr = new System.IO.StreamReader(dataJsonPath);
        string jsonStr = sr.ReadToEnd();
        JsonDataClass jsonDataClass = JsonUtility.FromJson<JsonDataClass>(jsonStr);

        
        System.Random rd = new System.Random();
        for (int i = 0; i < jsonDataClass.nodes.Length; i++)
        {
            float x = rd.Next(-20, 20);
            float y = rd.Next(-20, 20);
            float z = rd.Next(-20, 20);
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

    private void Update()
    {
        /*
        Node node1 = nodes[1] as Node;
        Node node2 = nodes[2] as Node;
        node1.force = new Vector3(0,0,0);
        node2.force = new Vector3(0,0,0);
        Vector3 dis = node1.transform.position - node2.transform.position;
        Vector3 springForce1 = (dis.magnitude-originalLen) * springK * -dis.normalized;
        Vector3 springForce2 = -springForce1;

        node1.force = springForce1;
        node2.force = springForce2;
        logger.Log(kTAG, "node1 " + springForce1.ToString());
        logger.Log(kTAG, "node2 " + springForce2.ToString());
        
        Vector3 coulombForce1 = dis.normalized / (dis.magnitude * dis.magnitude) * c;
        Vector3 coulombForce2 = -coulombForce1;
        node1.force += coulombForce1;
        node2.force += coulombForce2;
        */
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
