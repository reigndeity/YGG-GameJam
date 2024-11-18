using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MopObstacle : MonoBehaviour
{
    public float speed;
    [SerializeField] ParticleSystem poofEffect;
    [SerializeField] GameObject mopObj;
    private BoxCollider _boxCollider;
    public AudioSource destroyPlayer;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

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
            if (other.gameObject.CompareTag("Player")) destroyPlayer.Play();
        }
        else if (other.gameObject.CompareTag("Border") || other.gameObject.CompareTag("Ground"))
        {
            poofEffect.Play();
            mopObj.SetActive(false);
            _boxCollider.enabled = false;
            Invoke("DestroyMop",1.5f);
        }
    }

    public void DestroyMop()
    {
        Destroy(this.gameObject);
    }
}
