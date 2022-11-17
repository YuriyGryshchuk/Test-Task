using UnityEngine;

public class Need1InputFactory : NeedInputFactory
{
    [SerializeField]
    private FactoryFirstInputStorage _factoryFirstInputStorage;

    protected override void IngridientCheck()
    {
        StartIngredientRequest();
        if (Ingredient1 != null)
        {
            SpawnProduct();
            Ingredient1.GetComponent<ProductMover>().Destroy();
            Ingredient1 = null;
        }
    }

    private void StartIngredientRequest()
    {
        Ingredient1Request(_factoryFirstInputStorage);
    }
}
