using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder.MeshOperations;

public class ShootAction : BaseAction
{
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private GameObject damageTextPrefab;

    private Unit targetUnit;
    private State state;
    private int maxShootDistance = 5;
    private float stateTimer;
    private bool canShootBullet;
    private int attackRoll;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break; 
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break; 
            case State.Cooloff:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    public void Shoot()
    {
        attackRoll = RollDice(20);
        Debug.Log("Attack Roll: " + attackRoll);
        
        OnAnyShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        if (attackRoll == 1)
        {
            //Critical Miss
            Debug.Log("Critical Miss");
            int totalDamage = VandalDamage(10, 3);
            unit.Damage(totalDamage);
            Debug.Log("Vandal Damage = " + totalDamage);
            //Instantiate(damageTextPrefab, targetUnit, Quaternion.identity);
        }
        else if (attackRoll < 10)
        {
            //Miss
            Debug.Log("Miss");
            int totalDamage = 0;
            targetUnit.Damage(totalDamage);
            Debug.Log("Vandal Damage = " + totalDamage);

        }
        else if (attackRoll == 20)
        {
            //Critical Hit
            Debug.Log("CRITICAL!!!");
            int totalDamage = 30;
            targetUnit.Damage(totalDamage);
            Debug.Log("Vandal Damage = " + totalDamage);

        }
        else
        {
            //Normal Hit
            Debug.Log("HIT");
            int totalDamage = VandalDamage(10, 3);
            targetUnit.Damage(totalDamage);
            Debug.Log("Vandal Damage = " + totalDamage);
        }
    }

    private int RollDice(int sides)
    {
        return UnityEngine.Random.Range(1, sides);
    }

    public int VandalDamage(int sides, int numberOfDice)
    {
        int total = 0;
        for (int i = 0; i < numberOfDice; i++)
        {
            total += RollDice(sides);
        }
        return total;
    }

    public int ClassicDamage(int sides, int numberOfDice)
    {
        int total = 0;
        for (int i = 0; i < numberOfDice; i++)
        {
            total += RollDice(sides);
        }
        return total;
    }

    public override string GetActionName()
    {
        return "Shoot";
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();


        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z, 0);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    { continue; }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                
                if (testDistance > maxShootDistance)
                    { continue; }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    { continue; }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                    { continue; }

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(
                    unitWorldPosition + Vector3.up * unitShoulderHeight,
                    shootDir,
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                    obstaclesLayerMask))
                    { continue; }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShootBullet = true;

        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
