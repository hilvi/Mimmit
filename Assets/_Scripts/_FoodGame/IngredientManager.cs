using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientManager : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject bananaPrefab;
    public GameObject pearPrefab;

    //private List<GameObject> ingredientList = new List<GameObject> ();

    void Start ()
    {
        //Quaternion angle = Quaternion.identity;
        Vector3 anchor = transform.position;

        // Spawns food randomly for testing
        for (int y = 0; y < 5; y++) {
            for (int x = 0; x < 5; x++) {
                // Calculate new positions
                var t = new Vector3 (anchor.x, anchor.y, anchor.z);
                t.x += x * 1f;
                t.y -= y * 1f;

                // Randomly choose a fruit
                int choice = Random.Range (0, 3);
                switch (choice) {
                case 0:
                    SpawnFood (FoodType.Apple, t);
                    break;
                case 1:
                    SpawnFood (FoodType.Banana, t);
                    break;
                case 2:
                    SpawnFood (FoodType.Pear, t);
                    break;
                }
            }
        }

        //
    }
	
    void Update ()
    {
	
    }

    public GameObject SpawnFood (FoodType foodType, Vector3 position)
    {
        Quaternion angle = Quaternion.identity; // Quaternion.Euler(90, 180, 0); // Planes have to be rotated to be visible
        GameObject o = null;
        switch (foodType) {
        case FoodType.Apple:
            o = Instantiate (applePrefab, position, angle) as GameObject;
            break;
        case FoodType.Banana:
            o = Instantiate (bananaPrefab, position, angle) as GameObject;
            break;
        case FoodType.Pear:
            o = Instantiate (pearPrefab, position, angle) as GameObject;
            break;
        }

        // Set parent under Ingredient Container
        o.transform.parent = transform;
        return o;
    }
}
