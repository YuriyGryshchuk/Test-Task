using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField]
    private GameObject _productPrefab;
    [SerializeField]
    private float _factorySpawnDelay;
    [SerializeField]
    private FactoryOutputStorage _factoryStorage;

    [SerializeField]
    protected Transform _factorySpawn;

    private Transform _productTransformInStorage;

    protected bool IsSpawn = false;
    protected bool IsStorageEmpty = false;

    private void Awake()
    {
        StartCoroutine(SpawnProductDelay());
        _factoryStorage.StorageReady += SpawnReadyInit;
    }

    private void Update()
    {
        SpawnProduct—heck();
    }

    protected virtual void SpawnProduct—heck()
    {
        if (IsSpawn && IsStorageEmpty)
        {
            SpawnProduct();
        }
    }

    protected void SpawnProduct()
    {
        GameObject product = Instantiate(_productPrefab, _factorySpawn.position, Quaternion.identity);
        product.GetComponent<BoxCollider>().enabled = false;
        product.GetComponent<ProductMover>().Init(_productTransformInStorage);
        _factoryStorage.SetCurrentProduct(product);
        IsSpawn = false;
        IsStorageEmpty = false;
    }

    private void SpawnReadyInit(Transform productTransformInStorage)
    {
        _productTransformInStorage = productTransformInStorage;
        IsStorageEmpty = true;
    }

    private IEnumerator SpawnProductDelay()
    {
        while (true)
        {
            IsSpawn = true;
            yield return new WaitForSeconds(_factorySpawnDelay);
        }
    }
}
