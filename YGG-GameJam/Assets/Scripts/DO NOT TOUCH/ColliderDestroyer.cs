using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDestroyer : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {      
        Debug.Log("DESTROYED!");  
        if (other.gameObject.tag == "Ground" || other.gameObject.layer == 3)
        {
            Destroy(gameObject);
        }
    }
    
}
