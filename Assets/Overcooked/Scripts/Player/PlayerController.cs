using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public LayerMask solidObjectsLayer;
    public float moveSpeed;
    public bool isMoving;
    private Vector2 input;


    private void Update()
    {
        if(!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            input.x = input.x > 0 ? 1 : (input.x < 0 ? -1 : 0);
            input.y = input.y > 0 ? 1 : (input.y < 0 ? -1 : 0);

            if (input.x != 0)
            {
                input.y = 0;
            }

            if(input != Vector2.zero )
            {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if(IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));

                }
            }
        }


        IEnumerator Move(Vector3 targetPos)
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

        bool IsWalkable(Vector3 targetPos)
        {
            if (Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer) != null)
            {
                return false;
            }
            return true;
        }

    }


}
