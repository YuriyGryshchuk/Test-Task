using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory3 : MonoBehaviour
{
    [SerializeField]
    private GameObject _productPrefab;
    [SerializeField]
    private Transform _factorySpawn;
    [SerializeField]
    private float _factorySpawnDelay;
    [SerializeField]
    private FactoryOutputStorage _factoryOutputStorage;
    [SerializeField]
    private FactoryInputStorage _factoryInputStorage1;
    [SerializeField]
    private FactoryInputStorage _factoryInputStorage2;


    private bool _isSpawn = false;
    private bool _isStorageEmpty = false;
    private Vector3 _productPositionInStorage;
    private GameObject _ingredient;

    private void Awake()
    {
        StartCoroutine(SpawnProductDelay());
        _factoryOutputStorage.StorageReady += SpawnReadyInit;
    }

    //private void Update()
    //{
    //    SpawnProduct();
    //}

    //private void SpawnProduct()
    //{
    //    if (_isSpawn && _isStorageEmpty)
    //    {
    //        StartCoroutine(IngredientRequest());
    //        if (_ingredient != null)
    //        {
    //            GameObject product = Instantiate(_productPrefab, _factorySpawn.position, Quaternion.identity);
    //            product.GetComponent<BoxCollider>().enabled = false;
    //            product.GetComponent<ProductMover>().Init(_productPositionInStorage, false);
    //            _factoryOutputStorage.SetCurrentProduct(product);
    //            _isSpawn = false;
    //            _isStorageEmpty = false;
    //            _ingredient = null;
    //        }
    //    }
    //}

    private void SpawnReadyInit(Vector3 productPositionInStorage)
    {
        _productPositionInStorage = productPositionInStorage;
        _isStorageEmpty = true;
    }

    private IEnumerator SpawnProductDelay()
    {
        while (true)
        {
            _isSpawn = true;
            yield return new WaitForSeconds(_factorySpawnDelay);
        }
    }

    //private IEnumerator IngredientRequest()
    //{
    //    while (_ingredient == null)
    //    {
    //        IEnumerator transitToFactory = _factoryInputStorage.TransitToFactory(_factorySpawn.position, this);
    //        _factoryInputStorage.StartCoroutine(transitToFactory);
    //        yield return new WaitForSeconds(1);
    //    }
    //}

    public void SetIngredient(GameObject ingredient)
    {
        _ingredient = ingredient;
    }
}
