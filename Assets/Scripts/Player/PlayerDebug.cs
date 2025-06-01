using UnityEngine;

#if UNITY_EDITOR || DEBUG
public class PlayerDebug : MonoBehaviour
{
    [SerializeField]
    private Vector3[] teleportPositions = {
        // new Vector3(-10, 3, 0),
        // new Vector3(100, 19, 0),
        // new Vector3(46, 32, 0),
        // new Vector3(146, 35, 0),
        // new Vector3(241, 35, 0)
    };

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            transform.position = teleportPositions[0];
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            transform.position = teleportPositions[1];
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            transform.position = teleportPositions[2];
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            transform.position = teleportPositions[3];
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            transform.position = teleportPositions[4];
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            transform.position = teleportPositions[5];
        }
    }
}

#else
{

}
#endif