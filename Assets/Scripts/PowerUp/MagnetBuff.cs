using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/MagnetBuff")]
public class MagnetBuff : PowerUp
{
    public override void ApplyPowerUp(GameObject target, float duration)
    {
        target.GetComponent<HandlePowerUp>().MagnetBuff(duration);
    }
}
