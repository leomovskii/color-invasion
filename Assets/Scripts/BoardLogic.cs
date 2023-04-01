using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BoardLogic {

	public static void SetAllCompletedToType(Tile[,] board, TileType type) {
		foreach (var tile in board)
			if (tile.completed)
				tile.type = type;
	}

	public static bool HasIncompletedTile(Tile[,] board) {
		foreach (var tile in board)
			if (!tile.completed)
				return true;
		return false;
	}

	public static void CompleteNewTiles(Tile[,] board, Tile startTile) {
		foreach (var tile in board)
			tile.visited = false;

		var newTiles = CollectAllSameColorNearby(board, startTile);
		foreach (var tile in newTiles)
			tile.completed = true;
	}

	private static List<Tile> CollectAllSameColorNearby(Tile[,] board, Tile tile) {
		var result = new List<Tile>();
		tile.visited = true;
		result.Add(tile);

		if (tile.coords.x > 0) {
			Tile left = board[tile.coords.x - 1, tile.coords.y];
			if (left.type == tile.type && !left.visited) {
				var leftNeighbors = CollectAllSameColorNearby(board, left);
				result = result.Union(leftNeighbors).ToList();
			}
		}

		if (tile.coords.x < board.GetLength(0) - 1) {
			Tile right = board[tile.coords.x + 1, tile.coords.y];
			if (right.type == tile.type && !right.visited) {
				var rightNeighbors = CollectAllSameColorNearby(board, right);
				result = result.Union(rightNeighbors).ToList();
			}
		}

		if (tile.coords.y > 0) {
			Tile bottom = board[tile.coords.x, tile.coords.y - 1];
			if (bottom.type == tile.type && !bottom.visited) {
				var bottomNeighbors = CollectAllSameColorNearby(board, bottom);
				result = result.Union(bottomNeighbors).ToList();
			}
		}

		if (tile.coords.y < board.GetLength(1) - 1) {
			Tile top = board[tile.coords.x, tile.coords.y + 1];
			if (top.type == tile.type && !top.visited) {
				var topNeighbors = CollectAllSameColorNearby(board, top);
				result = result.Union(topNeighbors).ToList();
			}
		}

		return result;
	}

	public static void BoardToUndo(Tile[,] board, TileUndoData[,] undoBoard) {
		for (int y = 0; y < board.GetLength(1); y++)
			for (int x = 0; x < board.GetLength(0); x++) {
				undoBoard[x, y].type = board[x, y].type;
				undoBoard[x, y].completed = board[x, y].completed;
			}
	}

	public static void UndoToBoard(Tile[,] board, TileUndoData[,] undoBoard) {
		for (int y = 0; y < board.GetLength(1); y++)
			for (int x = 0; x < board.GetLength(0); x++) {
				board[x, y].type = undoBoard[x, y].type;
				board[x, y].completed = undoBoard[x, y].completed;
			}
	}
}