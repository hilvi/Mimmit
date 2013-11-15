using System;

public enum FoodType
{
    Apple,
    Banana,
    Pear
}

public interface IFoodObject
{
    void Handle ();
    FoodType GetFoodType ();
}

