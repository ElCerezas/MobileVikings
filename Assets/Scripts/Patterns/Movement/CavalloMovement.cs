using System.Collections.Generic;
[System.Serializable]
public class CavalloMovement : MovementPattern
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
            Tile intermediateTile = GridSystem.instance.GetTile(currentX, intermediateY);

            if (intermediateTile == null) continue;

            // direccion lateral basada en paridad
            int lateralDirection = IsEven(intermediateY) ? 1 : -1; // Par: derecha, Impar: izquierda

            //  mover una casilla hacia el lado
            int targetX = currentX + lateralDirection;
            int targetY = intermediateY;

            Tile targetTile = GridSystem.instance.GetTile(targetX, targetY);
            if (targetTile != null) targetTiles.Add(targetTile);
        }

        return targetTiles.ToArray();
    }

    private bool IsEven(int number)
    {
        return number % 2 == 0;
    }
}

