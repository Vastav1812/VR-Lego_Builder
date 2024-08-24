using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Outliner : MonoBehaviour
{
    // Start is called before the first frame update
    public  bool is_outlined = true;
    void Start()
    {
        this.GetComponent<Outline>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(is_outlined == false)
        {
            this.GetComponent<Outline>().enabled = false;
        }
    }
}
