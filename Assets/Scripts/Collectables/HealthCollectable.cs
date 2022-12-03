using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : BaseCollectable
{
    bool isTriggered = false;

    public int amount = 50;
    public override bool TryCollect(Collider c)
    {
        var characterAttribute = c.gameObject.GetComponent<CharacterAttributes>();
        if (characterAttribute != null && !isTriggered)
        {
            isTriggered = true;
            Destroy(gameObject); // Remove health pack

            // Don't modify health if value is already at max.
            if (characterAttribute.characterAttributes.Health >= CharacterAttributeItems.MAX_HEALTH) return true;

            var healthAmountToAdd = System.Math.Min(
                CharacterAttributeItems.MAX_HEALTH - characterAttribute.characterAttributes.Health,
                amount
            );

            characterAttribute.AddModifier(new HealthModifier(
                healthAmount: healthAmountToAdd,
                trigger: ModifierTrigger.ON_ADD
            )); // Add a +50 health modifier to the player

            return true;
        }

        return false;
    }

    void OnTriggerEnter(Collider c)
    {
        if (disableTrigger) return;

        if (c.gameObject.GetComponent<CharacterAttributes>() != null && !isTriggered)
        {
            isTriggered = true;
            Destroy(this.gameObject); // Remove health pack
            c.gameObject.GetComponent<CharacterAttributes>().AddModifier(new HealthModifier(
                healthAmount: amount,
                trigger: ModifierTrigger.ON_ADD
            )); // Add a +50 health modifier to the player
        }
    }
}
