using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryInputStorage : MonoBehaviour
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
    private float _transitToFactoryStorageDeley;

    protected float ProductLength;
    protected float ProductWidth;
    protected float ProductHeight;
    private GameObject[,] _storageList;
    private GameObject _currentProduct;
    private Vector3 _productPositionInStorage;
    private PlayerStorage _playerStorage;
    private bool _isSpawn = false;
    private bool _isStorageEmpty = false;
    private IEnumerator _checkStorageFullnessCoroutine;

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
    }

    private IEnumerator CheckStorageFullness(PlayerStorage playerStorage)
    {
        for (int i = 0; i < _storageWidth; i++)
        {
            for (int j = 0; j < _storageLength; j++)
            {
                if (_storageList[i, j] == null)
                {
                    playerStorage.PlayerStorageInit(this, GenerateProductPositionInStorage(i, j));
                    yield return new WaitUntil(predicate: () => _currentProduct != null);
                    _storageList[i, j] = _currentProduct;
                    _currentProduct = null;
                    yield return new WaitForSeconds(_transitToFactoryStorageDeley);
                }
            }
        }
    }

    public IEnumerator TransitToFactory(Vector3 ingridientPosition, Factory2 factory2)
    {
        for (int i = 0; i < _storageWidth; i++)
        {
            for (int j = 0; j < _storageLength; j++)
            {
                if (_storageList[i, j] != null)
                {
                    _storageList[i, j].GetComponent<ProductMover>().Init(ingridientPosition, true);
                    factory2.SetIngredient(_storageList[i, j]);
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

    public void StartTransit(PlayerStorage playerStorage)
    {
        _checkStorageFullnessCoroutine = CheckStorageFullness(playerStorage);
        StartCoroutine(_checkStorageFullnessCoroutine);
    }

    public void StopTransit()
    {
        StopCoroutine(_checkStorageFullnessCoroutine);
    }
}
