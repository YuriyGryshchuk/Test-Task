using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Need2InputStorage : NeedInputFactory
{
    [SerializeField]
    private FactoryFirstInputStorage _factoryFirstInputStorage;
    [SerializeField]
    private FactorySecondInputStorage _factorySecondInputStorage;


    protected override void IngridientCheck()
    {
        StartIngredientRequest();
        if (Ingredient1 != null && Ingredient2 != null)
        {
            SpawnProduct();
            Ingredient1.GetComponent<ProductMover>().Destroy();
            Ingredient1 = null;
            Ingredient2.GetComponent<ProductMover>().Destroy();
            Ingredient2 = null;
        }
    }

    private void StartIngredientRequest()
    {
        Ingredient1Request(_factoryFirstInputStorage);
        Ingredient2Request(_factorySecondInputStorage);
    }
}
