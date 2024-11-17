using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MopObstacle : MonoBehaviour
{
    public float speed;
    [SerializeField] ParticleSystem poofEffect;

    private void Update()
    {
        transform.Translate(transform.forward * speed* Time.deltaTime, Space.World);
    }
    private void OnCollisionEnter(Collision other)
    {
        int randomNumber = Random.Range(0, 3);
        if (other.gameObject.CompareTag("Player") || other.gameObject.layer == 3)
        {
            poofEffect.Play();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Border") || other.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
}
