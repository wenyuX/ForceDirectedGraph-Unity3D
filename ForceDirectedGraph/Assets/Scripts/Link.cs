using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public int id;
    public Node source;
    public Node target;
    public int value;
    
    public bool loaded = false;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.material = new Material(Shader.Find("Self-Illumin/Diffuse"));

        float width = 0.2f;
        lineRenderer.startWidth = width;
        
        lineRenderer.endWidth = width;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        lineRenderer.SetPosition(1, new Vector3(1, 0, 0));
    }

    void Update()
    {
        if (source && target && !loaded)
        {
            
            lineRenderer.SetPosition(0, source.transform.position);
            lineRenderer.SetPosition(1, target.transform.position);

            //loaded = true;
        }
    }
}
