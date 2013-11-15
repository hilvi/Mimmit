using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientManager : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject bananaPrefab;
    public GameObject pearPrefab;

    private List<GameObject> ingredientList = new List<GameObject> ();

    void Start ()
    {
        Quaternion angle = Quaternion.Euler (-90, 0, 0); // Planes have to be rotated to be visible
        Vector3 anchor = transform.position;

        // Spawns food randomly for testing
        for (int y = 0; y < 3; y++) {
            for (int x = 0; x < 3; x++) {
                // Calculate new positions
                var t = new Vector3 (anchor.x, anchor.y, anchor.z);
                t.x += x * 2f;
                t.y -= y * 2f;

                // Randomly choose a fruit
                int choice = Random.Range (0, 3);
                switch (choice) {
                case 0:
                    SpawnFood (FoodType.Apple, t, angle);
                    break;
                case 1:
                    SpawnFood (FoodType.Banana, t, angle);
                    break;
                case 2:
                    SpawnFood (FoodType.Pear, t, angle);
                    break;
                }
            }
        }

        //
    }
	
    void Update ()
    {
	
    }

    private void SpawnFood (FoodType foodType, Vector3 position, Quaternion angle)
    {
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
    }
}
