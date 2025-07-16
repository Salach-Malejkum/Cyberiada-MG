using UnityEngine;
using FMODUnity;

public class SoundPlayer : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private EventReference walk_1;
    [SerializeField] private EventReference walk_2;
    [SerializeField] private EventReference walk_3;
    [Header("Atk")]
    [SerializeField] private EventReference atk_1;
    [SerializeField] private EventReference atk_2;
    [SerializeField] private EventReference r_atk_1;
    [Header("Run")]
    [SerializeField] private EventReference run_1;
    [SerializeField] private EventReference run_2;
    [SerializeField] private EventReference run_3;
    [Header("Jump")]
    [SerializeField] private EventReference jump;
    [SerializeField] private EventReference land;


    public void PlayWalk1() { SFXManager.instance.PlayOneShot(walk_1, this.transform.position); }

    public void PlayWalk2() { SFXManager.instance.PlayOneShot(walk_2, this.transform.position); }

    public void PlayWalk3() { SFXManager.instance.PlayOneShot(walk_3, this.transform.position); }

    public void PlayAtk1() { SFXManager.instance.PlayOneShot(atk_1, this.transform.position); }

    public void PlayAtk2() { SFXManager.instance.PlayOneShot(atk_2, this.transform.position); }

    public void PlayRAtk() { SFXManager.instance.PlayOneShot(r_atk_1, this.transform.position); }

    public void PlayRun1() { SFXManager.instance.PlayOneShot(run_1, this.transform.position); }

    public void PlayRun2() { SFXManager.instance.PlayOneShot(run_2, this.transform.position); }

    public void PlayRun3() { SFXManager.instance.PlayOneShot(run_3, this.transform.position); }

    public void PlayJump() { SFXManager.instance.PlayOneShot(jump, this.transform.position); }

    public void PlayLand() { SFXManager.instance.PlayOneShot(land, this.transform.position); }
}
