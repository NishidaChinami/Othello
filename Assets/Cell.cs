using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColorY()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }
    public void ChangeColorG() {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

}
