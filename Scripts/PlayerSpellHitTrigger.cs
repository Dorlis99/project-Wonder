using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpellHitTrigger : MonoBehaviour
{
    public bool requiresSpecificSpell;
    public HandMagicCaster.spells spellTypeRequired;
    public UnityEvent targetFunction;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void hitTheTrigger(HandMagicCaster.spells spellType)
    {
        if(requiresSpecificSpell)
        {
            if(spellTypeRequired != spellType)
            {
                return;
            }
        }

        targetFunction.Invoke();
    }
}
