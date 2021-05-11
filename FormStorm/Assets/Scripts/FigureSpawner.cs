using System.Collections.Generic;
using UnityEngine;

public class FigureSpawner : MonoBehaviour
{
    [SerializeField] private FigureController _figurePrefab;
    [SerializeField] private SpriteRenderer _growingFigure;
    [SerializeField] private Transform _figureForm;

    [SerializeField] private ParticleSystem _particles;

    [SerializeField] private float _allowedDeltaSize;
    [SerializeField] private float _figureStartGrowSpeed;
    [SerializeField] private float _figureMaxGrowSpeed;
    [SerializeField] private float _figureMinSizeToCut;
    [SerializeField] private float _figureMaxSize;
    [SerializeField] private float _forceAfterCut;
    [SerializeField] private float _torqueAfterCut;

    [SerializeField] private int _minCutsBeforeFormSizeChange;
    [SerializeField] private int _maxCutsBeforeFormSizeChange;
    [SerializeField] private float _formNormalSize;
    [SerializeField] private float _formSmallSize;
    [SerializeField] private float _formBigSize;

    [SerializeField] private Color[] _colors;
    [SerializeField] private CameraShaker _camera;

    private bool _isPlaying;
    private float _currentSpeed;
    private float _currentSize;
    private Color _figureColor;
    private int _lastColor;
    private ScoreController _scoreController;
    private List<GameObject> _spawnedFigures;

    private Coroutine _formSizeChangeCoroutine;
    private float _currentFormSize;
    private int _currentCutBeforeFormSizeChange;
    private int _cutsToFormSizeChange;

    public static FigureSpawner Instance;

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStart += HandleGameStart;
        GameManager.OnGameEnd += HandleGameEnd;
        _spawnedFigures = new List<GameObject>(64);
        _lastColor = Random.Range(0, _colors.Length);
        _figureColor = _colors[_lastColor];
        _growingFigure.color = _figureColor;
    }

    private void Start()
    {
        _scoreController = ScoreController.Instance;
    }

    private void HandleGameStart()
    {
        _isPlaying = true;
        _currentSize = 0;
        _cutsToFormSizeChange = Random.Range(_minCutsBeforeFormSizeChange, _maxCutsBeforeFormSizeChange + 1);
        _currentFormSize = _formNormalSize;
        _figureForm.localScale = Vector3.one * _currentFormSize;
        _currentSpeed = _figureStartGrowSpeed;
    }

    private void HandleGameEnd()
    {
        if (_isPlaying)
            AudioManager.Instance.PlayLostSound();
        _isPlaying = false;
        if (_formSizeChangeCoroutine != null)
            StopCoroutine(_formSizeChangeCoroutine);

        foreach (var figure in _spawnedFigures)
            Destroy(figure);
    }

    private void Update()
    {
        if (!_isPlaying)
            return;

        _currentSize += _currentSpeed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0) || _currentSize > _figureMaxSize)
            Cut(Random.Range(0, 1f) > .5f);

        _growingFigure.transform.localScale = Vector3.one * _currentSize;
    }

    public void Revive()
    {
        HandleGameStart();
    }

    private void Cut(bool side)
    {
        if (!_isPlaying || _currentSize < _figureMinSizeToCut)
            return;

        _camera.Shake();

        if (Mathf.Abs(_currentFormSize - _currentSize) <= _allowedDeltaSize)
        {
            _scoreController.AddScore(1);
            AudioManager.Instance.PlayPopSound();
            SpawnParticles();
            TryChangeSize();
        }
        else
        {
            AudioManager.Instance.PlaySpawnSound();
            SpawnFigure(side);
        }

        _currentSize = 0;
        _growingFigure.color = NextColor();
        UpdateSpeed();
    }

    private void SpawnParticles()
    {
        var main = _particles.main;
        main.startColor = _figureColor;
        var shape = _particles.shape;
        shape.radius = _currentFormSize;
        _particles.Play();
    }

    private void UpdateSpeed()
    {
        _currentSpeed = Mathf.Lerp(_figureStartGrowSpeed, _figureMaxGrowSpeed, _scoreController.Score / 150f);
    }

    private void SpawnFigure(bool side)
    {
        FigureController figure = Instantiate(_figurePrefab, _growingFigure.transform.position, Quaternion.identity);
        figure.Initialize(_currentFormSize, _currentSize, _figureColor);
        figure.AddVelocity(side, _forceAfterCut, _torqueAfterCut);
        _spawnedFigures.Add(figure.gameObject);
    }

    private Color NextColor()
    {
        _lastColor++;
        _lastColor %= _colors.Length;
        _figureColor = _colors[_lastColor];
        return _figureColor;
    }

    private void TryChangeSize()
    {
        if (_currentCutBeforeFormSizeChange < _cutsToFormSizeChange)
        {
            _currentCutBeforeFormSizeChange++;
            return;
        }

        _currentCutBeforeFormSizeChange = 0;
        _cutsToFormSizeChange = Random.Range(_minCutsBeforeFormSizeChange, _maxCutsBeforeFormSizeChange + 1);

        float toSize;
        if (_currentFormSize != _formNormalSize)
            toSize = _formNormalSize;
        else
            toSize = Random.Range(0, 1f) > .4 ? _formSmallSize : _formBigSize;

        _formSizeChangeCoroutine = StartCoroutine(FormSizeChangeCoroutine(_currentFormSize, toSize));
        _currentFormSize = toSize;
    }

    private System.Collections.IEnumerator FormSizeChangeCoroutine(float from, float to)
    {
        float startTime = Time.time;
        float duration = .2f;
        while(Time.time < startTime + duration)
        {
            _figureForm.localScale = Vector3.one * Mathf.Lerp(from, to, (Time.time - startTime) / duration);
            yield return null;
        }

        _figureForm.localScale = Vector3.one * to;
        _formSizeChangeCoroutine = null;
    }
}
