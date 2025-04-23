
using System;
using UnityEngine;
public interface IHoverable
{
    void OnHoverEnter();
    void OnHoverExit();
}
public interface IUIRenderable
{
    void BindToUI(GameObject uiRoot);
}
public interface IGameUI
{
    void Show();
    void Hide();
}
// 로비 버튼, 재화 표시
public interface ILobbyUI : IGameUI
{
    void UpdateCurrency(int gold, int gem);
}

// 주사위 굴리기, 이동 애니메이션, 현재 위치 표시
public interface IBoardUI : IGameUI
{
    void RollDice(int value);
    void MovePlayer(int steps);
    void UpdatePlayerPosition(int tileIndex);
}

// 턴 정보, 스킬 버튼, HP 표시
public interface IBattleUI : IGameUI
{
    void SetTurnInfo(string currentActor);
    void UpdatePlayerHP(float current, float max);
    void UpdateSkillButtons(bool[] activeStates);
}

// 승패 결과, 보상 표시
public interface IResultUI : IGameUI
{
    void ShowResult(bool isVictory);
    void DisplayRewards(string[] rewardNames);
}

// 무기 강화, 합성
public interface IReinforceUI : IGameUI
{
    void ShowReinforceOptions();
    void PlayReinforceEffect(bool success);
}

// 아이템/장비 목록
public interface IInventoryUI : IGameUI
{
    void RefreshInventory();
    void SelectItem(int index);
}
 
// 상점 목록, 재화 표시
public interface IShopUI : IGameUI
{
   //void ShowItems(ShopItemData[] items);
    void UpdateCurrencyDisplay(int gold);
}

// 하단 간단한 알림
public interface IToastUI : IGameUI
{
    void ShowToast(string message, float duration = 2f);
}

// 공통 팝업, 확인/취소
public interface IPopupUI : IGameUI
{
    void ShowPopup(string message, Action onConfirm, Action onCancel = null);
}

// 상시 노출 재화, 스테미나 등 HUD
public interface IHUDUI : IGameUI
{
    void UpdateGold(int amount);
    void UpdateStamina(int current, int max);
}
public interface ITitleUI : IGameUI
{
     event Action OnContinueClicked;
}