using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// the hunt movement type will allow an AI to track and move towards a target
/// this also allows for the maneuverer type to avoid the target while in range
/// </summary>
public class MT_Hunt : MonoBehaviour
{
    public float stopDistance, startMovingRange;
    public float moveSpeed;
    private Transform targetPosition;
    private GameObject playerOBJ;

    public bool canUseMoveExtras;
    private ME_Hunt ME_HuntScript;
    private bool usingExtraMove;

    public bool canBackpedal;
    public float backpedalMoveDistance;
    public float backpedalDistance;
    public float backpedalSpeed;

    public bool canStrafe;
    public float strafeDistance;
    public float strafeSpeed;
    public float strafeCooldown;
    public bool strafeReady = true;

    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        playerOBJ = GameObject.FindGameObjectWithTag("Player");
        targetPosition = playerOBJ.transform;
        agent = GetComponent<NavMeshAgent>();
        if(canUseMoveExtras)
        {
            ME_HuntScript = GetComponentInChildren<ME_Hunt>();
        }
    }

    public IEnumerator RangeCheck()//run a range check to decide if the AI needs to move or not
    {
        yield return new WaitForSeconds(1f);

        var distance = Vector3.Distance(transform.position, targetPosition.transform.position);
        while (true)
        {
            distance = Vector3.Distance(transform.position, targetPosition.transform.position);
            if (distance <= startMovingRange)
            {
                agent.stoppingDistance = stopDistance;
                agent.SetDestination(targetPosition.position);
                agent.speed = moveSpeed;
            }
            else
            {
                agent.stoppingDistance = 0;
                agent.SetDestination(transform.position);
                agent.speed = 0;
            }
            if (canUseMoveExtras)
            {
                usingExtraMove = true;
                StartCoroutine(UseMoveExtra());
            }
            yield return new WaitForEndOfFrame();
            yield return new WaitWhile(() => usingExtraMove);
        }
    }

    public IEnumerator UseMoveExtra()//if extra movement types are available then use them and prevent typical movement while in use
    {
        var distance = Vector3.Distance(transform.position, targetPosition.transform.position);
        if (canBackpedal && distance <= backpedalDistance)
        {
            ME_HuntScript.Backpedal();
            yield return new WaitForSeconds(.5f);
            usingExtraMove = false;
            StopCoroutine(UseMoveExtra());
        }
        if (canStrafe && strafeReady && agent.velocity == new Vector3(0,0,0))
        {
            strafeReady = false;
            StartCoroutine(ME_HuntScript.Strafe());
            yield return new WaitForSeconds(.5f);
            usingExtraMove = false;
            StopCoroutine(UseMoveExtra());
        }
        usingExtraMove = false;
    }
}
