using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public MainGameController.EntityType type;
    public Animator animator;
    public float moveSpeed = 8f;

    Vector3 targetPos;
    bool moving;

    public void Init(Vector3 decisionPos)
    {
        targetPos = decisionPos;
        moving = true;
    }

    void Update()
    {
        if (!moving) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );
    }

    public void PlaySwipe(MainGameController.EntityType input, Vector3 moveTarget)
    {
        targetPos = moveTarget;
        moving = true;

        animator.SetTrigger(
            input == MainGameController.EntityType.Dog ? "SwipeLeft" :
            input == MainGameController.EntityType.Cat ? "SwipeRight" :
            "SwipeDown"
        );
    }
}
