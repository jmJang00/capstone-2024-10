using UnityEngine;
using Steamworks;
using FishNet;
using FishNet.Managing;

public class ClientManager : MonoBehaviour
{
    /*
    public static ClientManager Instance { get; private set; }

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

    #region 클라이언트 초기화 및 연결

    public void InitializeClient(Lobby lobby)
    {
        currentLobby = lobby;

        // 로비 데이터 변경 감지
        currentLobby.OnLobbyDataChanged += OnLobbyDataChanged;

        // 초기 상태 확인
        CheckStartGameSignal();
    }

    private void OnDestroy()
    {
        if (currentLobby != null)
        {
            currentLobby.OnLobbyDataChanged -= OnLobbyDataChanged;
        }
    }

    private void OnLobbyDataChanged()
    {
        CheckStartGameSignal();
    }

    private void CheckStartGameSignal()
    {
        string startGame = currentLobby.GetData("start_game");
        if (startGame == "true")
        {
            ConnectToServer();
        }
    }

    private void ConnectToServer()
    {
        string serverSteamIdString = currentLobby.GetData("server_steamid");
        if (ulong.TryParse(serverSteamIdString, out ulong serverSteamId))
        {
            SteamId serverSteamIdObj = new SteamId(serverSteamId);

            // 네트워크 매니저 설정
            if (networkManager != null)
            {
                networkManager.ClientManager.StartConnection();
                Debug.Log("Client started and connecting to server.");
            }
            else
            {
                Debug.LogError("NetworkManager is null.");
            }
        }
        else
        {
            Debug.LogError("Failed to get server Steam ID.");
        }
    }

    #endregion

    #region 플레이어 상태 업데이트

    public void SetReadyState(bool isReady)
    {
        currentLobby.SetMemberData("is_ready", isReady ? "true" : "false");
    }

    #endregion
    */
}
