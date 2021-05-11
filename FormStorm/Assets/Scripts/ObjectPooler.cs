using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private FigureController _figurePrefab;
    [SerializeField] private float _amount;

    private Queue<FigureController> _pool;
    public static ObjectPooler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        _pool = new Queue<FigureController>();

        for (int i = 0; i < _amount; i++)
        {
            FigureController obj = Instantiate(_figurePrefab);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public FigureController Spawn(Vector3 position)
    {
        FigureController obj = _pool.Dequeue();

        obj.gameObject.SetActive(true);
        obj.transform.position = position;
        _pool.Enqueue(obj);

        return obj;
    }
}
