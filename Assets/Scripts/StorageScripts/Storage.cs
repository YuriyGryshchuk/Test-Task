using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Storage: MonoBehaviour
{
    [SerializeField]
    protected GameObject ProductPrefab;
    [SerializeField]
    protected Transform StorageTransformSpawnpoint;
    [SerializeField]

    protected float ProductLength;
    protected float ProductWidth;
    protected float ProductHeight;

    private void Start()
    {
        var product = Instantiate(ProductPrefab);
        ProductLength = product.transform.localScale.z;
        ProductWidth = product.transform.localScale.x;
        ProductHeight = product.transform.localScale.y;
        Destroy(product);
    }

}
