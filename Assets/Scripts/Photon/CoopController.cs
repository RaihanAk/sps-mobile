using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using System.IO;

public class CoopController : MonoBehaviourPunCallbacks
{
    Vector3 pos = new Vector3(-28.5f, 4f, -7.25f);


    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate("CubePlayerTest", pos, Quaternion.identity);
    }
}
