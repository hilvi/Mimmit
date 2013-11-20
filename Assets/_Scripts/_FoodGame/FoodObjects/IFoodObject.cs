using System;

public enum FoodType
{
    Apple,
    Banana,
    Pear
}

public enum FoodState
{
    Full,
    Half,
    Quarter
}

public interface IFoodObject
{
    void Handle ();
    FoodType GetFoodType ();
}

