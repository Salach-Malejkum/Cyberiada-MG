using UnityEngine;

public class PlayerDebug : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            transform.position = new Vector3(-10, 3, 0);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            transform.position = new Vector3(100, 19, 0);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            transform.position = new Vector3(46, 32, 0);
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            transform.position = new Vector3(146, 35, 0);
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            transform.position = new Vector3(241, 35, 0);
        }
    }
}
