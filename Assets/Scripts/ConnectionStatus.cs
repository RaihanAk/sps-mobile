using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class ConnectionStatus : MonoBehaviour
{
    private string connectionStatusMessage;

    public Text ConnectionStatusText;

    // Update is called once per frame
    void Update()
    {
        ConnectionStatusText.text = "Status : " + PhotonNetwork.NetworkClientState;
    }
}
