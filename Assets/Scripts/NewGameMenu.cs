using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameMenu : MonoBehaviour {

	[SerializeField] private InputField m_SeedInput;
	[Space(10)]
	[SerializeField] private Slider m_ColorsCountSlider;
	[SerializeField] private GameObject m_YellowDisable;
	[SerializeField] private GameObject m_PurpleDisable;
	[Space(10)]
	[SerializeField] private Slider m_BoardSizeSlider;
	[SerializeField] private Text m_BoardSizeText;

	private void Start() {
		m_SeedInput.text = Prefs.seed;

		m_ColorsCountSlider.value = Prefs.colorsCount;
		m_YellowDisable.SetActive(Prefs.colorsCount < 5);
		m_PurpleDisable.SetActive(Prefs.colorsCount < 6);

		m_BoardSizeSlider.value = Prefs.boardSize;
	}

	public void OnSeedChanged(string value) {
		Prefs.seed = value;
	}

	public void OnColorsCountChanged(float value) {
		int count = (int) value;
		Prefs.colorsCount = count;
		m_ColorsCountSlider.value = count;
		m_YellowDisable.SetActive(count < 5);
		m_PurpleDisable.SetActive(count < 6);
	}

	public void OnBoardSizeChanged(float value) {
		int size = (int) value;
		Prefs.boardSize = size;
		m_BoardSizeSlider.value = size;
		m_BoardSizeText.text = Strings.SIZES[size];
	}

	public void OnPlayPressed() {
		SceneManager.LoadScene(0);
	}
}