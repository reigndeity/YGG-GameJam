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

    [Header("Visual & Audio")]
    [SerializeField] float rotationSpeed;
    public GameObject coverObject;
    public ParticleSystem coverParticle;
    public AudioSource sfxSource;
    public AudioClip[] sfxClip; // 0 = Add Item, 1 = Complete Item

    void Start()
    {
        sfxSource.volume = 0.25f;
        recipeType = GameManager.instance.recipeChosen;
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
            sfxSource.clip = sfxClip[0];
            sfxSource.Play();
        }
        if (other.gameObject.tag == ingredientsNeeded[1])
        {
            ingredientTwo = true;
            sfxSource.clip = sfxClip[0];
            sfxSource.Play();
        }
        if (other.gameObject.tag == ingredientsNeeded[2])
        {
            ingredientThree = true;
            sfxSource.clip = sfxClip[0];
            sfxSource.Play();
        }
    }

    void ResetIngredientBools()
    {
        ingredientOne = false;
        ingredientTwo = false;
        ingredientThree = false;
        coverObject.SetActive(true);
        sfxSource.clip = sfxClip[1];
        sfxSource.Play();
        Invoke("DeactivateIngredients", .8f);
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
}
