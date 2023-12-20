using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private  float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private  float stoppingDistance = .1f;
    
    private Vector3 targetPosition;

    private void Awake() 
    {
        targetPosition = transform.position;
    }
    private void Update() 
    {

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
    }

    public void Move (Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }


}
