using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefs {

	private static readonly string keySeed = "seed";
	private static readonly string keyColorsCount = "colors_count";
	private static readonly string keyBoardSize = "board_size";

	public static string seed {
		get => PlayerPrefs.GetString(keySeed, "");
		set => PlayerPrefs.SetString(keySeed, value);
	}

	public static int colorsCount {
		get => PlayerPrefs.GetInt(keyColorsCount, 5);
		set => PlayerPrefs.SetInt(keyColorsCount, value);
	}

	public static int boardSize {
		get => PlayerPrefs.GetInt(keyBoardSize, 1);
		set => PlayerPrefs.SetInt(keyBoardSize, value);
	}
}