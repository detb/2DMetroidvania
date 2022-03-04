using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime; 

    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        waitTime -= Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.S))
            waitTime = 0.2f;

        if (Input.GetKeyDown(KeyCode.S))
        {
            effector.rotationalOffset = 180f;
            waitTime = 0.2f;
        }

        if ((waitTime) < 0)
            effector.rotationalOffset = 0;

        if (Input.GetButtonDown("Jump"))
        {
            effector.rotationalOffset = 0;
            waitTime = 0.2f;
        }

    }
}
