using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// the AI base will hold and control the other major AI scripts such as MT and CS types
/// the goal of this script is to make is simple to implement a new enemy or to swap out diffrent MT and CS scripts
/// </summary>
public class AIBase : MonoBehaviour
{
    public float baseDMG;
    public Transform combatTarget;

    public CS_Ranged CS_RangedScript;

    private NavMeshAgent agent;
    private MT_Hunt MT_HuntScript;

    public bool died;
    // Start is called before the first frame update
    void Start()
    {
        CS_RangedScript = GetComponent<CS_Ranged>();
        if(CS_RangedScript != null)
        {
            StartCoroutine(RangedCombatLoop());
        }
        MT_HuntScript = GetComponent<MT_Hunt>();
        if(MT_HuntScript != null)
        {
            StartCoroutine(MT_HuntScript.RangeCheck());
        }
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator RangedCombatLoop()
    {
        StartCoroutine(CS_RangedScript.AimCheck());
        CS_RangedScript.aimTarget = combatTarget;
        CS_RangedScript.baseAI = GetComponent<AIBase>();
        while(true)
        {
            yield return new WaitForSeconds(CS_RangedScript.fireInXs);
            CS_RangedScript.Fire();
        }
    }
}
