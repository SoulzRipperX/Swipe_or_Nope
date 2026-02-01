using UnityEngine;
using System;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    public MainGameController.EntityType type;
    public float moveSpeed = 8f;

    [Header("Swipe Prefabs")]
    public GameObject swipeLeftPrefab;
    public GameObject swipeRightPrefab;
    public GameObject swipeDownPrefab;

    Vector3 targetPos;
    bool moving;

    public bool canSwipe { get; private set; }

    SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        canSwipe = false;
    }

    void Update()
    {
        if (!moving) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            moving = false;
    }

    public void MoveTo(Vector3 pos)
    {
        targetPos = pos;
        moving = true;
    }

    public void EnableSwipe(bool value)
    {
        canSwipe = value;
    }

    public void PlaySwipe(
        MainGameController.EntityType input,
        Vector3 target,
        Action onFinish
    )
    {
        if (sprite != null)
            sprite.enabled = false;

        GameObject prefab = null;

        if (input == MainGameController.EntityType.Dog)
            prefab = swipeLeftPrefab;
        else if (input == MainGameController.EntityType.Cat)
            prefab = swipeRightPrefab;
        else
            prefab = swipeDownPrefab;

        if (prefab != null)
        {
            GameObject fx = Instantiate(prefab, transform.position, Quaternion.identity);

            SwipeMove move = fx.GetComponent<SwipeMove>();
            if (move != null)
                move.Init(target, moveSpeed, null);
        }

        StartCoroutine(MoveAndFinish(target, onFinish));
    }

    IEnumerator MoveAndFinish(Vector3 target, Action onFinish)
    {
        targetPos = target;
        moving = true;

        while (moving)
            yield return null;

        onFinish?.Invoke();
    }
}
