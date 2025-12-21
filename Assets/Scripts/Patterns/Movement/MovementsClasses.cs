using UnityEngine;
using System.Collections.Generic;
// ====================================================
// MOVIMIENTO ALANTE 
// ====================================================
public class ForwardMovement : MovementPattern
{
    public override Tile[] Move(Tile actualTile, int moveTiles, UnitOwner owner)
    {
        List<Tile> targetTiles = new List<Tile>();

        int currentX = actualTile.x;
        int currentY = actualTile.y;

        int directionY = owner == UnitOwner.Player ? 1 : -1;

        for (int i = 1; i <= moveTiles; i++)
        {
            int targetY = currentY + (directionY * i);

        }

        return targetTiles.ToArray();
    }
}
// ====================================================
// MOVIMIENTO EN DIAGONAL 
// ====================================================
public class DiagonalRightForwardMovement : MovementPattern
{
    public override Tile[] Move(Tile actualTile, int moveTiles, UnitOwner owner)
    {
        List<Tile> targetTiles = new List<Tile>();
        int currentX = actualTile.x;
        int currentY = actualTile.y;
        int directionY = owner == UnitOwner.Player ? 1 : -1;

        for (int i = 1; i <= moveTiles; i++)
        {
            int targetX = currentX + i;
            int targetY = currentY + (directionY * i);

         
        }

        return targetTiles.ToArray();
    }
}

public class DiagonalLeftForwardMovement : MovementPattern
{
    public override Tile[] Move(Tile actualTile, int moveTiles, UnitOwner owner)
    {
        List<Tile> targetTiles = new List<Tile>();
        int currentX = actualTile.x;
        int currentY = actualTile.y;
        int directionY = owner == UnitOwner.Player ? 1 : -1;

        for (int i = 1; i <= moveTiles; i++)
        {
            int targetX = currentX - i;
            int targetY = currentY + (directionY * i);

        }

        return targetTiles.ToArray();
    }
}
// ====================================================
//Las fumadas de alex 
// ====================================================
public class Especial1ForwardMovement : MovementPattern
{
    public override Tile[] Move(Tile actualTile, int moveTiles, UnitOwner owner)
    {
        List<Tile> targetTiles = new List<Tile>();
        int currentX = actualTile.x;
        int currentY = actualTile.y;
        int directionY = owner == UnitOwner.Player ? 1 : -1;

        //1( Avanza una vez en diagonal otra adelante cubrint 2 filas)
        Vector2Int[] knightOffsets = new Vector2Int[]
        {
            new Vector2Int(1, 2),   // Derecha 1, Adelante 2
            new Vector2Int(2, 1),   // Derecha 2, Adelante 1
            new Vector2Int(-1, 2),  // Izquierda 1, Adelante 2
            new Vector2Int(-2, 1)   // Izquierda 2, Adelante 1
        };

        foreach (Vector2Int offset in knightOffsets)
        {
            int targetX = currentX + offset.x;
            int targetY = currentY + (offset.y * directionY);

            Tile tile = GridSystem.Instance.GetTile(targetX, targetY);
            if (tile != null) targetTiles.Add(tile);
        }

        return targetTiles.ToArray();
    }
}
//gg deepseek
public class CavalloForwardMovement : MovementPattern
{
    public override Tile[] Move(Tile actualTile, int moveTiles, UnitOwner owner)
    {
        List<Tile> targetTiles = new List<Tile>();
        int currentX = actualTile.x;
        int currentY = actualTile.y;
        int directionY = owner == UnitOwner.Player ? 1 : -1;

        for (int i = 1; i <= moveTiles; i++)
        {
            // casilla hacia adelante
            int intermediateY = currentY + (directionY * i);
            Tile intermediateTile = GridSystem.Instance.GetTile(currentX, intermediateY);

            if (intermediateTile == null) continue;

            // direccion lateral basada en paridad
            int lateralDirection = IsEven(intermediateY) ? 1 : -1; // Par: derecha, Impar: izquierda

            //  mover una casilla hacia el lado
            int targetX = currentX + lateralDirection;
            int targetY = intermediateY;

            Tile targetTile = GridSystem.Instance.GetTile(targetX, targetY);
            if (targetTile != null) targetTiles.Add(targetTile);
        }

        return targetTiles.ToArray();
    }

    private bool IsEven(int number)
    {
        return number % 2 == 0;
    }
}

