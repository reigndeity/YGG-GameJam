using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MopObstacle : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.Translate(transform.forward * speed* Time.deltaTime, Space.World);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.layer == 3)
        {
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Border") || other.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
}
