using System;

public class Assasin : Hero, IAfterMove
{
    int turnsUntilJump = 0;
    public void AfterMove(Action onFinished)
    {
        if (moved)
        {
            onFinished?.Invoke();
            return;
        }

        Tile[] tiles = aPattern.Attack(currentTile, 3, owner, false, true, true);

        if (tiles == null || tiles.Length < 3)
        {
            onFinished?.Invoke();
            return;
        }

        Tile first = tiles[0];
        Tile second = tiles[1];
        Tile third = tiles[2];

        if (first.IsFree || second.IsFree || !third.IsFree)
        {
            onFinished?.Invoke();
            return;
        }

        Unit firstUnit = first.occupant;
        Unit secondUnit = second.occupant;

        bool firstIsAlly = firstUnit.GetOwner() == owner;
        bool secondIsEnemy = secondUnit.GetOwner() != owner;

        if (firstIsAlly && secondIsEnemy)
        {
            turnsUntilJump++;
        }

        if (turnsUntilJump > abilityModifiers[tier - 1])
        {
            turnsUntilJump = 0;
            currentTile.EmptyTile();
            third.SetNewOccupant(this);
            currentTile = third;
            moved = true;

            StartCoroutine(MoveCoroutine(third.transform.position, onFinished));
        }
    }
}