using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStorage : MonoBehaviour
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
    [SerializeField]
    private float _transitToPlayerStorageDeley;

    protected float ProductLength;
    protected float ProductWidth;
    protected float ProductHeight;
    private GameObject[,] _storageList;
    private GameObject _currentProduct;
    private bool _isCheckCoroutineRun = false;
    private IEnumerator _checkStorageFullnessCoroutine;
    private FactoryInputStorage _factoryInputStorage;
    private Vector3 _productPositionInStorage;

    private void Start()
    {
        _storageList = new GameObject[_storageWidth, _storageHeight];
        var product = Instantiate(ProductPrefab);
        ProductLength = product.transform.localScale.z;
        ProductWidth = product.transform.localScale.x;
        ProductHeight = product.transform.localScale.y;
        Destroy(product);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<FactoryOutputStorage>(out FactoryOutputStorage factoryStorage))
        {
            _checkStorageFullnessCoroutine = CheckStorageFullness(factoryStorage);
            StartCoroutine(_checkStorageFullnessCoroutine);
        }
        if(other.TryGetComponent<FactoryInputStorage>(out FactoryInputStorage factoryInputStorage))
        {
            factoryInputStorage.StartTransit(this);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<FactoryOutputStorage>(out FactoryOutputStorage factoryStorage))
        {
            StopCoroutine(_checkStorageFullnessCoroutine);
        }
        if (other.TryGetComponent<FactoryInputStorage>(out FactoryInputStorage factoryInputStorage))
        {
            factoryInputStorage.StopTransit();
        }
    }

    private IEnumerator CheckStorageFullness(FactoryOutputStorage factoryStorage)
    {
        for (int i = 0; i < _storageWidth; i++)
        {
            for (int j = 0; j < _storageHeight; j++)
            {
                if (_storageList[i, j] == null)
                {
                    factoryStorage.PlayerStorageInit(this, GenerateProductPositionInStorage(i, j));
                    yield return new WaitUntil(predicate: () => _currentProduct != null);
                    _storageList[i, j] = _currentProduct;
                    _currentProduct = null;
                    yield return new WaitForSeconds(_transitToPlayerStorageDeley);
                }
            }
        }

    }

    private IEnumerator TransitToPlayerStorage()
    {
        for (int i = 0; i < _storageWidth; i++)
        {
            for (int j = 0; j < _storageHeight; j++)
            {
                if (_storageList[i, j] != null)
                {
                    _storageList[i, j].GetComponent<ProductMover>().Init(_productPositionInStorage);
                    _storageList[i, j].transform.SetParent(null);
                    _factoryInputStorage.SetCurrentProduct(_storageList[i, j]);
                    _storageList[i, j] = null;
                    yield break;
                }
            }
        }
        yield return null;
    }

    private Vector3 GenerateProductPositionInStorage(int width, int height)
    {
        return new Vector3(
                StorageTransformSpawnpoint.position.x,
                StorageTransformSpawnpoint.position.y + ProductHeight * height,
                StorageTransformSpawnpoint.position.z + ProductWidth * width);
    }

    public void SetCurrentProduct(GameObject currentProduct)
    {
        _currentProduct = currentProduct;
    }

    public void PlayerStorageInit(FactoryInputStorage factoryInputStorage, Vector3 productPositionInStorage)
    {
        _factoryInputStorage = factoryInputStorage;
        _productPositionInStorage = productPositionInStorage;
        StartCoroutine(TransitToPlayerStorage());
    }
}

