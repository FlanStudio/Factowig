using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamReferencer : MonoBehaviour
{
    public static VCamReferencer Instance = null;

    public CinemachineVirtualCamera vcam1 = null;
    public CinemachineVirtualCamera vcam2 = null;

    private void Awake()
    {
        Instance = this;
    }
}
