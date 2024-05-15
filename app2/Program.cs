using System;
using System.Collections.Generic;
using System.Linq;

delegate void RecipeCaloriesExceededHandler(double totalCalories); // Delegate for notifying when calories exceed a certain value

class Ingredient
{
    public string Name; // Name of the ingredient
    public double Quantity; // Quantity of the ingredient
    public string Unit; // Unit of measurement for the quantity
    public int Calories; // Number of calories in the ingredient
    public string FoodGroup; // Food group the ingredient belongs to
    public double OriginalQuantity; // Original quantity of the ingredient

    // Constructor to initialize an ingredient with all its properties
    public Ingredient(string name, double quantity, string unit, int calories, string foodGroup)
    {
        Name = name;
        Quantity = quantity;
        Unit = unit;
        Calories = calories;
        FoodGroup = foodGroup;
        OriginalQuantity = quantity; // Store the original quantity
    }

    // Method to reset the quantity of the ingredient
    public void ResetQuantity()
    {
        Quantity = OriginalQuantity; // Reset to the original quantity
    }
}

// Class representing a recipe with its properties and methods
class Recipe
{
    public string Name; // Name of the recipe
    public List<Ingredient> Ingredients; // List of ingredients in the recipe
    public List<string> Steps; // List of steps to prepare the recipe

    public event RecipeCaloriesExceededHandler CaloriesExceeded; // Event to notify when calories exceed a certain value

    // Constructor to initialize a recipe with its name
    public Recipe(string name)
    {
        Name = name;
        Ingredients = new List<Ingredient>(); // Initialize the list of ingredients
        Steps = new List<string>(); // Initialize the list of steps
    }

    // Method to add an ingredient to the recipe
    public void AddIngredient(Ingredient ingredient)
    {
        Ingredients.Add(ingredient);
    }

    // Method to add a step to the recipe
    public void AddStep(string step)
    {
        Steps.Add(step);
    }

    // Method to print the recipe details
    public void PrintRecipe()
    {
        Console.WriteLine("Recipe: {0}", Name);
        Console.WriteLine("Ingredients:");
        foreach (Ingredient ingredient in Ingredients)
        {
            Console.WriteLine("{0} {1} {2}", ingredient.Quantity, ingredient.Unit, ingredient.Name);
        }

        Console.WriteLine("\nSteps:");
        for (int i = 0; i < Steps.Count; i++)
        {
            Console.WriteLine("{0}. {1}", i + 1, Steps[i]);
        }

        double totalCalories = Ingredients.Sum(ingredient => ingredient.Calories);
        Console.WriteLine("Total Calories: {0}", totalCalories);

        if (totalCalories > 300)
        {
            CaloriesExceeded?.Invoke(totalCalories); // Trigger the event if calories exceed 300
        }
    }

    // Method to scale the quantities of the ingredients in the recipe
    public void ScaleRecipe(double factor)
    {
        foreach (Ingredient ingredient in Ingredients)
        {
            ingredient.Quantity *= factor;
        }
    }

    // Method to reset the quantities of the ingredients in the recipe
    public void ResetQuantities()
    {
        foreach (Ingredient ingredient in Ingredients)
        {
            ingredient.ResetQuantity();
        }
    }

    // Method to clear the ingredients and steps in the recipe
    public void ClearRecipe()
    {
        Ingredients.Clear(); // Clear the list of ingredients
        Steps.Clear(); // Clear the list of steps
    }
}

// Main program class
class Program
{
    static void Main(string[] args)
    {
        List<Recipe> recipes = new List<Recipe>(); // Generic collection to store recipes

        while (true)
        {
            Console.WriteLine("\nRecipe Creator:");
            Console.WriteLine("1. Add Recipe");
            Console.WriteLine("2. Select Recipe");
            Console.WriteLine("3. Exit");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Enter recipe name: ");
                    string recipeName = Console.ReadLine();
                    Recipe recipe = new Recipe(recipeName);
                    recipe.CaloriesExceeded += Recipe_CaloriesExceeded; // Subscribe to the CaloriesExceeded event
                    AddIngredientsAndSteps(recipe);
                    recipes.Add(recipe);
                    break;

                case 2:
                    if (recipes.Count == 0)
                    {
                        Console.WriteLine("No recipes available.");
                        break;
                    }

                    // Sort recipes alphabetically by name
                    var sortedRecipes = recipes.OrderBy(r => r.Name).ToList();

                    Console.WriteLine("Select a recipe:");
                    for (int i = 0; i < sortedRecipes.Count; i++)
                    {
                        Console.WriteLine("{0}. {1}", i + 1, sortedRecipes[i].Name);
                    }

                    int recipeChoice = int.Parse(Console.ReadLine());
                    if (recipeChoice >= 1 && recipeChoice <= sortedRecipes.Count)
                    {
                        Recipe selectedRecipe = sortedRecipes[recipeChoice - 1];
                        selectedRecipe.PrintRecipe();
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please try again.");
                    }
                    break;

                case 3:
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    // Method to handle the CaloriesExceeded event
    static void Recipe_CaloriesExceeded(double totalCalories)
    {
        Console.WriteLine("WARNING: This recipe exceeds 300 calories with a total of {0} calories!", totalCalories);
    }

    // Method to add ingredients and steps to a recipe
    static void AddIngredientsAndSteps(Recipe recipe)
    {
        while (true)
        {
            Console.WriteLine("\nAdd to Recipe:");
            Console.WriteLine("1. Add Ingredients");
            Console.WriteLine("2. Add Step");
            Console.WriteLine("3. Print Recipe");
            Console.WriteLine("4. Scale Recipe");
            Console.WriteLine("5. Reset Quantities");
            Console.WriteLine("6. Clear Recipe");
            Console.WriteLine("7. Return to Recipe Creator");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Enter ingredient name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter quantity: ");
                    double quantity = double.Parse(Console.ReadLine());
                    Console.Write("Enter unit of measurement: ");
                    string unit = Console.ReadLine();
                    Console.Write("Enter calories: ");
                    int calories = int.Parse(Console.ReadLine());
                    Console.Write("Enter food group: ");
                    string foodGroup = Console.ReadLine();

                    Ingredient ingredient = new Ingredient(name, quantity, unit, calories, foodGroup);
                    recipe.AddIngredient(ingredient);
                    break;

                case 2:
                    Console.Write("Enter step description: ");
                    string step = Console.ReadLine();
                    recipe.AddStep(step);
                    break;

                case 3:
                    recipe.PrintRecipe();
                    break;

                case 4:
                    Console.Write("Enter scaling factor: ");
                    double factor = double.Parse(Console.ReadLine());
                    recipe.ScaleRecipe(factor);
                    break;

                case 5:
                    recipe.ResetQuantities();
                    break;

                case 6:
                    recipe.ClearRecipe();
                    break;

                case 7:
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}


