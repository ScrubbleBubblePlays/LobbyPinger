using BepInEx;
using Photon.Pun;
using Photon.Realtime;
using System.Net;
using UnityEngine;
using System.Text;

[BepInPlugin("com.yourname.gtaglobbypinger", "LobbyPinger", "1.0.0")]
public class LobbyPinger : BaseUnityPlugin
{
    private string webhookUrl = "https://discord.com/api/webhooks/YOUR_WEBHOOK_URL_HERE";
    private bool hasSent = false;

    void Update()
    {
        if (PhotonNetwork.InRoom && !hasSent)
        {
            string code = PhotonNetwork.CurrentRoom.Name;
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;

            SendWebhook(code, playerCount, maxPlayers);
            hasSent = true;
        }

        if (!PhotonNetwork.InRoom)
        {
            hasSent = false;
        }
    }

    void SendWebhook(string code, int players, int max)
    {
        string json = "{\"content\":\"🦍 I'm online in `" + code + "` (" + players + "/" + max + " players)\"}";

        using (WebClient client = new WebClient())
        {
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            client.UploadStringAsync(new System.Uri(webhookUrl), "POST", json);
        }

        Debug.Log("[LobbyPinger] Sent webhook with lobby info.");
    }
}
