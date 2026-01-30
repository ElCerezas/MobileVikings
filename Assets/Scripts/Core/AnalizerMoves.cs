using System.Collections.Generic;
using UnityEngine;

public class AnalizerMoves : MonoBehaviour
{
    public struct BestMove
    {
        public Unit unit;
        public Tile tile;
        public int score;
    }
    public BestMove bestMove;
    [Header("Local lookahead")]
    private int localRadius = 2;
    private float opponentWeight = 0.8f;

    private void InicializeDifficulty() 
    {

    }
    public void AnalyzeMoves(List<Unit> deck, List<Tile> grid, List<Tile> iaTiles)
    {
        bestMove = new BestMove { unit = null, tile = null, score = int.MinValue };
        foreach (var unit in deck)
        {
            foreach (var tile in iaTiles)
            {
                int score = EvaluateMove(unit, tile, grid);
                if (score > bestMove.score)
                {
                    bestMove.unit = unit;
                    bestMove.tile = tile;
                    bestMove.score = score;
                }
            }
        }
    }
    private int EvaluateMove(Unit unit, Tile tile, List<Tile> grid)
    {
        int score = 0;
        

        switch (unit)
        { 
            case Hero hero:
                
                break;

        }
        return score;


    }
}
