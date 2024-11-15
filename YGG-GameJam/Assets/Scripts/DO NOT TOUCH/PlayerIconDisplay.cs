using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIconDisplay : MonoBehaviour
{
    [SerializeField] Camera cameraMain;
    [SerializeField] Transform target;


    void Awake()
    {
        cameraMain = FindObjectOfType<Camera>();
    }

    void Update()
    {
        transform.rotation = cameraMain.transform.rotation;
    }
}
