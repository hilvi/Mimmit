using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class AppleRecipe : FoodRecipe
{
    private FoodObject _apple;
    private FoodObject _knife;
    private FoodAnimation _anim;

    public override void Build()
    {
        Vector3 __appleSpawnPos = new Vector3(-4f, 2f, 0f);
        _apple = (
            (GameObject)Instantiate(
                _foodHandler.apple.gameObject,
                __appleSpawnPos,
                Quaternion.identity
            )
        ).GetComponent<FoodObject>();
        var moveFood = new FoodInstruction(FoodInstruction.Type.MoveFood, _apple);
        _AddInstruction(moveFood);

        // Initialize knife
        Vector3 __knifeSpawnPos = new Vector3(-2.73f, -0.74f, 0f);
        _knife = (
            (GameObject)Instantiate(
                _foodHandler.knife.gameObject,
                __knifeSpawnPos,
                Quaternion.identity
            )
        ).GetComponent<FoodObject>();
        var pickKnife = new FoodInstruction(FoodInstruction.Type.PickKnife, _knife);
        _AddInstruction(pickKnife);

        // Initialize animation
        Vector3 __animPos = new Vector3(0f, -2.75f, 0f);
        _anim = (
            (GameObject)Instantiate(
                _foodHandler.appleSliceAnimation.gameObject,
                __animPos,
                Quaternion.identity
            )
        ).GetComponent<FoodAnimation>();
        _anim.gameObject.SetActive(false);
        var sliceAppleWithKnife = new FoodInstruction(FoodInstruction.Type.SliceFood, _apple, _knife);
        sliceAppleWithKnife.AddAnimation(_anim);
        _AddInstruction(sliceAppleWithKnife);
    }
}
