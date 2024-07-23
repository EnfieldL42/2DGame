using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerp1 : MonoBehaviour
{
    public LayerMask solidObjectsLayer;
    public float moveSpeed;
    public bool isMoving;
    private Vector2 input;

    private BoxCollider2D boxCollider;
    private Transform spriteTransform;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteTransform = transform.GetChild(0); // Assumes the sprite is the first child
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            input.x = input.x > 0 ? 1 : (input.x < 0 ? -1 : 0);
            input.y = input.y > 0 ? 1 : (input.y < 0 ? -1 : 0);

            if (input.x != 0)
            {
                input.y = 0;
            }

            if (input != Vector2.zero)
            {
                var targetPos = GetTargetPosition();
                UpdateColliderRotation();

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPos = currentPosition + new Vector3(input.x, input.y, 0);
        targetPos.x = Mathf.Round(targetPos.x);
        targetPos.y = Mathf.Round(targetPos.y);
        return targetPos;
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        Collider2D collider = Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer);
        return collider == null;
    }

    private void UpdateColliderRotation()
    {
        if (input.x > 0)
        {
            RotateCollider(90); // Facing right
        }
        else if (input.x < 0)
        {
            RotateCollider(270); // Facing left
        }
        else if (input.y > 0)
        {
            RotateCollider(180); // Facing up
        }
        else if (input.y < 0)
        {
            RotateCollider(0); // Facing down
        }
    }

    private void RotateCollider(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
        spriteTransform.rotation = Quaternion.Euler(0, 0, 0); // Ensure sprite stays unrotated
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.red;
        Vector2 direction = (Vector2)transform.position + input - (Vector2)transform.position;
        Gizmos.DrawWireCube((Vector2)transform.position + direction, boxCollider.size);
    }
}