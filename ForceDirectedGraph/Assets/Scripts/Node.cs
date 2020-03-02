using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private Rigidbody rd;
    private int thrust=3;
    private float damp = 100f;
    private float gravity = 10f;
    private float epsilon = 1f ;

    public int id;
    //public string name;
    public int group;
    //public TextMesh nodeName;

    public Vector3 force;
    void Start()
    {
        rd = this.GetComponent<Rigidbody>();
        force = new Vector3(0,0,0);
    }

 
    void Update()
    {
        /*
        if (force.magnitude < this.epsilon)
        {
            rd.velocity = Vector3.zero;
            return;
        }
        */
        force += -rd.velocity.normalized * this.damp;
        force += -this.transform.position * this.gravity;
        rd.AddForce(force);
        //nodeName.transform.LookAt(Camera.main.transform);
    }
}
