using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_enterGate : MonoBehaviour
{
    public Animator myAnimator;



    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void openGate()
    {
        myAnimator.Play("Open");
        Destroy(this);
    }
}
