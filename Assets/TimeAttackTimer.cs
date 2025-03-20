using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeAttackTimer : MonoBehaviour
{
    public Text timerText;
    public Text finishText; // UI text ที่จะแสดงข้อความ "Finish!"
    public Button startButton; // Start button
    private float timeElapsed; // Time counter
    private bool isTiming; // Is the timer running?
    private bool isFinishLineCrossed; // Did the player cross the finish line?

    public static TimeAttackTimer Instance;

    void Start()
    {
        timeElapsed = 0f;
        isTiming = false;
        isFinishLineCrossed = false;
        timerText.text = "00:00"; // Initial text display
        finishText.text = ""; // Clear finish text at the start
        startButton.gameObject.SetActive(true); // Make sure the button is active at the start
    }

    void Update()
    {
        if (isTiming && !isFinishLineCrossed) // Only update time if the game is running and player hasn't crossed the finish line
        {
            timeElapsed += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timeElapsed / 60); // Get the minutes part
            int seconds = Mathf.FloorToInt(timeElapsed % 60); // Get the seconds part
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Display time in MM:SS format
        }

        // Detect if the "R" key is pressed to reset the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame(); // Reset the entire game when 'R' is pressed
        }
    }

    public void StartTimer()
    {
        startButton.gameObject.SetActive(false); // Hide the Start button
        isTiming = true; // Start timing when button is pressed
    }

    public void StopTimer()
    {
        isTiming = false; // Stop timing when button is pressed
    }

    // Function to reset the entire game (reload the scene)
    private void ResetGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    // This method is called when the player crosses the finish line
    public void OnCrossFinishLine()
    {
        if (!isFinishLineCrossed) // Prevent multiple calls
        {
            isFinishLineCrossed = true; // Mark the finish line as crossed
            StopTimer(); // Stop the timer
            finishText.text = "Finish!"; // Display "Finish!"
            Debug.Log("Race Finished!");
        }
    }
}
