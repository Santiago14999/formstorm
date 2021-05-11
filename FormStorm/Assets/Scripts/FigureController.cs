using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class FigureController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _figureSprite;
    [SerializeField] private GameObject _mask;

    private CircleCollider2D _circleCollider;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(float formSize, float size, Color color)
    {
        _circleCollider.radius = size * .64f;
        _figureSprite.transform.localScale = Vector3.one * size;
        _figureSprite.color = color;
        _mask.transform.localScale = Vector3.one * formSize;
        _mask.SetActive(size > formSize);
    }

    public void AddVelocity(bool side, float force, float torque)
    {
        float sign = side ? 1 : -1;

        _rigidbody.AddForce(Vector2.right * sign * force, ForceMode2D.Impulse);
        _rigidbody.AddTorque(torque * -sign, ForceMode2D.Impulse);
    }
}
