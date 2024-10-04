using UnityEngine;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
public class LobbyManager : MonoBehaviour
{
    // Singleton 패턴 적용
    public static LobbyManager Instance { get; private set; }

    // 콜백 핸들러
    protected Callback<LobbyCreated_t> m_LobbyCreated;
    protected Callback<LobbyMatchList_t> m_LobbyList;
    protected Callback<LobbyEnter_t> m_LobbyEnter;
    protected Callback<LobbyChatUpdate_t> m_LobbyChatUpdate;
    protected Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested;
    protected Callback<LobbyDataUpdate_t> m_LobbyDataUpdate;

    // 현재 로비 ID
    public CSteamID CurrentLobbyId { get; private set; }

    // 로비 목록을 저장하는 리스트
    public List<CSteamID> LobbyList { get; private set; } = new List<CSteamID>();

    // 로비 목록 수신 이벤트
    public event Action<List<LobbyData>> OnLobbyListUpdated;

    // 로비 데이터 구조체
    public struct LobbyData
    {
        public CSteamID LobbyId;
        public string LobbyName;
        public int CurrentMembers;
        public int MaxMembers;
    }

    private void Awake()
    {
        // Singleton 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 콜백 등록
            m_LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            m_LobbyList = Callback<LobbyMatchList_t>.Create(OnLobbyListReceived);
            m_LobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            m_LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
            m_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            m_LobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region 로비 생성
    public void CreateLobby(int maxMembers = 4)
    {
        SteamAPICall_t call = SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, maxMembers);
        // 콜백은 이미 등록되어 있으므로 별도 처리가 필요하지 않습니다.
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult == EResult.k_EResultOK)
        {
            CurrentLobbyId = new CSteamID(callback.m_ulSteamIDLobby);
            Debug.Log("Lobby created successfully. Lobby ID: " + CurrentLobbyId);

            // 로비 데이터 설정
            SteamMatchmaking.SetLobbyData(CurrentLobbyId, "name", SteamFriends.GetPersonaName() + "'s Lobby");
            SteamMatchmaking.SetLobbyData(CurrentLobbyId, "host_steamid", SteamUser.GetSteamID().ToString());
        }
        else
        {
            Debug.LogError("Failed to create lobby. Error code: " + callback.m_eResult);
        }
    }
    #endregion

    #region 로비 검색
    public void SearchLobbies()
    {
        SteamAPICall_t call = SteamMatchmaking.RequestLobbyList();
        // 콜백은 이미 등록되어 있으므로 별도 처리가 필요하지 않습니다.
    }

    private void OnLobbyListReceived(LobbyMatchList_t callback)
    {
        Debug.Log("Number of lobbies found: " + callback.m_nLobbiesMatching);

        LobbyList.Clear();
        List<LobbyData> lobbyDataList = new List<LobbyData>();

        for (int i = 0; i < callback.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyId = SteamMatchmaking.GetLobbyByIndex(i);
            LobbyList.Add(lobbyId);

            string lobbyName = SteamMatchmaking.GetLobbyData(lobbyId, "name");
            int currentMembers = SteamMatchmaking.GetNumLobbyMembers(lobbyId);
            int maxMembers = SteamMatchmaking.GetLobbyMemberLimit(lobbyId);

            LobbyData data = new LobbyData()
            {
                LobbyId = lobbyId,
                LobbyName = lobbyName,
                CurrentMembers = currentMembers,
                MaxMembers = maxMembers
            };

            lobbyDataList.Add(data);

            Debug.Log($"Lobby ID: {lobbyId}, Name: {lobbyName}, Members: {currentMembers}/{maxMembers}");
        }

        // 로비 목록 업데이트 이벤트 호출
        OnLobbyListUpdated?.Invoke(lobbyDataList);
    }
    #endregion

    #region 로비 참가
    public void JoinLobby(CSteamID lobbyId)
    {
        SteamMatchmaking.JoinLobby(lobbyId);
        // 콜백은 이미 등록되어 있으므로 별도 처리가 필요하지 않습니다.
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        CurrentLobbyId = new CSteamID(callback.m_ulSteamIDLobby);
        Debug.Log("Joined lobby: " + CurrentLobbyId);

        // 호스트인지 확인
        if (SteamMatchmaking.GetLobbyOwner(CurrentLobbyId) == SteamUser.GetSteamID())
        {
            Debug.Log("You are the host.");
            // 서버 시작 등 호스트 작업 수행
        }
        else
        {
            Debug.Log("You are a client.");
            // 클라이언트 작업 수행
        }
    }
    #endregion

    #region 로비 나가기
    public void LeaveLobby()
    {
        if (CurrentLobbyId.IsValid())
        {
            SteamMatchmaking.LeaveLobby(CurrentLobbyId);
            Debug.Log("Left lobby: " + CurrentLobbyId);
            CurrentLobbyId = CSteamID.Nil;
        }
        else
        {
            Debug.LogWarning("No valid lobby to leave.");
        }
    }
    #endregion

    #region 콜백 처리
    private void OnLobbyChatUpdate(LobbyChatUpdate_t callback)
    {
        // 로비 멤버 변경 처리 (참가, 퇴장 등)
        // 필요에 따라 구현합니다.
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Game lobby join requested. Lobby ID: " + callback.m_steamIDLobby);
        JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyDataUpdate(LobbyDataUpdate_t callback)
    {
        // 로비 데이터가 업데이트되었을 때 처리
        // 필요에 따라 구현합니다.
    }
    #endregion

    private void OnDestroy()
    {
        // 로비 나가기
        LeaveLobby();

        // SteamManager에서 SteamAPI.Shutdown()을 호출하므로 여기서는 필요하지 않습니다.
    }
}
