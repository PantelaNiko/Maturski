using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator animator;

    PlayerMovement movementScript;
    float originalSpeed;

    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
        originalSpeed = movementScript.MOVE_SPEED;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 playerPosition = playerTransform.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            playerPosition.z = 0f;
            Vector3 directionalVector = mousePosition - playerPosition;

            movementScript.MOVE_SPEED = 0;
            animator.SetFloat("MouseHorizontal", directionalVector.x);
            animator.SetFloat("MouseVertical", directionalVector.y);
            animator.SetTrigger("MouseButton1");
            Invoke("StopStun", 1f / 3f);
        }
    }
    void StopStun()
    {
        movementScript.MOVE_SPEED = originalSpeed;
    }
}
