using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryOutputStorage : MonoBehaviour
{
    [SerializeField]
    protected GameObject ProductPrefab;
    [SerializeField]
    protected Transform StorageTransformSpawnpoint;
    [SerializeField]
    private int _storageLength;
    [SerializeField]
    private int _storageWidth;
    [SerializeField]
    private int _storageHeight;

    protected float ProductLength;
    protected float ProductWidth;
    protected float ProductHeight;
    private GameObject[,] _storageList;
    private GameObject _currentProduct;
    private Vector3 _productPositionInStorage;
    private PlayerStorage _playerStorage;
    private bool _isSpawn = false;
    private bool _isStorageEmpty = false;

    public delegate void StorageDelegate(Vector3 productPositionInStorage);
    public event StorageDelegate StorageReady;

    private void Start()
    {
        _storageList = new GameObject[_storageWidth, _storageLength];
        var product = Instantiate(ProductPrefab);
        ProductLength = product.transform.localScale.z;
        ProductWidth = product.transform.localScale.x;
        ProductHeight = product.transform.localScale.y;
        Destroy(product);
        StartCoroutine(CheckStorageFullness());
    }

    private IEnumerator CheckStorageFullness()
    {
        for (int i = 0; i < _storageWidth; i++)
        {
            for (int j = 0; j < _storageLength; j++)
            {
                if (_storageList[i, j] == null)
                {
                    StorageReady?.Invoke(GenerateProductPositionInStorage(i, j));
                    yield return new WaitUntil(predicate: () => _currentProduct != null);
                    _storageList[i, j] = _currentProduct;
                    _currentProduct = null;
                }
            }
        }
    }

    private IEnumerator TransitToPlayerStorage()
    {
        for (int i = 0; i < _storageWidth; i++)
        {
            for (int j = 0; j < _storageLength; j++)
            {
                if (_storageList[i, j] != null)
                {
                    _storageList[i, j].GetComponent<ProductMover>().Init(_productPositionInStorage);
                    _storageList[i, j].transform.SetParent(_playerStorage.gameObject.transform);
                    _playerStorage.SetCurrentProduct(_storageList[i, j]);
                    _storageList[i, j] = null;
                    yield break;
                }
            }
        }
        yield return null;
    }

    private Vector3 GenerateProductPositionInStorage(int width, int lenght)
    {
        return new Vector3(
                StorageTransformSpawnpoint.position.x + ProductWidth * width,
                StorageTransformSpawnpoint.position.y,
                StorageTransformSpawnpoint.position.z + ProductLength * lenght);
    }

    public void SetCurrentProduct(GameObject currentProduct)
    {
        _currentProduct = currentProduct;
    }

    public void PlayerStorageInit(PlayerStorage playerStorage, Vector3 productPositionInStorage)
    {
        _playerStorage = playerStorage;
        _productPositionInStorage = productPositionInStorage;
        StartCoroutine(TransitToPlayerStorage());
    }
}

