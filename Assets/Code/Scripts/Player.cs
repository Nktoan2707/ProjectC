﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;


public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public bool IsEnabled { get; private set; }

    public event EventHandler<OnInteractionEventArgs> OnInteraction;
    public class OnInteractionEventArgs : EventArgs
    {
        public IInteractable interactedObject;
    }

    public float Speed { get; private set; }
    public float SpeedMultiplier { get; set; }

    private const float movingUnit = 1f;
    private Vector2 movingDirection;
    private float movingDistance;
    private Vector3 movingDestination;
    public bool IsMoving { get; set; }

    private List<IInteractable> interactableObjectList;

    private void Awake()
    {
        Instance = this;
        IsEnabled = true;

        Speed = 3f;
        SpeedMultiplier = 1f;

        transform.position = Vector3.zero;
        movingDirection = Vector3.zero;
        movingDestination = transform.position;
        IsMoving = false;
        interactableObjectList = new List<IInteractable>();
    }

    private void Start()
    {

    }


    private void Update()
    {
        movingDistance = Speed * SpeedMultiplier * Time.deltaTime;
        IsMoving = !Mathf.Approximately(Vector3.Distance(transform.position, movingDestination), 0);

        if (IsMoving)
        {
            HandleMovement();
        }
        else
        {
            ExecuteCommands();
        }
    }

    private void HandleMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, movingDestination, movingDistance);
        IsMoving = true;
        return;
    }

    private void ExecuteCommands()
    {
        movingDirection = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.A))
        {
            movingDirection.x = -movingUnit;

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            movingDirection.x = movingUnit;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            movingDirection.y = movingUnit;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            movingDirection.y = -movingUnit;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }

        movingDirection.Normalize();
        SetMovingDestination(transform.position + new Vector3(movingDirection.x, movingDirection.y));
    }

    private void Interact()
    {
        if (interactableObjectList.Count <= 0)
        {
            return;
        }

        interactableObjectList[0].Interact();
        OnInteraction?.Invoke(this, new OnInteractionEventArgs
        {
            interactedObject = interactableObjectList[0]
        });

        // if interacted object is a gem
        CollectibleGem collectibleGem = interactableObjectList[0] as CollectibleGem;
        if (collectibleGem != null)
        {
            collectibleGem.DestroySelf();
        }
    }

    public void AddInteractableObject(IInteractable interactableObject)
    {
        interactableObjectList.Add(interactableObject);
    }

    public void RemoveInteractableObject(IInteractable interactableObject)
    {
        interactableObjectList.Remove(interactableObject);
    }

    public Vector2 GetMovingDirection()
    {
        return movingDirection;
    }

    private void SetMovingDestination(Vector3 newMovingDirection)
    {
        if(Physics2D.OverlapCircle(newMovingDirection, 0.1f))
        {
            return;
        }

        this.movingDestination = newMovingDirection;
    }
}

