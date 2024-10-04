using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using System;

public class SessionEntryArgs
{
    public SessionInfo _session { get; set; }
    public string RoomName { get; set; }
    public int CurrentMembers { get; set; }
    public int MaxMembers { get; set; }
    public bool IsOpen { get; set; }
}

public class UI_SessionEntry : UI_Base
{
    #region UI 목록들
    public enum Buttons
    {
        JoinButton,
    }

    public enum Images
    {
    }

    public enum Texts
    {
        RoomName,
        PlayerCount,
    }
    #endregion

    private UI_Lobby _lobby;
    public Action OnClick { get; set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TMP_Text>(typeof(Texts));

        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        GetButton(Buttons.JoinButton).onClick.AddListener(() => OnClick.Invoke());

        return true;
    }

    public IEnumerator SetInfo(SessionEntryArgs args, UI_Lobby lobby)
    {
        yield return null;

        _lobby = lobby;
        string roomName = args.RoomName;

        GetText(Texts.RoomName).text = roomName;
        GetText(Texts.PlayerCount).text = args.CurrentMembers + "/" + args.MaxMembers;

        if (args.IsOpen == false || args.CurrentMembers >= args.MaxMembers)
        {
            GetButton(Buttons.JoinButton).interactable = false;
        }
        else
        {
            GetButton(Buttons.JoinButton).interactable = true;
        }
    }
}
