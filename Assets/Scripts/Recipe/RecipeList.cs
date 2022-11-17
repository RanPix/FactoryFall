using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe List", menuName = "Create Recipe list")]
public class RecipeList : ScriptableObject
{
    /*[SerializeField]*/ private Recipe[] recipes;

    public Recipe[] Recipes => recipes;
}