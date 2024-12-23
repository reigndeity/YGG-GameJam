using System.Collections;
using UnityEngine;

public class IngredientBehavior : MonoBehaviour
{
    private const float timeBeforeDestroyed = 15f;
    private const string IngredientCheckerTag = "Ingredient_Checker";

    private float timer;
    public bool isGrabbed;
    private MeshCollider _meshCollider;
    private Renderer _renderer;
    private Coroutine blinkingCoroutine;

    [Header("ONLY FOR THE BURGER BUNS")]
    // temp fix:
    public bool isBurgerBuns;
    public Renderer[] _burgerBunRenderers;

    private void Start()
    {
        timer = timeBeforeDestroyed;
        _meshCollider = GetComponent<MeshCollider>();
        _renderer = GetComponent<Renderer>();

        _meshCollider.enabled = false;
        StartCoroutine(EnableColliderAfterDelay(1f));
    }

    private void Update()
    {
        if (!isGrabbed && timer > 0)
        {
            timer -= Time.deltaTime;

            // Start blinking when timer reaches 5 seconds
            if (timer <= 5f && blinkingCoroutine == null)
            {
                blinkingCoroutine = StartCoroutine(BlinkEffect());
            }
        }
        else if (isGrabbed)
        {
            timer = timeBeforeDestroyed;

            // Stop blinking if grabbed
            if (blinkingCoroutine != null)
            {
                StopCoroutine(blinkingCoroutine);
                blinkingCoroutine = null;
                SetVisible(true); // Ensure object stays visible
            }
        }

        if (timer <= 0)
        {
            Destroy(gameObject);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(IngredientCheckerTag))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _meshCollider.enabled = true;
    }

    private IEnumerator BlinkEffect()
    {
        while (timer > 0)
        {
            // If we're in the last 3 seconds, reduce the blink time to make it faster
            float blinkSpeed = (timer <= 3f) ? 0.1f : 0.25f; // Blink faster in the last 3 seconds

            SetVisible(false); // Make object invisible
            yield return new WaitForSeconds(blinkSpeed); // Wait for the adjusted blink speed

            SetVisible(true); // Make object visible
            yield return new WaitForSeconds(blinkSpeed); // Wait for the adjusted blink speed
        }
    }

    private void SetVisible(bool isVisible)
    {
        if (_renderer != null)
        {
            _renderer.enabled = isVisible;

            // Temp Fix
            if (isBurgerBuns == true) 
            {
                _burgerBunRenderers[0].enabled = isVisible;
                _burgerBunRenderers[1].enabled = isVisible;
                _burgerBunRenderers[2].enabled = isVisible;
            }
        }
    }
}
