using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 startPos;
    bool locked;
    public float swipeThreshold = 50f;

    void Update()
    {
        if (locked) return;

        HandleKeyboard();

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    void HandleKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            DoSwipe(MainGameController.EntityType.Dog);

        else if (Input.GetKeyDown(KeyCode.RightArrow))
            DoSwipe(MainGameController.EntityType.Cat);

        else if (Input.GetKeyDown(KeyCode.DownArrow))
            DoSwipe(MainGameController.EntityType.Anomaly);
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
            startPos = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
            Swipe((Vector2)Input.mousePosition - startPos);
    }

    void HandleTouch()
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

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            DoSwipe(delta.x < 0
                ? MainGameController.EntityType.Dog
                : MainGameController.EntityType.Cat);
        }
        else if (delta.y < 0)
        {
            DoSwipe(MainGameController.EntityType.Anomaly);
        }
    }

    void DoSwipe(MainGameController.EntityType type)
    {
        locked = true;
        SoundManager.Instance?.Playclick();
        MainGameController.Instance.OnSwipe(type);
        Invoke(nameof(Unlock), 0.2f);
    }

    void Unlock() => locked = false;
}
