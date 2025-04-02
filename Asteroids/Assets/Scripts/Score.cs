using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>(); // Get the TextMeshProUGUI component attached to this GameObject
    }

    public void UpdateMyScore(int score){
        scoreText.text = score.ToString("00000000");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
