using UnityEngine;
using System;

public class GroundedManager : MonoBehaviour
{
    private const int GROUND_LAYER = 3;
    public event Action<bool> OnIsGroundedChanged;
    private bool isGrouded;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GROUND_LAYER)
        {
            isGrouded = true;
            OnIsGroundedChanged?.Invoke(isGrouded);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == GROUND_LAYER)
        {
            isGrouded = false;
            OnIsGroundedChanged?.Invoke(isGrouded);
        }
    }
}
