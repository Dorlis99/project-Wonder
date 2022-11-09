using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTool_MagnusGlass : MonoBehaviour
{

    public ParticleSystem[] PS1;
    public ParticleSystem[] PS2;
    private bool particleEnabled;

    public TextMeshPro nameText;
    public TextMeshPro typeText;

    
    private bool objectDetected;
    private MagnusGlass_tag detectedTag;
    private bool isDisplayingText;
    private Animator myAnimator;

    public Transform raycastForward;

    void Start()
    {
        myAnimator = gameObject.GetComponent<Animator>();
        

        toggleParticles(false);
    }

    void Update()
    {
        Debug.DrawRay(raycastForward.position, raycastForward.TransformDirection(Vector3.forward) * 2, Color.yellow);

        RaycastHit hit;

        if (Physics.Raycast(raycastForward.position, raycastForward.TransformDirection(Vector3.forward), out hit, 2))
        {
            if(hit.collider.gameObject.GetComponent<MagnusGlass_tag>() != null)
            {
                detectedTag = hit.collider.gameObject.GetComponent<MagnusGlass_tag>();
                isDisplayingText = true;
                nameText.text = detectedTag.MagnusName;
                typeText.text = detectedTag.MagnusType;
            }

            if(hit.collider.gameObject.GetComponent<Bottle>() != null)
            {
                var bottle = hit.collider.gameObject.GetComponent<Bottle>();

                if(bottle.content == Bottle.flaskContentTypes.liquid)
                {
                    isDisplayingText = true;
                    nameText.text = bottle.liquidFlaskContentName;
                    typeText.text = "Potion";
                }
            }
           
        }
        else
        {
            detectedTag = null;
            isDisplayingText = false;
        }
        
        



        if(isDisplayingText && particleEnabled == false)
        {
            particleEnabled = true;
            myAnimator.Play("ShowText");
            toggleParticles(true);
        }

        if(isDisplayingText == false && particleEnabled)
        {
            particleEnabled = false;
            myAnimator.Play("HideText");
            toggleParticles(false);
        }

        

    }

    public void toggleParticles(bool enabled)
    {
        foreach(ParticleSystem ps in PS1)
        {
            if(enabled)
            {
                ps.Play();
            }
            if(enabled == false)
            {
                ps.Stop();
            }
        }

        foreach (ParticleSystem ps in PS2)
        {
            if (enabled)
            {
                ps.Play();
            }
            if (enabled == false)
            {
                ps.Stop();
            }
        }
    }
}
