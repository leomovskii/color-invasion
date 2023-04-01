using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	private MenuStage _Stage;

	[SerializeField] private Game m_GameController;
	[Space(10)]
	[SerializeField] private GameObject m_PauseMenuUI;
	[SerializeField] private GameObject m_NewGameUI;
	[SerializeField] private GameObject m_HowToPlayUI;
	[SerializeField] private GameObject m_WinUI;

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape))
			OnBackPressed();
	}

	private void OnBackPressed() {
		if (_Stage == MenuStage.NewGame) {
			if (m_GameController.state == GameState.Endgame) {
				UpdateMenuStage(MenuStage.Win);
			} else {
				UpdateMenuStage(MenuStage.Pause);
			}
		} else if (_Stage == MenuStage.HowToPlay) {
			UpdateMenuStage(MenuStage.Pause);

		} else if (_Stage == MenuStage.Pause) {
			UpdateMenuStage(MenuStage.Closed);

		} else {
			OnQuit();
		}
	}

	public void SetPauseMenuShown(bool isShown) {
		UpdateMenuStage(isShown ? MenuStage.Pause : MenuStage.Closed);
	}

	public void SetNewGameMenuShown(bool isShown) {
		if (isShown)
			UpdateMenuStage(MenuStage.NewGame);
		else if (m_GameController.state == GameState.Endgame)
			UpdateMenuStage(MenuStage.Win);
		else
			UpdateMenuStage(MenuStage.Pause);
	}

	public void SetHowToPlayMenuShown(bool isShown) {
		UpdateMenuStage(isShown ? MenuStage.HowToPlay : MenuStage.Pause);
	}

	private void UpdateMenuStage(MenuStage nextStage) {
		_Stage = nextStage;
		m_PauseMenuUI.SetActive(_Stage == MenuStage.Pause);
		m_NewGameUI.SetActive(_Stage == MenuStage.NewGame);
		m_HowToPlayUI.SetActive(_Stage == MenuStage.HowToPlay);
		m_WinUI.SetActive(_Stage == MenuStage.Win);
	}

	public void OnQuit() {
		Application.Quit();
	}

	public void OnUndo() {
		m_GameController.Undo();
	}

	public void OnGameOver() {
		UpdateMenuStage(MenuStage.Win);
	}
}