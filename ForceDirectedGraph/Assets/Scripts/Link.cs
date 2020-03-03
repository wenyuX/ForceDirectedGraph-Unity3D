using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public int id;
    public Node source;
    public Node target;
    public int value;

    void Update()
    {
        if (source && target)
        {
            float dis = (source.transform.position - target.transform.position).magnitude; 
            transform.localScale = new Vector3(transform.localScale.x, dis/2,transform.localScale.z);
            transform.position = (source.transform.position+target.transform.position)/2;
            transform.up = target.transform.position-source.transform.position;
        }
    }
}
