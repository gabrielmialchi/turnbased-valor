using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private  float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 10f;
    private Vector3 targetPosition;

    private void Update() 
    {
        
        float stoppingDistance = .1f;

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {

        
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        
        unitAnimator.SetBool("isWalking", true);
        
        }
        else 
        {
        unitAnimator.SetBool("isWalking", false);
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
    }

    private void Move (Vector3 targetPosition)
    {

        this.targetPosition = targetPosition;

    }


}
