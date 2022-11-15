using System.Collections;
using UnityEngine;

public class ProductMover : MonoBehaviour
{
    [SerializeField]
    private float _productSpeed = 2f;

    private Vector3 _targetPosition;
    private Transform _productTransform;

    private void Start()
    {
        _productTransform = GetComponent<Transform>();
    }
    private void Update()
    {
        _productTransform.position = Vector3.Lerp(_productTransform.position, _targetPosition, Time.deltaTime);
    }

    public void Init(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
