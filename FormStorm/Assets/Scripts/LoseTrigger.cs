using UnityEngine;

public class LoseTrigger : MonoBehaviour
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _gameManager.LoseChoise();
    }
}
