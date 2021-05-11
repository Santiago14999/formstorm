using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float _magnitue;
    [SerializeField] private float _duration;

    private Coroutine _shakeCoroutine;

    public void Shake()
    {
        if (_shakeCoroutine != null)
            StopCoroutine(_shakeCoroutine);

        _shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        Vector3 startPosition = transform.position;
        float startTime = Time.time;

        while(Time.time < startTime + _duration)
        {
            float x = (Random.Range(0, 1f) > .5f ? 1 : -1) * _magnitue;
            float y = (Random.Range(0, 1f) > .5f ? 1 : -1) * _magnitue;

            transform.position = new Vector3(startPosition.x + x, startPosition.y + y, startPosition.z);
            yield return null;
        }

        transform.position = startPosition;
        _shakeCoroutine = null;
    }
}
