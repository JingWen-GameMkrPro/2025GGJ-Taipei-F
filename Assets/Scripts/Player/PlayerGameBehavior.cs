using System.Collections.Generic;
using GamePlay;
using UnityEngine;
using Utility;

public class PlayerGameBehavior : IPlayerBehavior{
    public void HandleMove(PlayerController _pController) {
        if (_pController.GetCharacterController() != null) {
            _pController.GetCharacterController().Move(_pController.GetData().move);
        }

        _pController.GetPlayerAnimation().UpdateState(_pController.GetData().move, _pController.GetData().faceType, _pController.GetData().IsDied());
    }

    public void HandleAttack(PlayerController _pController) {
        ItemManager.ItemInfo info = new ItemManager.ItemInfo();
        info.PlayerIndex = _pController.GetData().index;
        info.Owner = _pController.gameObject;
        info.Position = _pController.GetFirePointList()[_pController.GetData().faceType].position;
        info.Direction = _pController.GetData().faceto;
        info.Speed = 15;
        ItemManager.Instance.UseItem(info);
        if(SystemService.TryGetService<IBattleManager>(out IBattleManager battleManager) == true){
            battleManager.UseItem(_pController.GetData().index, info);
        }
    }

    public void HandleDamage(PlayerController _pController, int v, int ownerIndex = -1) {
        if(SystemService.TryGetService<IBattleManager>(out IBattleManager battleManager) == false){
            return;
        }
        _pController.GetPlayerAnimation().PlayHurtAnimation();
        battleManager.DoDamage(ownerIndex, _pController.GetData().index, v);
    }

    public void HandleSpeedModify(PlayerController _pController, int v) {
        _pController.GetData().ModifySpeed(v);
    }

    public void HandleHint(PlayerController _pController) {
        if (_pController.defenceHint != null) {
            _pController.defenceHint.SetActive(_pController.GetData().IsDefencing());
        }
        if (_pController.dieHint != null) {
            _pController.dieHint.SetActive(_pController.GetData().IsDied());
        }
    }
    public void TriggerHit(PlayerController _pController, List<ItemManager.InteractType> typeList, int ownerIndex) {
        if (_pController.GetData().IsDied() || !_pController.GetData().isInitlized) {
            return;
        }
        foreach (ItemManager.InteractType type in typeList) {
            switch (type) {
                case ItemManager.InteractType.Damage:
                    HandleDamage(_pController, -40, ownerIndex);
                    break;
                case ItemManager.InteractType.Boom:
                    HandleDamage(_pController, -60, ownerIndex);
                    break;
                case ItemManager.InteractType.SpeedDown:
                    HandleSpeedModify(_pController, -5);
                    break;
                case ItemManager.InteractType.SpeedUp:
                    HandleSpeedModify(_pController, 5);
                    break;
            }
        }
    }
}
