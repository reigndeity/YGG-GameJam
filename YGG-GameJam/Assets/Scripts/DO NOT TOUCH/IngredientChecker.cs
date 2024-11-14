using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientChecker : MonoBehaviour
{
    [Header("Ingredient Checker Properties")]
    public int recipeType; //0:Burger | 1:Sandwich | 2:Hotdogs
    public string[] ingredientsNeeded;
    [SerializeField] GameObject[] ingredientObjs;
    public bool ingredientOne;
    public bool ingredientTwo;
    public bool ingredientThree;

    [Header("Visual ")]
    [SerializeField] float rotationSpeed;

    void Update()
    {
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
        }

        // IF INGREDIENTS ARE COMPLETE
        if (ingredientOne == true && ingredientTwo == true && ingredientThree == true)
        {
            ResetIngredientBools();
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
        }
        if (other.gameObject.tag == ingredientsNeeded[1])
        {
            ingredientTwo = true;
        }
        if (other.gameObject.tag == ingredientsNeeded[2])
        {
            ingredientThree = true;
        }
    }

    void ResetIngredientBools()
    {
        ingredientOne = false;
        ingredientTwo = false;
        ingredientThree = false;
        Invoke("DeactivateIngredients", 1f);
    }
    void DeactivateIngredients()
    {
        ingredientObjs[0].SetActive(false);
        ingredientObjs[1].SetActive(false);
        ingredientObjs[2].SetActive(false);
    }
}
