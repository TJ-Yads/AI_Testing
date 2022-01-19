using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// special attacks for ranged, these attacks work on a cooldown system and change how typical projectiles are fired
/// vollet will launch projectiles at a fast pace for a short time
/// burst will fire multiple projectiles all at once-- similar to buckshot
/// </summary>
public class SA_Ranged : MonoBehaviour
{
    private CS_Ranged csRangedScript;
    // Start is called before the first frame update
    void Start()
    {
        csRangedScript = GetComponentInParent<CS_Ranged>();
    }

    public IEnumerator VolleyAttack()//run a vollet loop to fire many projectiles in a short time
    {
        for (int i = 0; i != csRangedScript.volletCount; i++)
        {
            csRangedScript.Invoke(csRangedScript.fireTypeString, 0);
            yield return new WaitForSeconds(csRangedScript.volleyDelay);
        }
        csRangedScript.canFire = true;
        yield return new WaitForSeconds(csRangedScript.volleyCooldown);
        csRangedScript.volleyReady = true;
    }

    public IEnumerator BurstAttack()//run a burst loop to fire many projectiles all at once
    {
        for (int i = 0; i != csRangedScript.burstCount; i++)
        {
            csRangedScript.Invoke(csRangedScript.fireTypeString, 0);
        }
        csRangedScript.canFire = true;
        yield return new WaitForSeconds(csRangedScript.burstCooldown);
        csRangedScript.burstready = true;
    }
}
