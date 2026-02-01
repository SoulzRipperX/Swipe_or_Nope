using UnityEngine;
using System;

public class SwipeMove : MonoBehaviour
{
    Vector3 target;
    float speed;
    Action callback;

    bool moving;

    public void Init(Vector3 targetPos, float moveSpeed, Action onFinish)
    {
        target = targetPos;
        speed = moveSpeed;
        callback = onFinish;
        moving = true;
    }

    void Update()
    {
        if (!moving) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            moving = false;
            callback?.Invoke();
            Destroy(gameObject);
        }
    }
}
