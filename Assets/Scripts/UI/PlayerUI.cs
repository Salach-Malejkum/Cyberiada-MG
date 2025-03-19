using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private UnitStats unitStats;
    [SerializeField] private Image HealthBarImage;

    private void Awake()
    {
        this.unitStats.OnUnitHealthUpdate += PlayerHealthUpdated;
    }

    private void OnDestroy()
    {
        this.unitStats.OnUnitHealthUpdate -= PlayerHealthUpdated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayerHealthUpdated(float newCurrHP, float newMaxHP)
    {
        if (newCurrHP > newMaxHP)
        {
            newCurrHP = newMaxHP;
        }
        this.HealthBarImage.fillAmount = newCurrHP / newMaxHP;
    }
}
