using TMPro;
using UnityEngine;

public class TimerDebug : MonoBehaviour
{
    private TextMeshProUGUI timerUI;
    private float timer = 0;

    void Start()
    {
        timerUI = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        timerUI.text = timer.ToString();
    }
}
