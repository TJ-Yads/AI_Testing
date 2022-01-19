using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///Combat style script for ranged enemies
///controls the firing mechanics of enemies and allows for various special combat interactions based on other attached scripts
/// </summary>
public class CS_Ranged : MonoBehaviour
{
    //basic variables for all fire functions
    public float fireInXs;
    public float accuracy;
    public float baseRotationSpeed;
    private float rotationSpeed;
    public bool needLOS;
    public bool slowAimBeforeFire;
    public float slowDuration, slowRotationSpeed;

    //variables for damage interactions
    private DI_Base damageInteractions;

    //variables for firing type-- depending on which bool is true it will change how the enemy is able to fire
    public bool fire_MovingOBJ;
    public GameObject projectileOBJ;
    public float projectileLifeTime;

    public bool fire_Raycast;
    public float rayDistance;
    public GameObject trailOBJ;
    private RaycastHit hitResult;

    public bool fire_Dumb;

    //variables for controlling special attacks-- attacks include unique combat mechanics such as burst or volley of projectiles
    public bool useSpecialAttacks;
    public int specialChanceRoll;
    private SA_Ranged specialAttacksScript;

    public bool canVolley;
    public bool volleyReady = true;
    public float volleyDelay, volleyCooldown;
    public int volletCount;

    public bool canBurst;
    public bool burstready = true;
    public int burstCount;
    public float burstCooldown;

    //variables that is changed or modified by other scripts
    public bool canFire = true;
    public string fireTypeString;
    public Transform projectileSpawnPoint;
    public Transform aimTarget;
    public AIBase baseAI;

    private void Start()
    {
        rotationSpeed = baseRotationSpeed;
        if (useSpecialAttacks)
        {
            specialAttacksScript = GetComponentInChildren<SA_Ranged>();
        }
        damageInteractions = GetComponent<DI_Base>();
    }

    public void Fire()
    {
        if(useSpecialAttacks)//run a check and roll for a special attack
        {
            int roll = Random.Range(0, 101);
            if(roll <= specialChanceRoll)
            {
                StartSpecialAttack();
            }
        }
        if(canFire)//if the enemy can fire then start a basic fire function
        {
            if(slowAimBeforeFire)//cause rotation speed to slow if enabled
            {
                rotationSpeed = slowRotationSpeed;
                Invoke(fireTypeString, slowDuration);
                Invoke("ReturnBaseRotation", slowDuration + .1f);
            }
            else
            {
                Invoke(fireTypeString, 0);
            }
        }
    }

    /// <summary>
    /// this loop will run to allow the enemy to rotate towards the target and also find what projectile type it will use
    /// </summary>
    /// <returns></returns>
    public IEnumerator AimCheck()
    {
        if(fire_MovingOBJ)
        {
            fireTypeString = "FireMovingOBJ";
        }
        if (fire_Raycast)
        {
            fireTypeString = "FireRaycast";
        }
        if (fire_Dumb)
        {
            fireTypeString = "FireDumb";
        }
        while (true)
        {
            yield return new WaitForEndOfFrame();
            Vector3 targetDirection = aimTarget.position - transform.position;
            targetDirection = new Vector3(targetDirection.x, targetDirection.y, targetDirection.z);
            float singleStep = rotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }


    /// <summary>
    /// before firing this will check the accuracy of a projectile and change its behavior if its raycast or not
    /// </summary>
    /// <param name="isRaycast"></param>
    /// <returns></returns>
    Vector3 AccuracyCheck(bool isRaycast)
    {
        if(isRaycast)
        {
            float acX = (1 - 2 * Random.value) * accuracy / 100;
            float acY = (1 - 2 * Random.value) * accuracy / 100;
            float acZ = 1;
            return transform.TransformDirection(new Vector3(acX, acY, acZ));
        }
        else
        {
            float acX = Random.Range(-accuracy, accuracy);
            float acY = Random.Range(-accuracy, accuracy);
            float acZ = Random.Range(-accuracy, accuracy);
            return new Vector3(acX, acY, acZ);
        }
    }

    public void FireMovingOBJ()//fire a single projectile that moves in the world towards the target
    {
        Vector3 accuracyAngle = AccuracyCheck(false);

        GameObject projOBJ = Instantiate(projectileOBJ, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation * Quaternion.Euler(accuracyAngle));
        ProjectileData projData = projOBJ.GetComponent<ProjectileData>();
        projData.damageInteractions = damageInteractions;
        projData.DMGValue = baseAI.baseDMG;
        Destroy(projOBJ, projectileLifeTime);
    }

    public void FireRaycast()//fire a raycast from self to target
    {
        Vector3 accuracyAngle = AccuracyCheck(true);
        Ray ray = new Ray(projectileSpawnPoint.position, accuracyAngle);
        if (Physics.Raycast(projectileSpawnPoint.position, accuracyAngle, out hitResult, rayDistance))
        {   
            GameObject traOBJ = Instantiate(trailOBJ, projectileSpawnPoint.position, transform.rotation);
            traOBJ.GetComponent<ProjectileMover>().targetTransform = ray.direction * hitResult.distance;
            Destroy(traOBJ, 2f);
            Debug.Log(hitResult.collider.tag);
        }
    }
    public void FireDumb()//fire a projectile outward with no regard for aiming
    {
        Vector3 accuracyAngle = AccuracyCheck(false);

        GameObject projOBJ = Instantiate(projectileOBJ, projectileSpawnPoint.transform.position, transform.rotation * Quaternion.Euler(accuracyAngle));
        ProjectileData projData = projOBJ.GetComponent<ProjectileData>();
        projData.DMGValue = baseAI.baseDMG;
        Destroy(projOBJ, projectileLifeTime);
    }

    private void ReturnBaseRotation()//small function that forces the rotation speed to normal whenever it needs to be reset
    {
        rotationSpeed = baseRotationSpeed;
    }

    private void StartSpecialAttack()//run various special attacks if available and a previous roll was succesful
    {
        if (canVolley && volleyReady)
        {
            canFire = false;
            volleyReady = false;
            StartCoroutine(specialAttacksScript.VolleyAttack());
            return;
        }
        if (canBurst && burstready)
        {
            canFire = false;
            burstready = false;
            StartCoroutine(specialAttacksScript.BurstAttack());
            return;
        }
    }
}
