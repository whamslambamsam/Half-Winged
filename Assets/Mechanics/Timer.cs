using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public int duration = 45000;
    public int timeRemaining;
    public bool isCountingDown = false;
    public int timeSeconds;

    private void Update()
    {
        if (!isCountingDown)
        {
            isCountingDown = true;
            timeRemaining = duration;
        }

        if (isCountingDown)
        {
            timeRemaining--;
            //timeSeconds = timeRemaining / 1000;
            timerText.text = timeRemaining.ToString();
        }
    }
}
