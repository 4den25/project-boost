using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuittingApplication : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit Application");
            Application.Quit();
        }
    }
}
