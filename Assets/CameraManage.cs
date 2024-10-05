using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraManage : MonoBehaviour
{
    [SerializeField] private int _iterations;
    
    void Update()
    {
        var cam = Camera.main;
        for (int i = 0; i < _iterations; i++)
        {
            var result = cam;
        }
    }
    
}
