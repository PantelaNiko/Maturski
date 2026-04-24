using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    Transform target;
    Vector3 offset;

    public void Init(Transform t)
    {
        target = t;
        offset = transform.position - t.position;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = target.position + offset;
    }
}