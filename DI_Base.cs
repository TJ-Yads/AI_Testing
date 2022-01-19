using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// damage interaction is how a attack interacts with targets and enviorments
/// single target allows that attack to hit a single collider before ending-- can make use of penetration to allow it to hit more
/// AOE allows the attack to create a AOE field when it hits a collider
/// linger allows attacks to create an AOE field on colliders that stays for a short time
/// </summary>
public class DI_Base : MonoBehaviour
{
    public enum InteractionType
    {
        isSingle, isAOE, isLinger
    }
    public InteractionType interactionTypeCheck;
    public string interactionTracker;

    //AOE data
    public GameObject AOE_OBJ;
    public float AOE_Radius;
    public float AOE_Damage;
    public float AOE_Delay;

    //linger data
    public GameObject linger_OBJ;
    public float linger_Radius;
    public float linger_Damage;
    public float linger_TickRate;
    public float linger_Duration;

    // Start is called before the first frame update
    void Start()
    {
        SetInteractionType();
    }
    //when an interaction is set and an AI hits a target the projectile is given the data used to create the various effects
    //this switch statment will set the AIs DI type on start
    private void SetInteractionType()
    {
        switch(interactionTypeCheck)
        {
            case InteractionType.isSingle:
                interactionTracker = "Single";
                break;
            case InteractionType.isAOE:
                interactionTracker = "AOE";
                break;
            case InteractionType.isLinger:
                interactionTracker = "Linger";
                break;
        }
    }
}
