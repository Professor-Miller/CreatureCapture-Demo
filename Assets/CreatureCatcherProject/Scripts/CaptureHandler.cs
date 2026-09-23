using System;
using UnityEngine;
using System.Collections;
using System.Collections.Concurrent;

public class CaptureHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;

    public static event Action<CreatureDataSO> OnCreatureCaught;
    public static event Action OnMissedCreature;

    private bool _hasBeenCaught;
    private bool _missedCreature;
    private CreatureHandler _creature;
    private readonly float _timeToDelete = 3.0f;
    private float _deleteTimer;

    void OnBecameInvisible()
    {
        MissedCreature();
    }

    void Update()
    {
        if (!_missedCreature) return;

        _deleteTimer += Time.deltaTime;
        if (_deleteTimer >= _timeToDelete)
        {
            MissedCreature();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _missedCreature = true;
            Transform groundPlane = collision.transform.parent;

            gameObject.transform.parent = groundPlane;
        }

        if (collision.gameObject.TryGetComponent(out CreatureHandler creature))
        {
            if (_missedCreature) return;

            _creature = creature;
            _hasBeenCaught = true;
            rigidbody.isKinematic = true;
            gameObject.GetComponent<Collider>().enabled = false;
            creature.Capture();

            Transform trackedImage = collision.transform.parent;

            // Move back toward tracked image
            StartCoroutine(MoveBallToTarget(transform, trackedImage.transform, 0.1f));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Death")) return;

        MissedCreature();
    }

    private IEnumerator MoveBallToTarget(Transform ball, Transform target, float duration)
    {
        Vector3 startPosition = ball.position;
        float elapsed = 0f;
        var groundPosition = target.position + new Vector3(0, 0.1f, 0);

        yield return new WaitForSeconds(0.3f);

        while (elapsed < duration)
        {
            if (ball == null || target == null)
                yield break;

            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            ball.position = Vector3.Lerp(startPosition, groundPosition, t);

            yield return null;
        }

        if (ball == null || target == null)
            yield break;

        ball.position = groundPosition;
        ball.SetParent(target);

        OnCreatureCaught?.Invoke(_creature.CreatureData);
    }

    private void MissedCreature()
    {
        if (_hasBeenCaught) return;

        _missedCreature = true;
        OnMissedCreature?.Invoke();
        Debug.Log("Missed the Creature!");
        Destroy(this.gameObject);
    }
}
