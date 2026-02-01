using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 startPos;
    private float swipeThreshold = 50f;
    private bool hasAnswered;

    public Animator animator;

    void Update()
    {
        if (hasAnswered) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
            startPos = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
            HandleSwipe((Vector2)Input.mousePosition - startPos);
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0) return;

        Touch t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Began)
            startPos = t.position;

        if (t.phase == TouchPhase.Ended)
            HandleSwipe(t.position - startPos);
    }

    void HandleSwipe(Vector2 delta)
    {
        if (delta.magnitude < swipeThreshold) return;

        hasAnswered = true;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x < 0)
            {
                animator.SetTrigger("SwipeLeft");
                MainGameController.Instance.OnSwipe(MainGameController.EntityType.Dog);
            }
            else
            {
                animator.SetTrigger("SwipeRight");
                MainGameController.Instance.OnSwipe(MainGameController.EntityType.Cat);
            }
        }
        else if (delta.y < 0)
        {
            animator.SetTrigger("SwipeDown");
            MainGameController.Instance.OnSwipe(MainGameController.EntityType.Anomaly);
        }

        Invoke(nameof(ResetAnswer), 0.2f);
    }

    void ResetAnswer()
    {
        hasAnswered = false;
    }
}
