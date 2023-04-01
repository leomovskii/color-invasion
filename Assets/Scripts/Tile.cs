using UnityEngine;

public class Tile : MonoBehaviour {

	private static readonly Color[] colors = {
		new Color(0.22f, 0.54f, 0.27f),
		new Color(0.79f, 0.54f, 0.26f),
		new Color(0.30f, 0.61f, 0.85f),
		new Color(0.60f, 0.20f, 0.20f),
		new Color(0.86f, 0.85f, 0.38f),
		new Color(0.66f, 0.24f, 0.74f)
	};

	TileType _Type;

	public SpriteRenderer spriteRenderer;
	public Vector2Int coords;
	public bool completed;
	public bool visited;

	public TileType type {
		get => _Type;
		set {
			_Type = value;
			spriteRenderer.color = colors[(int) value];
		}
	}
}