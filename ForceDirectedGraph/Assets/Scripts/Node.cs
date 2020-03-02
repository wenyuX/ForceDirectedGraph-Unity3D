using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private Rigidbody rd;
    private int thrust=3;
    private float damp = 5f;

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
        if (force == Vector3.zero)
        {
            //rd.velocity = Vector3.zero;
        }

        rd.AddForce(force-rd.velocity.normalized*this.damp);

        
        //nodeName.transform.LookAt(Camera.main.transform);
    }
}
