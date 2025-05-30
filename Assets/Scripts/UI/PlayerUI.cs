using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private UnitStats unitStats;
    [SerializeField] private Image HealthBarImage;
    [SerializeField] private TextMeshProUGUI hintText;

    private void Awake()
    {
        this.unitStats.OnUnitHealthUpdate += PlayerHealthUpdated;
        HintTriggerZone.OnMessageTriggered += HandleMessages;
    }

    private void OnDestroy()
    {
        this.unitStats.OnUnitHealthUpdate -= PlayerHealthUpdated;
        HintTriggerZone.OnMessageTriggered -= HandleMessages;
    }

    private void PlayerHealthUpdated(float newCurrHP, float newMaxHP)
    {
        if (newCurrHP > newMaxHP)
        {
            newCurrHP = newMaxHP;
        }
        this.HealthBarImage.fillAmount = newCurrHP / newMaxHP;
    }

    public void UpdateMessage(string message)
    {
        if (hintText != null)
            hintText.text = message;
    }

    private void SetShortJumpText() => UpdateMessage("Press Space to jump");
    private void SetLongJumpText() => UpdateMessage("Hold Space to jump higher");
    private void SetRunningJumpText() => UpdateMessage("Run to one side and press and hold Space to jump even higher");
    private void SetMovementext() => UpdateMessage("Use WASD to move");
    private void HandleMessages(string message)
    {
        switch (message)
        {
            case "hint_short_jump":
                SetShortJumpText();
                break;
            case "hint_long_jump":
                SetLongJumpText();
                break;
            case "hint_running_jump":
                SetRunningJumpText();
                break;
            case "hint_movement":
                SetMovementext();
                break;
            default:
                hintText.text = "";
                break;
        }
    }
}
