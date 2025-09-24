using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [Range(0f, 2f)]
    public float Intensity;

    Transform _target;
    Vector3 _initialPos;

    void Start()
    {
        _target = GetComponent<Transform>();
        _initialPos = _target.localPosition;
    }

    float _pendingShakeDuration = 0f;

    public void Shake(float duration)
    {
        if (duration > 0)
        {
            _pendingShakeDuration += duration;
        }
    }

    bool _isShaking = false;

    void Update()
    {
        if (_pendingShakeDuration > 0 && !_isShaking)
        {
            // shake because the pendingShakedDuration variable will be above 0s
            StartCoroutine(DoShake());
        }
    }

    IEnumerator DoShake()
    {
        _isShaking = true;
        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + _pendingShakeDuration)
        {
            // do shake stuff
            var randomPoint = new Vector3(Random.Range(-1f, 1f)*Intensity, Random.Range(-0.5f, 0.5f)*Intensity, _initialPos.z);
            _target.localPosition = randomPoint;
            yield return null;
        }

        _pendingShakeDuration = 0f;
        _target.localPosition = _initialPos;
        _isShaking = false;
    }
}
