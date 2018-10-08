using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAnim : MonoBehaviour {

    [SerializeField]
    private LevelManager levelManager;


    public void LaunchTransition()
    {
        levelManager.LaunchTransition();
    }
}
