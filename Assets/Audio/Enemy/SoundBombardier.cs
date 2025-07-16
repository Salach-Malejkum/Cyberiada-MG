using UnityEngine;
using FMODUnity;

public class SoundBombardier : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private EventReference walk_1;
    [SerializeField] private EventReference walk_2;
    [SerializeField] private EventReference walk_3;
    [Header("Atk")]
    [SerializeField] private EventReference atk_1;


    public void PlayWalk1() { SFXManager.instance.PlayOneShot(walk_1, this.transform.position); }

    public void PlayWalk2() { SFXManager.instance.PlayOneShot(walk_2, this.transform.position); }

    public void PlayWalk3() { SFXManager.instance.PlayOneShot(walk_3, this.transform.position); }

    public void PlayAtk1() { SFXManager.instance.PlayOneShot(atk_1, this.transform.position); }
}
