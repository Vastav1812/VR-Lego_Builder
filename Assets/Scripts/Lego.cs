using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lego : MonoBehaviour
{
     [HideInInspector]
    public BoxCollider Collider;
    //public Outliner my_script;
    
    public void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }

}
