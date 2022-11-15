using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField]
    private GameObject _productPrefab;
    [SerializeField]
    private Transform _factorySpawn;
    [SerializeField]
    private float _factorySpawnDelay;
    [SerializeField]
    private FactoryOutputStorage _factoryStorage;

    private bool _isSpawn = false;
    private bool _isStorageEmpty = false;
    private Vector3 _productPositionInStorage;

    private void Awake()
    {
        StartCoroutine(SpawnProductDelay());
        _factoryStorage.StorageReady += SpawnReadyInit;
    }

    private void Update()
    {
        SpawnProduct();
    }

    private void SpawnProduct()
    {
        if (_isSpawn && _isStorageEmpty)
        {
            GameObject product = Instantiate(_productPrefab, _factorySpawn.position, Quaternion.identity);
            product.GetComponent<BoxCollider>().enabled = false;
            product.GetComponent<ProductMover>().Init(_productPositionInStorage);
            _factoryStorage.SetCurrentProduct(product);
            _isSpawn = false;
            _isStorageEmpty = false;
        }
    }

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
}
