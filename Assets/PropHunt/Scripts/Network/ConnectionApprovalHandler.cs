using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static Unity.Netcode.NetworkManager;

/// </summary>
/// Connection Approval Handler Component
/// </summary>
/// <remarks>
/// This should be placed on the same GameObject as the NetworkManager.
/// It automatically declines the client connection for example purposes.
/// </remarks>
public class ConnectionApprovalHandler : MonoBehaviour
{
    private NetworkManager m_NetworkManager;
    public GameOnlineManager m_GameOnlineManager;

    public int MaxNumberOfPlayers = 10;
    private int _numberOfPlayers = 0;

    private void Start()
    {
        m_NetworkManager = GetComponent<NetworkManager>();
        if (m_NetworkManager != null)
        {
            m_NetworkManager.OnClientDisconnectCallback += OnClientDisconnectCallback;
            m_NetworkManager.ConnectionApprovalCallback += CheckApprovalCallback;
        }
        if (MaxNumberOfPlayers == 0)
        {
            MaxNumberOfPlayers++;
        }
    }

    private void CheckApprovalCallback(ConnectionApprovalRequest request, ConnectionApprovalResponse response)
    {
        bool isApproved = true;
        _numberOfPlayers++;
        //Debug.Log(m_GameOnlineManager);
        if (m_GameOnlineManager.IsGameStart == true)
        {
            isApproved = false;
            response.Reason = "Game Already Start";
        }
        if (_numberOfPlayers > MaxNumberOfPlayers)
        {
            isApproved = false;
            response.Reason = "Too many players in lobby!";
        }

        response.Approved = isApproved;
        response.CreatePlayerObject = isApproved;
        response.Position = new Vector3(0, 3, 0);
    }

    private void OnClientDisconnectCallback(ulong clientID)
    {
        if (!m_NetworkManager.IsServer && m_NetworkManager.DisconnectReason != string.Empty && !m_NetworkManager.IsApproved)
        {
            Debug.Log($"Approval Declined Reason: {m_NetworkManager.DisconnectReason}");
        }
        _numberOfPlayers--;
    }
}

