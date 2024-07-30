using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class HitboxParameters : MonoBehaviour
{
    public float min;
    public float max;

    public Vector2 launchAngle;

    public void GenerateAngle()
    {
        float random = Random.Range(min, max);
        Quaternion randomAngle = Quaternion.AngleAxis(random, Vector3.forward );

        Debug.Log(random);
        Debug.Log(randomAngle);
        if (gameObject.GetComponentInParent<PlayerInput>().facingRight == true)
        {
            launchAngle = randomAngle * Vector2.left;
            launchAngle.y *= -1;
        }
        else
        {
            launchAngle = randomAngle * Vector2.right;
        }
        

    }


}
