using UnityEngine;
using System;
using Chess.Scripts.Core;

public class ChessPossibleMovesCalculator : MonoBehaviour{
    public enum PieceType{
        King,
        Queen,
        Bishop,
        Knight,
        Rook,
        Pawn
    }

    [SerializeField] private PieceType _type;
    [SerializeField] private string _tag;


    private void OnMouseDown(){
        ChessBoardPlacementHandler.Instance.ClearHighlights();
        HighlightPossibleMoves();
    }

    private void HighlightPossibleMoves(){
        switch (_type){
            case PieceType.King:
                HighlightKingMoves();
                break;
            case PieceType.Queen:
                HighlightQueenMoves();
                break;
            case PieceType.Bishop:
                HighlightBishopMoves();
                break;
            case PieceType.Knight:
                HighlightKnightMoves();
                break;
            case PieceType.Rook:
                HighlightRookMoves();
                break;
            case PieceType.Pawn:
                HighlightPawnMoves();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void HighlightKingMoves(){
        HighlightKingMove(- 1, - 1);
        HighlightKingMove(- 1, 0);
        HighlightKingMove(- 1, 1);
        HighlightKingMove(0, - 1);
        HighlightKingMove(0, 1);
        HighlightKingMove(1, - 1);
        HighlightKingMove(1, 0);
        HighlightKingMove(1, 1);
    }

    private void HighlightKingMove(int x, int y){
        int row = GetComponent<ChessPlayerPlacementHandler>().row;
        int col = GetComponent<ChessPlayerPlacementHandler>().column;

        if (IsInvalidTile(row + x, col + y)) return;
        if (ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponent(row + x, col + y, _tag)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row + x, col + y);
    }

    private void HighlightQueenMoves(){
        HighlightBishopMoves();
        HighlightRookMoves();
    }

    private void HighlightBishopMoves(){
        var chessPlayerPlacementHandler = GetComponent<ChessPlayerPlacementHandler>();
        var bishopCol = chessPlayerPlacementHandler.column;
        var bishopRow = chessPlayerPlacementHandler.row;

        ChessBoardPlacementHandler.Instance.GetTile(bishopRow, bishopCol);

        var row = bishopRow;
        var col = bishopCol;

        HighlightMovesInDirection(row, col, -1, -1);
        HighlightMovesInDirection(row, col, -1, 1);
        HighlightMovesInDirection(row, col, 1, -1);
        HighlightMovesInDirection(row, col, 1, 1);
    }

    private void HighlightMovesInDirection(int row, int col, int rowDirection, int colDirection){
        var curRow = row + rowDirection;
        var curCol = col + colDirection;
        while (IsValidTile(curRow, curCol)){
            if (ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponent(curRow, curCol, _tag)){
                return;
            }
            ChessBoardPlacementHandler.Instance.Highlight(curRow, curCol);
            curRow += rowDirection;
            curCol += colDirection;
        }
    }

    private void HighlightKnightMoves(){
        HighlightKnightMove(2, 1);
        HighlightKnightMove(2, - 1);
        HighlightKnightMove(- 2, 1);
        HighlightKnightMove(- 2, - 1);
        HighlightKnightMove(1, 2);
        HighlightKnightMove(1, - 2);
        HighlightKnightMove(- 1, 2);
        HighlightKnightMove(- 1, - 2);
    }

    private void HighlightKnightMove(int x, int y){
        var chessPlayerPlacementHandler = GetComponent<ChessPlayerPlacementHandler>();
        var knightCol = chessPlayerPlacementHandler.column;
        var knightRow = chessPlayerPlacementHandler.row;

        ChessBoardPlacementHandler.Instance.GetTile(knightRow, knightCol);

        if (IsValidTile(knightRow + x, knightCol + y)){
            if (!ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponent(knightRow + x, knightCol + y, _tag)){
                ChessBoardPlacementHandler.Instance.Highlight(knightRow + x, knightCol + y);
            }
        }
    }

    private void HighlightRookMoves(){
        int row = GetComponent<ChessPlayerPlacementHandler>().row;
        int col = GetComponent<ChessPlayerPlacementHandler>().column;

        HighlightMovesInDirection(row, col, -1, 0);
        HighlightMovesInDirection(row, col, 1, 0);
        HighlightMovesInDirection(row, col, 0, -1);
        HighlightMovesInDirection(row, col, 0, 1);
    }

    private void HighlightPawnMoves(){
        int row = GetComponent<ChessPlayerPlacementHandler>().row;
        int col = GetComponent<ChessPlayerPlacementHandler>().column;

        if (_tag == "White"){
            HighlightWhitePawnMove(row - 1, col);
            HighlightWhitePawnAttackMove(row - 1, col - 1);
            HighlightWhitePawnAttackMove(row - 1, col + 1);
            if (row == 6) HighlightWhitePawnMove(row - 2, col);
        }
        else if (_tag == "Black"){
            HighlightBlackPawnMove(row + 1, col);
            HighlightBlackPawnAttackMove(row + 1, col - 1);
            HighlightBlackPawnAttackMove(row + 1, col + 1);
            if (row == 1) HighlightBlackPawnMove(row + 2, col);
        }
    }

    private void HighlightWhitePawnMove(int row, int col){
        if (IsInvalidTile(row, col)) return;
        if (!ChessBoardPlacementHandler.Instance.IsTileEmpty(row, col)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }

    private void HighlightBlackPawnMove(int row, int col){
        if (IsInvalidTile(row, col)) return;
        if (!ChessBoardPlacementHandler.Instance.IsTileEmpty(row, col)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }

    private void HighlightWhitePawnAttackMove(int row, int col){
        if (IsInvalidTile(row, col)) return;
        if (!ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponent(row, col, _tag)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }

    private void HighlightBlackPawnAttackMove(int row, int col){
        if (IsInvalidTile(row, col)) return;
        if (!ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponent(row, col, _tag)) return;
        ChessBoardPlacementHandler.Instance.Highlight(row, col);
    }

    private bool IsInvalidTile(int row, int col){
        return !IsValidTile(row, col) || ChessBoardPlacementHandler.Instance.IsTileOccupiedByOpponent(row, col, _tag);
    }

    private bool IsValidTile(int row, int col){
        return row >= 0 && row <= 7 && col >= 0 && col <= 7;
    }
}