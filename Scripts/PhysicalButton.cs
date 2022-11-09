using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalButton : MonoBehaviour
{

    public UnityEvent functionToRun;
    private Animation pressAnimation;

    // Start is called before the first frame update
    void Start()
    {
        pressAnimation = gameObject.GetComponent<Animation>();
    }

    void Awake()
    {
        if (functionToRun == null)
        {
            functionToRun = new UnityEvent();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<GrabberScript>() != null)
        {
            pressAnimation.Play();
            functionToRun.Invoke();
        }
    }


}
