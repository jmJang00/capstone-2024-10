using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class UI_SessionList : UI_Popup
{
    public enum GameObjects
    {
        RoomContent,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind<GameObject>(typeof(GameObjects));

        return true;
    }

    private UI_Lobby _lobby;

    public void SetInfo(UI_Lobby lobby)
    {
        _lobby = lobby;
    }

    public void RefreshSessionLIst(string keyword = "")
    {
        foreach (Transform child in GetObject((int)GameObjects.RoomContent).transform)
        {
            Destroy(child.gameObject);
        }

        foreach (SessionInfo session in Managers.NetworkMng.Sessions)
        {
            if (session.IsVisible)
            {
                if (!keyword.IsNullOrEmpty() && !session.Name.Contains(keyword))
                    continue;

                string roomName = session.Name;
                if (session.Properties.TryGetValue("password", out SessionProperty password) && password != "")
                {
                    roomName = "<sprite=0>   " + roomName;
                }
                else
                {
                    roomName = "       " + roomName;
                }

                UI_SessionEntry entry = Managers.UIMng.MakeSubItem<UI_SessionEntry>(GetObject((int)GameObjects.RoomContent).transform);
                entry.Init();
                var args = new SessionEntryArgs()
                {
                    RoomName = roomName,
                    CurrentMembers = session.PlayerCount,
                    MaxMembers = session.MaxPlayers,
                    IsOpen = session.IsOpen,
                };
                StartCoroutine(entry.SetInfo(args, _lobby));
                entry.OnClick = () => { JoinSession(session); };
            }
        }
    }

    private async void JoinSession(SessionInfo session)
    {
        if (session.Properties.TryGetValue("password", out SessionProperty password))
        {
            Managers.UIMng.ClosePopupUIUntil<UI_Lobby>();
            var popup = Managers.UIMng.ShowPopupUI<UI_JoinRoom>(parent: Managers.UIMng.PeekPopupUI<UI_Lobby>().transform);
            popup.SetInfo(session.Name, password, _lobby);
        }
        else
        {
            Managers.UIMng.ShowPopupUI<UI_RaycastBlock>();
            bool result = await Managers.NetworkMng.ConnectToSession(session.Name, null);
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
    }

    public void RefreshSessionLIst(List<LobbyManager.LobbyData> lobbyDataList)
    {
        foreach (Transform child in GetObject((int)GameObjects.RoomContent).transform)
        {
            Destroy(child.gameObject);
        }

        // 새로운 로비 목록 생성
        foreach (var lobbyData in lobbyDataList)
        {
            UI_SessionEntry entry = Managers.UIMng.MakeSubItem<UI_SessionEntry>(GetObject((int)GameObjects.RoomContent).transform);
            entry.Init();
            var args = new SessionEntryArgs()
            {
                RoomName = lobbyData.LobbyName,
                CurrentMembers = lobbyData.CurrentMembers,
                MaxMembers = lobbyData.MaxMembers,
                IsOpen = true,
            };
            StartCoroutine(entry.SetInfo(args, _lobby));

            // 참가 버튼 이벤트 설정
            entry.OnClick = () => { LobbyManager.Instance.JoinLobby(lobbyData.LobbyId); };
        }
    }
}
