using System.Collections;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private int objectHP = 1;
    [SerializeField] private GameObject hitParticles;
    private bool destroyed = false;

    public void TakeDamage()
    {
        if (objectHP > 0)
        {
            Instantiate(hitParticles, transform.position, Quaternion.identity);
            objectHP--;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
