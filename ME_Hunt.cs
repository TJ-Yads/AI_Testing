using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// hunt maneuverers allows the AI to avoid a target while in combat
/// backpedal with allow it to back away from the target if its to close and is useful for things like ranged AI
/// strafe allows for an AI to dodge left and right while in range of its target
/// </summary>
public class ME_Hunt : MonoBehaviour
{
    private MT_Hunt MT_HuntScript;

    private void Start()
    {
        MT_HuntScript = GetComponentInParent<MT_Hunt>();
    }

    public void Backpedal()//if the target is to close then set the new destination to a distance backward
    {
        MT_HuntScript.agent.speed = MT_HuntScript.backpedalSpeed;
        MT_HuntScript.agent.stoppingDistance = 0;
        MT_HuntScript.agent.SetDestination(transform.position - transform.forward * MT_HuntScript.backpedalMoveDistance);
    }

    public IEnumerator Strafe()//if in range then begin to strafe which will move left and right to avoid taking damage
    {
        MT_HuntScript.agent.speed = MT_HuntScript.strafeSpeed;
        MT_HuntScript.agent.stoppingDistance = 0;
        int randomLeftRight = Random.Range(0, 2);
        if(randomLeftRight == 0)
        {
            randomLeftRight = -1;
        }
        MT_HuntScript.agent.SetDestination(transform.position - transform.right * MT_HuntScript.backpedalMoveDistance * randomLeftRight);
        yield return new WaitForSeconds(MT_HuntScript.strafeCooldown);
        MT_HuntScript.strafeReady = true;
    }
}
