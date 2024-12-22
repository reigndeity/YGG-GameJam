using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MopObstacle : MonoBehaviour
{
    public float speed;
    [SerializeField] ParticleSystem poofEffect;
    [SerializeField] GameObject mopObj;
    private BoxCollider _boxCollider;
    public bool canMove;
    public Rigidbody rb;
    public Animator animator;

    [SerializeField] AudioManager _audioManager;
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        Invoke("StartMoving", 2f);
        _audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if(canMove) transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }
    private void OnCollisionEnter(Collision other)
    {
        
            int randomNumber = Random.Range(0, 3);
            if (other.gameObject.CompareTag("Player") || other.gameObject.layer == 3)
            {
                poofEffect.Play();
                Destroy(other.gameObject);
                if (other.gameObject.CompareTag("Player")) _audioManager.PlayPlayerDeathSound();
            }
            else if (other.gameObject.CompareTag("Border") || other.gameObject.layer == 6 || other.gameObject.CompareTag("Mop"))
            {
                poofEffect.Play();
                mopObj.SetActive(false);
                _boxCollider.enabled = false;
                Invoke("DestroyMop", 1.5f);
            }
             
    }

    public void StartMoving()
    {
        canMove = true;
        _boxCollider.enabled = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.useGravity = false;
    }
    public void DestroyMop()
    {
        Destroy(this.gameObject);
    }
}
