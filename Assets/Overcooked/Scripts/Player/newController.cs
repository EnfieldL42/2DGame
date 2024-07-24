using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    private float currentSpeed;

    public LayerMask obstacleLayer;

    private Vector3 targetPosition;
    private bool isMoving;

    // Public method to set the spawn position
    public void SetSpawnPosition(Vector3 spawnPosition)
    {
        // Snap to tile position
        targetPosition = SnapToTile(spawnPosition);
        transform.position = targetPosition;
        isMoving = false; // Ensure the player is not moving initially
    }

    private void Start()
    {
        // Optionally, you can set a default spawn position
        // Example: SetSpawnPosition(new Vector3(0, 0, 0));
    }

    private void Update()
    {
        if (!isMoving)
        {
            HandleInput();
        }

        MovePlayer();
    }

    private void HandleInput()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 direction = Vector3.zero;

        if (input != Vector2.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            if (input.x != 0)
            {
                direction = new Vector3(Mathf.Sign(input.x), 0, 0);
            }
            else if (input.y != 0)
            {
                direction = new Vector3(0, Mathf.Sign(input.y), 0);
            }

            Vector3 nextPosition = transform.position + direction;
            if (CanMoveTo(nextPosition))
            {
                targetPosition = SnapToTile(nextPosition);
                isMoving = true;
            }
        }
    }

    private bool CanMoveTo(Vector3 position)
    {
        Collider2D hit = Physics2D.OverlapCircle(position, 0.1f, obstacleLayer);
        return hit == null;
    }

    private void MovePlayer()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    private Vector3 SnapToTile(Vector3 position)
    {
        // Adjust this as needed based on your tile size
        float tileSize = 1f; // Example tile size, change if different
        return new Vector3(Mathf.Round(position.x / tileSize) * tileSize, Mathf.Round(position.y / tileSize) * tileSize, position.z);
    }
}