using UnityEngine;
using Steamworks;
using FishNet;
using FishNet.Managing;

public class HostManager : MonoBehaviour
{
    /*
    public static HostManager Instance { get; private set; }

    private FishNet.Managing.NetworkManager networkManager;
    private Lobby currentLobby;

    private void Awake()
    {
        // Singleton 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            networkManager = InstanceFinder.NetworkManager;

            if (networkManager == null)
            {
                Debug.LogError("NetworkManager not found in the scene.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region 서버 시작 및 관리

    public void InitializeHost(Lobby lobby)
    {
        currentLobby = lobby;

        // 서버 시작
        StartServer();

        // 서버의 Steam ID를 로비 데이터에 저장
        currentLobby.SetData("server_steamid", SteamClient.SteamId.ToString());

        // 게임 시작 신호 전송
        SendStartGameSignal();
    }

    private void StartServer()
    {
        if (networkManager != null)
        {
            networkManager.ServerManager.StartConnection();
            Debug.Log("Server started.");
        }
        else
        {
            Debug.LogError("NetworkManager is null.");
        }
    }

    private void SendStartGameSignal()
    {
        currentLobby.SetData("start_game", "true");
        Debug.Log("Start game signal sent.");
    }

    #endregion

    #region 플레이어 상태 관리

    public bool AreAllPlayersReady()
    {
        foreach (var member in currentLobby.Members)
        {
            string isReady = currentLobby.GetMemberData(new Friend(member.Id), "is_ready");
            if (isReady != "true")
            {
                return false;
            }
        }
        return true;
    }

    #endregion
    */
}
