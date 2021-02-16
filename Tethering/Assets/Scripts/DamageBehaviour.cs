using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehaviour : MonoBehaviour
{
    [SerializeField]
    private int _damageAmount = 40;

    [SerializeField]
    private GameObject SpawnOnDamage;

    [SerializeField]
    private GameObject SpawnOnHit;

    private void OnCollisionEnter(Collision collision)
    {
        OnHit(collision.transform);
    }

    private void OnTriggerEnter(Collider collider)
    {
        OnHit(collider.transform);
    }

    private void OnHit(Transform transform)
    {
        var health = transform.GetComponent<HealthBehaviour>();
        if (health != null)
        {
            health.Damage(_damageAmount, out bool destroyOrKill);
            if (SpawnOnDamage != null)
                Instantiate(SpawnOnDamage, transform.position, transform.rotation);
        }
        if (SpawnOnHit != null)
            Instantiate(SpawnOnHit, transform.position, transform.rotation);
    }
}
