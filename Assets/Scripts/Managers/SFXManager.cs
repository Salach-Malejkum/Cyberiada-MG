using UnityEngine;
using FMODUnity;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void PlayOneShot(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }
}
