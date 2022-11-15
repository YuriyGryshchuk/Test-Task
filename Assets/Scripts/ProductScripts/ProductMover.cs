using System.Collections;
using UnityEngine;

public class ProductMover : MonoBehaviour
{
    [SerializeField]
    private float _productSpeed = 2f;
    [SerializeField]
    private float _lerpModifaer = 0.1f;
    private bool _isDestroy;
    private Vector3 _targetPosition;
    private Transform _productTransform;

    private void Start()
    {
        _productTransform = GetComponent<Transform>();
    }
    private void Update()
    {
        _productTransform.position = Vector3.Lerp(_productTransform.position, _targetPosition, Time.deltaTime);
        if(Vector3.Distance(_productTransform.position, _targetPosition) <= _lerpModifaer)
        {
            _productTransform.position = _targetPosition;
            if (_isDestroy)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Init(Vector3 targetPosition, bool isDestroy)
    {
        _isDestroy = isDestroy;
        _targetPosition = targetPosition;
    }
}
