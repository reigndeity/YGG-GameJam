using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MopObstacle : MonoBehaviour
{
    public float speed;
    private IngrSpawner spawner;

    private void Start()
    {
        spawner = FindObjectOfType<IngrSpawner>();
    }

    private void Update()
    {
        transform.Translate(transform.forward * speed* Time.deltaTime, Space.World);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.layer == 3)
        {
            Destroy(other.gameObject);
            if (other.gameObject.layer == 3)
            {
                spawner.ingrCount--;
            }
        }
        else if (other.gameObject.CompareTag("Border") || other.gameObject.CompareTag("Mop"))
        {
            Destroy(this.gameObject);
        }
    }
}
