using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public string scoreWord = "Score: ";
    public int scoreValue = 0;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore(int amount)
    {
        scoreValue += amount;
        scoreText.text = $"{scoreWord}{scoreValue}";
    }
}
