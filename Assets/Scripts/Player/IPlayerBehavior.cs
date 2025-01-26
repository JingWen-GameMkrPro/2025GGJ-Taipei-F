using System.Collections.Generic;
using UnityEngine;

public interface IPlayerBehavior
{
    public void HandleMove(PlayerController _pController) ;

    public void HandleAttack(PlayerController _pController);

    public void HandleHint(PlayerController _pController);

    public void TriggerHit(PlayerController _pController, List<ItemManager.InteractType> typeList, int ownerIndex);
}
