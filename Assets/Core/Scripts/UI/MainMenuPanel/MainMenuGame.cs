using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGame : MonoBehaviour
{
    public static float cycleTime = 0.4f;
    private bool down;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(down)
        {
            cycleTime -= Time.deltaTime;
            if (cycleTime < 0f)
            {
                cycleTime = 0f;
                down = false;
            }
        }
        else
        {
            cycleTime += Time.deltaTime;
            if (cycleTime > 0.4f)
            {
                cycleTime = 0.4f;
                down = true;
            }
        }
    }
}
