using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

public struct TileUndoData {
	public TileType type;
	public bool completed;
}

public class Game : MonoBehaviour {

	private Tile[,] _Board;
	private GameObject _HelpArrow;
	private GameState _GameState;
	private TileType _ActualType;

	private TileUndoData[,] _UndoBoard;
	private TileType _UndoActualType;
	private bool _CanUndo;

	public Tile tilePrefab;
	public GameObject helpArrowPrefab;
	[Space(10)]
	[SerializeField] private Menu m_MenuController;
	[Space(10)]
	public GameObject panel4Colors;
	public GameObject panel5Colors;
	public GameObject panel6Colors;

	public GameState state => _GameState;

	void Start() {
		InitSeed();
		InitColors();

		var sizeData = Constants.SIZES[Prefs.boardSize];

		int xSize = sizeData.x;
		int ySize = sizeData.y;

		Camera.main.orthographicSize = sizeData.z;

		var leftBottomPoint = GetLeftBottomCoords(xSize, ySize);

		MakeBoard(xSize, ySize, leftBottomPoint);
		InitBoard();

		_UndoBoard = new TileUndoData[xSize, ySize];

		var helpArrowPosition = new Vector3(leftBottomPoint.x, leftBottomPoint.y - 1, 0);
		_HelpArrow = Instantiate(helpArrowPrefab, helpArrowPosition, Quaternion.identity);
	}

	private Vector2 GetLeftBottomCoords(float xSize, float ySize) {
		return new Vector2(-xSize / 2 + 0.5f, -ySize / 2 + 0.5f);
	}

	private void InitSeed() {
		Random.InitState(Prefs.seed.Length > 0 ? Int32.Parse(Prefs.seed) : (int) DateTime.Now.Ticks);
	}

	private void InitColors() {
		if (Prefs.colorsCount == 4)
			panel4Colors.SetActive(true);

		else if (Prefs.colorsCount == 5)
			panel5Colors.SetActive(true);

		else if (Prefs.colorsCount == 6)
			panel6Colors.SetActive(true);
	}

	private void MakeBoard(int xSize, int ySize, Vector2 leftBottom) {
		_Board = new Tile[xSize, ySize];
		for (int y = 0; y < ySize; y++)
			for (int x = 0; x < xSize; x++) {
				Vector3 spawnPos = new Vector3(x + leftBottom.x, y + leftBottom.y, 0);
				_Board[x, y] = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
				_Board[x, y].coords = new Vector2Int(x, y);
			}
	}

	private void InitBoard() {
		foreach (var tile in _Board)
			tile.type = (TileType) Random.Range(0, Prefs.colorsCount);

		while (_Board[0, 0].type == _Board[1, 0].type || _Board[0, 0].type == _Board[0, 1].type)
			_Board[0, 0].type = (TileType) Random.Range(0, Prefs.colorsCount);

		_Board[0, 0].completed = true;
		_ActualType = _Board[0, 0].type;
	}

	public void PressColor(int colorIndex) {
		if (_GameState == GameState.Endgame)
			return;

		TileType nextType = (TileType) colorIndex;
		if (nextType == _ActualType)
			return;

		if (_GameState == GameState.Pregame) {
			Destroy(_HelpArrow);
			_GameState = GameState.Ingame;
		}

		BoardLogic.BoardToUndo(_Board, _UndoBoard);
		_UndoActualType = _ActualType;
		_CanUndo = true;

		BoardLogic.SetAllCompletedToType(_Board, nextType);
		BoardLogic.CompleteNewTiles(_Board, _Board[0, 0]);
		_ActualType = nextType;

		if (!BoardLogic.HasIncompletedTile(_Board)) {
			_GameState = GameState.Endgame;
			m_MenuController.OnGameOver();
		}
	}

	public void Undo() {
		if (_CanUndo) {
			BoardLogic.UndoToBoard(_Board, _UndoBoard);
			_ActualType = _UndoActualType;
			_CanUndo = false;
		}
	}
}