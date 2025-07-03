using System.Collections;
using UnityEngine;
using FMODUnity;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private int objectHP = 1;
    [SerializeField] private GameObject hitParticles;
    [SerializeField] private EventReference hitSound;
    private bool destroyed = false;

    public void TakeDamage()
    {
        if (objectHP > 0)
        {
            Instantiate(hitParticles, transform.position, Quaternion.identity);
            SFXManager.instance.PlayOneShot(hitSound, this.transform.position);
            objectHP--;
        }
        else
        {
            SFXManager.instance.PlayOneShot(hitSound, this.transform.position);
            Destroy(gameObject);
        }
    }
}
