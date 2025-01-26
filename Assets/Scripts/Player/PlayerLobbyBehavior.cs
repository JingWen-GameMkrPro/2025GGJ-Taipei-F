using System.Collections.Generic;
using System.Diagnostics;
using GamePlay;
using Utility;

public class PlayerLobbyBehavior : IPlayerBehavior
{
    public void HandleMove(PlayerController _pController) {
        if (_pController.GetCharacterController() != null) {
            _pController.GetCharacterController().Move(_pController.GetData().move);
        }

        _pController.GetPlayerAnimation().UpdateState(_pController.GetData().move, _pController.GetData().faceType);
    }

    public void HandleAttack(PlayerController _pController) {
        // Ready!
        if(_pController.GetData().matchStatus == GamePlay.MatchStatus.NotReady){
            if(SystemService.TryGetService<IMatchManager>(out IMatchManager matchManager) == false){
                return;
            }
            UnityEngine.Debug.Log("Ready! " + _pController.GetData().index);
            matchManager.Ready(_pController.GetData().index);
        }else if(_pController.GetData().index == 0 && _pController.GetData().matchStatus == GamePlay.MatchStatus.Ready){
            if(SystemService.TryGetService<IMatchManager>(out IMatchManager matchManager) == false){
                return;
            }
            UnityEngine.Debug.Log("Start!");
            matchManager.Start();
        }
    }

    public void HandleDamage(PlayerController _pController, int v) {
        /*Do Nothing in Lobby*/
    }

    public void HandleSpeedModify(PlayerController _pController, int v) {
        /*Do Nothing in Lobby*/
    }

    public void HandleHint(PlayerController _pController) {
        if (_pController.defenceHint != null) {
            _pController.defenceHint.SetActive(false);
        }
        if (_pController.dieHint != null) {
            _pController.dieHint.SetActive(false);
        }
    }
    public void TriggerHit(PlayerController _pController, List<ItemManager.InteractType> typeList) {
        /*Do Nothing in Lobby*/
    }
}
