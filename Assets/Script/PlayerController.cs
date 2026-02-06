using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 startPos;
    bool locked;
    public float swipeThreshold = 50f;

    void Update()
    {
        if (locked) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        Mouse();
#else
        Touch();
#endif
    }

    void Mouse()
    {
        if (Input.GetMouseButtonDown(0))
            startPos = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
            Swipe((Vector2)Input.mousePosition - startPos);
    }

    void Touch()
    {
        if (Input.touchCount == 0) return;

        Touch t = Input.GetTouch(0);
        if (t.phase == TouchPhase.Began)
            startPos = t.position;
        if (t.phase == TouchPhase.Ended)
            Swipe(t.position - startPos);
    }

    void Swipe(Vector2 delta)
    {
        if (delta.magnitude < swipeThreshold) return;

        SoundManager.Instance?.Playclick();
        locked = true;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            MainGameController.Instance.OnSwipe(
                delta.x < 0 ?
                MainGameController.EntityType.Dog :
                MainGameController.EntityType.Cat
            );
        else if (delta.y < 0)
            MainGameController.Instance.OnSwipe(MainGameController.EntityType.Anomaly);

        Invoke(nameof(Unlock), 0.2f);
    }

    void Unlock() => locked = false;
}
