using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private static bool spawned = false;
    void Awake(){
        DontDestroyOnLoad (this);
        if(spawned)     
            Destroy(gameObject);
        else        
            spawned = true;
    }
}
