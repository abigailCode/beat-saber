using System.Collections;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour {
    public delegate void GameFinished();
    public static event GameFinished OnGameFinished;

    [SerializeField] TMP_Text _scoreText, _comboText, _doubleText;
    int score = -1;
    int combo = -1;
    int maxCombo = -1;

    void Start() {
        UpdateScore();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) IncreaseScore();
        if (Input.GetKeyDown(KeyCode.DownArrow)) FinishGame();
    }

    void OnEnable() => OnGameFinished += GoToRanking;

    void OnDisable() => OnGameFinished -= GoToRanking;

    void UpdateScore(int score = 0) {
        if (combo != 0 && combo % 10 == 0) StartCoroutine(ShowDouble());
        if (_doubleText.IsActive()) ++score;

        _scoreText.text = score.ToString();
        _comboText.text = (++combo).ToString();
    }

    void IncreaseScore() => UpdateScore(++score);

    void DecreaseScore() { if (score > 0) UpdateScore(--score); }

    void GoToRanking() {
        if (combo > maxCombo) maxCombo = combo;
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetInt("max_combo", maxCombo);

        SCManager.instance.LoadScene("Ranking");
        AudioManager.instance.UnloadSong();
    }

    public static void FinishGame() => OnGameFinished.Invoke();

    IEnumerator ShowDouble() {
        if (_doubleText.enabled) yield return null;
        _doubleText.gameObject.SetActive(true);
        yield return new WaitForSeconds(10);
        _doubleText.gameObject.SetActive(false);
    }

    void ResetCombo() {
        if (combo > maxCombo) maxCombo = combo;
        combo = 0;
        _comboText.text = combo.ToString();
    }
}