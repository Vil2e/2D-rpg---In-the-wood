using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamera : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    Transform targetToFollow;

    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        targetToFollow = GameObject.FindAnyObjectByType<PlayerController>().transform;

        if(targetToFollow != null)
        {
            vcam.Follow = targetToFollow;
        }
        else
        {
            return;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
