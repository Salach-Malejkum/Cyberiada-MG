using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private UnitStats unitStats;
    [SerializeField] private Image HealthBarImage;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private float TimeToHideHint = 4f;

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

    private Coroutine stopText;
    public void UpdateMessage(string message)
    {
        if (stopText != null)
        {
            StopCoroutine(stopText);
        }
        
        if (hintText != null)
        {
            hintText.text = message;
            stopText = StartCoroutine(StopDisplay());
        }
    }

    public IEnumerator StopDisplay()
    {
        yield return new WaitForSeconds(TimeToHideHint);
        hintText.text = "";
    }

    private void SetShortJumpText() => UpdateMessage("Press [Space] to jump");
    private void SetLongJumpText() => UpdateMessage("Hold [Space] to jump higher");
    private void SetRunningJumpText() => UpdateMessage("Run to one side, then press and hold [Space] to jump even higher and furhter.");
    private void SetMovementText() => UpdateMessage("Use [WASD] to move");
    private void SetEnterablePlatformText() => UpdateMessage("You can enter wooden platforms from below and drop from them using [S]");
    private void SetSpikesText() => UpdateMessage("Be careful! Spikes deal damage when you touch them, better be safe.");
    private void SetWallJumpText() => UpdateMessage("When touching the wall, press [SPACE] to jump off it.");
    private void SetTalkNpcText() => UpdateMessage("Press [F] to talk to NPCs");
    private void SetBonfireText() => UpdateMessage("Press [F] to activate checkpoint. Checkpoints restore HP and respawn enemies.");

    private void HandleMessages(GameObject gameObject, string message)
    {
        if (!GameManager.instance.finishedHints.Contains(message))
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
                    SetMovementText();
                    break;
                case "hint_enterable_platform":
                    SetEnterablePlatformText();
                    break;
                case "hint_spikes":
                    SetSpikesText();
                    break;
                case "hint_wall_jump":
                    SetWallJumpText();
                    gameObject.GetComponent<PlayerMove>().SetCanWallJump(true); // TODO: This is a temporary solution, should be removed later
                    break;
                case "hint_talk_npc":
                    SetTalkNpcText();
                    break;
                case "hint_bonfire":
                    SetBonfireText();
                    break;
                default:
                    hintText.text = "";
                    break;
            }
            GameManager.instance.finishedHints.Add(message);
        }
    }
}
