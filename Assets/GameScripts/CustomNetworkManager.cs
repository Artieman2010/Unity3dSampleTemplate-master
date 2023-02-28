using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class CustomNetworkManager : NetworkManager
{
    private List<string> bannedIPs = new List<string>() { "123.456.789.0", "987.654.321.0" };

   public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (bannedIPs.Contains(conn.address))
        {
            Debug.Log("Rejected connection from banned IP address: " + conn.address);
            conn.Disconnect();
        }
        else
        {
            base.OnServerConnect(conn);
        }
    }
    
}