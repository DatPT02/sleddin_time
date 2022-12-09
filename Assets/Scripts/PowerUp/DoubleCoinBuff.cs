using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/DoubleCoin")]
public class DoubleCoinBuff : PowerUp
{
    public override void ApplyPowerUp(GameObject target, float duration)
    {
        target.GetComponent<HandlePowerUp>().DoubleCoin(duration);
    }
}
