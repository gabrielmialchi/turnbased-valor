using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] private Material sphereGreenMaterial;
    [SerializeField] private Material sphereRedMaterial;

    [SerializeField] private MeshRenderer meshRenderer;

    private GridPosition gridPosition;
    private Action onInteractionComplete;

    private bool isGreen;
    private float timer;
    private bool isActive;


    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        SetColorGreen();
    }

    private void Update()
    {
        if (!isActive)
        { return; }

        timer -= Time.deltaTime;

        if (timer < 0f)
        { isActive = false; onInteractionComplete(); }
    }

    public void SetColorGreen()
    {
        isGreen = true;
        meshRenderer.material = sphereGreenMaterial;
    }

    public void SetColorRed()
    {
        isGreen = false;
        meshRenderer.material = sphereRedMaterial;
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;

        isActive = true;

        timer = 0.5f;

        if (isGreen)
        { SetColorRed(); }
        else
        { SetColorGreen(); }
    }

}
