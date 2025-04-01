using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientChecker : MonoBehaviour
{
    [Header("Player Kitchen Properties")]
    public int playerKitchen; // 1:PlayerOne | 2:PlayerTwo | 3:PlayerThree | 4:PlayerFour

    [Header("Ingredient Checker Properties")]
    public int recipeType; // 0:Burger | 1:Sandwich | 2:Hotdogs
    public string[] ingredientsNeeded;
    [SerializeField] GameObject[] ingredientObjs;
    public bool ingredientOne;
    public bool ingredientTwo;
    public bool ingredientThree;
    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();

    [Header("Visual & Audio")]
    [SerializeField] float rotationSpeed;
    public GameObject coverObject;
    public ParticleSystem coverParticle;
    [SerializeField] AudioManager _audioManager;
    void Start()
    {
        recipeType = GameManager.instance.recipeChosen;
        _audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        recipeType = GameManager.instance.recipeChosen;
        switch (recipeType)
        {
            case 0:
                ingredientsNeeded[0] = "Burger_Buns";
                ingredientsNeeded[1] = "Burger_Cheese";
                ingredientsNeeded[2] = "Burger_Patty";
                if (ingredientOne == true)
                    {
                        ingredientObjs[0].SetActive(true);
                    }
                    if (ingredientTwo == true)
                    {
                        ingredientObjs[1].SetActive(true);
                    }
                    if (ingredientThree == true)
                    {
                        ingredientObjs[2].SetActive(true);
                    }
                break;
            case 1:
                ingredientsNeeded[0] = "Hotdog_Buns";
                ingredientsNeeded[1] = "Hotdog_Mustard";
                ingredientsNeeded[2] = "Hotdog_Sausage";
                if (ingredientOne == true)
                    {
                        ingredientObjs[3].SetActive(true);
                    }
                    if (ingredientTwo == true)
                    {
                        ingredientObjs[4].SetActive(true);
                    }
                    if (ingredientThree == true)
                    {
                        ingredientObjs[5].SetActive(true);
                    }
                break;
            case 2:
                ingredientsNeeded[0] = "Sandwich_Bread";
                ingredientsNeeded[1] = "Sandwich_Ham";
                ingredientsNeeded[2] = "Sandwich_Lettuce";
                if (ingredientOne == true)
                    {
                        ingredientObjs[6].SetActive(true);
                    }
                    if (ingredientTwo == true)
                    {
                        ingredientObjs[7].SetActive(true);
                    }
                    if (ingredientThree == true)
                    {
                        ingredientObjs[8].SetActive(true);
                    }
                break;    
        }

        // IF INGREDIENTS ARE COMPLETE
        switch (playerKitchen)
        {
            case 1:
                if (ingredientOne == true && ingredientTwo == true && ingredientThree == true)
                {
                    ResetIngredientBools();
                    GameManager.instance.playerOneScore++;
                }
                break;
            case 2:
                if (ingredientOne == true && ingredientTwo == true && ingredientThree == true)
                {
                    ResetIngredientBools();
                    GameManager.instance.playerTwoScore++;
                }
                break;
            case 3:
                if (ingredientOne == true && ingredientTwo == true && ingredientThree == true)
                {
                    ResetIngredientBools();
                    GameManager.instance.playerThreeScore++;
                }
                break;
            case 4:
                if (ingredientOne == true && ingredientTwo == true && ingredientThree == true)
                {
                    ResetIngredientBools();
                    GameManager.instance.playerFourScore++;
                }
                break;
        }
        

        // VISUAL EFFECT
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationAmount, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ingredientsNeeded[0])
        {
            ingredientOne = true;
            _audioManager.PlayAddedItemSound();
        }
        if (other.gameObject.tag == ingredientsNeeded[1])
        {
            ingredientTwo = true;
            _audioManager.PlayAddedItemSound();
        }
        if (other.gameObject.tag == ingredientsNeeded[2])
        {
            ingredientThree = true;
            _audioManager.PlayAddedItemSound();
        }
    }

    void ResetIngredientBools()
    {
        ingredientOne = false;
        ingredientTwo = false;
        ingredientThree = false;
        coverObject.SetActive(true);
        _audioManager.PlayCompletedItemSound();
        Vector3 expandScale = new Vector3(1.3f, 1.3f, 1.3f); // Scale to expand to
        Vector3 shrinkScale = new Vector3(0.1f, 0.1f, 0.1f); // Scale to shrink to
        ExpandShrinkRestoreIngredients(0.3f, expandScale, 0.2f, 0.3f, shrinkScale, 0.2f);
        Invoke("DeactivateIngredients", 0.8f);
    }
    void DeactivateIngredients()
    {
        for (int i = 0; i <= 8; i++)
        {
            
            ingredientObjs[i].SetActive(false);
            StartCoroutine(RemoveCover());
        }
    }

    public IEnumerator RemoveCover()
    {
        yield return new WaitForSeconds(1f);
        coverParticle.Play();
        coverObject.SetActive(false);
    }
    public void ExpandShrinkRestoreIngredients(float expandTime, Vector3 expandScale, float airTime, float shrinkTime, Vector3 shrinkScale, float restoreTime)
    {
        StartCoroutine(ExpandShrinkRestoreCoroutine(expandTime, expandScale, airTime, shrinkTime, shrinkScale, restoreTime));
    }
    private IEnumerator ExpandShrinkRestoreCoroutine(float expandTime, Vector3 expandScale, float airTime, float shrinkTime, Vector3 shrinkScale, float restoreTime)
    {
        float elapsedTime = 0;

        // Track the original scale of the ingredients before expanding
        Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();
        foreach (GameObject ingredient in ingredientObjs)
        {
            originalScales[ingredient] = ingredient.transform.localScale;
        }

        // Expand the ingredients to the specified expandScale
        while (elapsedTime < expandTime)
        {
            float scaleFactor = elapsedTime / expandTime; // Gradually increase scale
            foreach (GameObject ingredient in ingredientObjs)
            {
                ingredient.transform.localScale = Vector3.Lerp(originalScales[ingredient], expandScale, scaleFactor);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure they are fully expanded to expandScale
        foreach (GameObject ingredient in ingredientObjs)
        {
            ingredient.transform.localScale = expandScale;
        }

        // Wait for the air time (optional pause after expanding)
        yield return new WaitForSeconds(airTime);

        // Shrink the ingredients from expandScale to the specified shrinkScale
        elapsedTime = 0;
        while (elapsedTime < shrinkTime)
        {
            float scaleFactor = elapsedTime / shrinkTime; // Gradually reduce scale
            foreach (GameObject ingredient in ingredientObjs)
            {
                ingredient.transform.localScale = Vector3.Lerp(expandScale, shrinkScale, scaleFactor);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure they are fully shrunk to shrinkScale
        foreach (GameObject ingredient in ingredientObjs)
        {
            ingredient.transform.localScale = shrinkScale;
        }

        // Wait for the air time (optional pause after shrinking before restoring)
        yield return new WaitForSeconds(airTime);

        // Restore the ingredients to their original scale after shrinking
        foreach (GameObject ingredient in ingredientObjs)
        {
            StartCoroutine(RestoreScale(ingredient, restoreTime, originalScales[ingredient]));
        }
    }

    private IEnumerator RestoreScale(GameObject ingredient, float time, Vector3 originalScale)
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            ingredient.transform.localScale = Vector3.Lerp(ingredient.transform.localScale, originalScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure they are fully restored to original scale
        ingredient.transform.localScale = originalScale;
    }
}
