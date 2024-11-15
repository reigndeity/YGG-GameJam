using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientBehavior : MonoBehaviour
{
    [SerializeField] float timeBeforeDestroyed;
    [SerializeField] float timer;           
    public bool isGrabbed;       

    private void Start()
    {
        timeBeforeDestroyed = 15;
        timer = timeBeforeDestroyed;
    }

    void Update()
    {
        if (timer > 0 && isGrabbed == false)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
        if (isGrabbed == true)
        {
            timer = timeBeforeDestroyed;
        }
    }

    public void OnGrabbed()
    {
        isGrabbed = true;
    }

    public void OnReleased()
    {
        Debug.Log("Timer Reset");
        isGrabbed = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ingredient_Checker")
        {
            Destroy(gameObject);
        }
    }
}
