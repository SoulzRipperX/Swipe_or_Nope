using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    [Header("Character Settings")]
    public MainGameController.EntityType type;
    public float moveSpeed = 8f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void MoveTo(Vector3 target)
    {
        StartCoroutine(MoveRoutine(target));
    }

    IEnumerator MoveRoutine(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
