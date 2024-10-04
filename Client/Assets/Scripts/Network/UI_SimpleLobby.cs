using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SimpleLobby : UI_Base
{
    private LobbyManager lobbyManager;
    private TMP_Dropdown lobbyDropdown; // Dropdown UI 요소
    private List<LobbyManager.LobbyData> currentLobbyDataList = new List<LobbyManager.LobbyData>(); // 로비 데이터 저장용 리스트

    #region Enums

    public enum Buttons
    {
        Btn_QuickStart,
        Btn_CreateGame,
        Btn_RefreshSession,
        Btn_GameTutorial,
        SearchIcon,
    }

    public enum Inputs
    {
        Search,
    }

    enum GameObjects
    {
        Right
    }
    #endregion

    public Transform Right {  get; set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind<Button>(typeof(Buttons));
        Bind<TMP_InputField>(typeof(Inputs));
        Bind<GameObject>(typeof(GameObjects));

        Right = GetObject(GameObjects.Right).transform;
        var popup = Managers.UIMng.ShowPopupUI<UI_SessionList>(parent : Right);
        popup.Init();
        popup.RefreshSessionLIst();

        lobbyManager = FindObjectOfType<LobbyManager>();
        if (lobbyManager == null)
        {
            Debug.LogError("LobbyManager not found in the scene.");
        }

        Get<Button>(Buttons.Btn_CreateGame).onClick.AddListener(OnClickCreateLobby);
        Get<Button>(Buttons.Btn_RefreshSession).onClick.AddListener(OnClickSearchLobby);

        LobbyManager.Instance.OnLobbyListUpdated += UpdateLobbyList;

        return true;
    }


    public void SetInfo(UI_LobbyController controller)
    {
        foreach (int i in Enum.GetValues(typeof(Buttons)))
        {
            BindEvent(GetButton(i).gameObject, (e) => {
                if (GetButton(i).interactable)
                    controller?.PlayHover();
            }, Define.UIEvent.PointerEnter);
        }
    }

    public void Refresh()
    {
        StartCoroutine(RefreshWait());
    }

    private IEnumerator RefreshWait()
    {
        Managers.UIMng.ClosePopupUIUntil<UI_Lobby>();
        var popup = Managers.UIMng.ShowPopupUI<UI_SessionList>(parent : Right);
        popup.Init();

        GetButton((int)Buttons.Btn_RefreshSession).interactable = false;
        popup.RefreshSessionLIst(Get<TMP_InputField>(Inputs.Search).text.Trim());
        yield return new WaitForSeconds(0.2f);
        GetButton((int)Buttons.Btn_RefreshSession).interactable = true;
    }

    private async void EnterGame()
    {
        Managers.UIMng.ShowPopupUI<UI_RaycastBlock>();
        bool result = await Managers.NetworkMng.ConnectToAnySession();
        Managers.UIMng.ClosePopupUI();

        if (result)
        {
            Managers.Clear();
            Managers.UIMng.ShowPanelUI<UI_Loading>();
        }
        else
        {
            Managers.UIMng.ClosePopupUIUntil<UI_Lobby>();
            var lobby = Managers.UIMng.PeekPopupUI<UI_Lobby>();
            Managers.UIMng.ShowPopupUI<UI_Warning>(parent : lobby.transform);
        }
    }

    private void StartTutorial()
    {
    }

    // 로비 목록을 갱신하는 메서드
    private void UpdateLobbyList(List<LobbyManager.LobbyData> lobbyDataList)
    {
        Managers.UIMng.CloseAllPopupUI();
        var popup = Managers.UIMng.ShowPopupUI<UI_SessionList>(parent : Right);
        popup.Init();

        popup.RefreshSessionLIst(lobbyDataList);
    }

    // 로비 선택 시 호출되는 메서드
    private void OnLobbySelected(int index)
    {
        if (index < 0 || index >= currentLobbyDataList.Count)
        {
            Debug.LogWarning("Invalid lobby index selected.");
            return;
        }

        LobbyManager.LobbyData selectedLobby = currentLobbyDataList[index];
        lobbyManager.JoinLobby(selectedLobby.LobbyId);
    }

    public void OnClickCreateLobby()
    {
        if (lobbyManager != null)
        {
            lobbyManager.CreateLobby();
        }
    }

    public void OnClickSearchLobby()
    {
        if (lobbyManager != null)
        {
            lobbyManager.SearchLobbies();
        }
    }

    public void OnClickStartGame()
    {
        /*
        if (HostManager.Instance.AreAllPlayersReady())
        {
            HostManager.Instance.InitializeHost(LobbyManager.Instance.CurrentLobby);
        }
        else
        {
            Debug.Log("Not all players are ready.");
        }
        */
    }

    public void OnClickReadyButton(bool isReady)
    {
        /*
        ClientManager.Instance.SetReadyState(isReady);
        */
    }
}
