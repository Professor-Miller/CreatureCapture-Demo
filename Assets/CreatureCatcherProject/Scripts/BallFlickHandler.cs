using System;
using UnityEngine;

public class BallFlickHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody ballRigidbody;

    [Header("Aiming")]
    [SerializeField] private float aimDistance = 0.5f;

    [Header("Throw")]
    [SerializeField] private float throwMultiplier = 0.01f;
    [SerializeField] private float minimumThrowForce = 3f;
    [SerializeField] private float maximumThrowForce = 10f;
    [SerializeField] private float horizontalInfluence = 0.5f;
    [SerializeField] private float upwardInfluence = 0.25f;


    private Vector2 _startPosition;
    private Camera _arCamera;
    private bool _isDragging;

    void OnEnable()
    {
        TouchInputHandler.OnTouchBegan += StartDrag;
        TouchInputHandler.OnTouchMoved += DragBall;
        TouchInputHandler.OnTouchEnded += ReleaseBall;
    }

    void OnDisable()
    {
        TouchInputHandler.OnTouchBegan -= StartDrag;
        TouchInputHandler.OnTouchMoved -= DragBall;
        TouchInputHandler.OnTouchEnded -= ReleaseBall;
    }

    void Awake()
    {
        _arCamera = Camera.main;
    }

    private void StartDrag(Vector2 screenPosition)
    {
        Ray ray =
            _arCamera.ScreenPointToRay(screenPosition);

        // Only start dragging if we actually touched this ball.
        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        if (hit.collider.gameObject != gameObject)
            return;

        _startPosition = screenPosition;
        _isDragging = true;

        // We control the ball while dragging.
        ballRigidbody.isKinematic = true;
    }

    private void DragBall(Vector2 screenPosition)
    {
        if (!_isDragging)
            return;

        Vector3 position = new Vector3(
            screenPosition.x,
            screenPosition.y,
            aimDistance
        );

        Vector3 worldPosition =
            _arCamera.ScreenToWorldPoint(position);

        transform.position = worldPosition;
    }

    private void ReleaseBall(Vector2 screenPosition)
    {
        if (!_isDragging)
            return;

        _isDragging = false;

        // Calculate how the finger moved.
        Vector2 swipe =
            screenPosition - _startPosition;

        // Convert our 2D swipe into a 3D direction.
        Vector3 throwDirection =
            _arCamera.transform.forward;

        throwDirection +=
            _arCamera.transform.right *
            swipe.normalized.x *
            horizontalInfluence;

        throwDirection +=
            Vector3.up *
            upwardInfluence;

        throwDirection.Normalize();

        // Longer swipes create stronger throws.
        float throwForce =
            Mathf.Clamp(
                swipe.magnitude * throwMultiplier,
                minimumThrowForce,
                maximumThrowForce
            );

        // Physics takes over.
        ballRigidbody.isKinematic = false;

        ballRigidbody.AddForce(
            throwDirection * throwForce,
            ForceMode.Impulse
        );
    }
}
