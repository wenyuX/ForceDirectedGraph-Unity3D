using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private Rigidbody rd;
    private int thrust=3;
    private float damp = 30f;
    private float gravity = 5f;
    private float epsilon = 0f ;

    public int id;
    //public string name;
    public int group;
    //public TextMesh nodeName;

    private static ILogger logger = Debug.unityLogger;
    private string kTAG = "Node ";

    public Vector3 force;
    void Start()
    {
        this.kTAG += this.name;
        rd = this.GetComponent<Rigidbody>();
        force = new Vector3(0,0,0);
    }

 
    void Update()
    {
        
        force += -rd.velocity * this.damp;
        force += -this.transform.position * this.gravity;
        if(this.name == "Child1")
            logger.Log(this.kTAG, force.magnitude.ToString());
        if (force.magnitude < this.epsilon)
        {
            rd.velocity = Vector3.zero;
            return;
        }
        rd.AddForce(force);
        //nodeName.transform.LookAt(Camera.main.transform);
        
    }
}
