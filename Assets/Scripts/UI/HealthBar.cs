using UnityEngine;
using UnityEngine.UI;

/**
 * Health bar for each player.
 */
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private CharacterAttributes _characterAttributes;

    private void Start()
    {
        _characterAttributes = GetComponentInParent<CharacterAttributes>();
        SetMaxHealth(CharacterAttributeItems.MAX_HEALTH);
    }

    private void LateUpdate()
    {
        if(_characterAttributes) UpdateHealth(_characterAttributes.characterAttributes.Health);
    }

    private void SetMaxHealth(float value)
    {
        slider.maxValue = value;
        slider.value = value;
        fill.color = gradient.Evaluate(1f);
    }

    private void UpdateHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}