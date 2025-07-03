using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Camera Movement")]
    [SerializeField] private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [Header("Vertical Look")]
    [SerializeField] private float lookOffset = 2f;
    [SerializeField] private float lookSmoothTime = 0.2f;
    [SerializeField] private float minLookY = -2f;
    [SerializeField] private float maxLookY = 2f;

    private float lookInputY = 0f;
    private float currentLookY = 0f;
    private float lookVelocity = 0f;

    private bool isPlayerStationary = true;

    void FixedUpdate()
    {
        FollowPlayer();
        motionDetection();
        HandleVerticalLook();
    }

    void motionDetection()
    {
        isPlayerStationary = playerRb.linearVelocity.sqrMagnitude < 0.01f;
    }

    void FollowPlayer()
    {
        Vector3 targetPosition = player.position + offset;
        targetPosition.y += currentLookY;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void HandleVerticalLook()
    {
        if (isPlayerStationary && lookInputY != 0)
        {
            float targetLookY = Mathf.Clamp(lookInputY * lookOffset, minLookY, maxLookY);
            currentLookY = Mathf.SmoothDamp(currentLookY, targetLookY, ref lookVelocity, lookSmoothTime);
        }
        else
        {
            currentLookY = Mathf.SmoothDamp(currentLookY, 0, ref lookVelocity, lookSmoothTime);
        }

    }

    void OnLook(InputValue value)
    {
        lookInputY = value.Get<float>();
        SendMessage("HandleLookInput", lookInputY, SendMessageOptions.DontRequireReceiver);
    }
}
