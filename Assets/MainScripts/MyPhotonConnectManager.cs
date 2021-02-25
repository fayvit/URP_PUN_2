using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using FayvitEventAgregator;
using FayvitSupportSingleton;
using FayvitCam;
using System.Collections.Generic;


public class MyPhotonConnectManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject prefabGame;

    private ConnectionState connState = ConnectionState.conectandoComoMaster;

    private enum ConnectionState
    { 
         conectandoComoMaster,
         conectandoParaJoin
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BotaoCriarSala()
    {
        connState = ConnectionState.conectandoComoMaster;
        PhotonNetwork.ConnectUsingSettings();
        EventAgregator.Publish(new GameEvent(EventKey.iniciandoConexao));
    }
    public void BotaoJuntarSe()
    {
        connState = ConnectionState.conectandoParaJoin;
        PhotonNetwork.ConnectUsingSettings();
        EventAgregator.Publish(new GameEvent(EventKey.conectandoParaJoin));
    }

    public override void OnConnectedToMaster()
    {
        if (connState == ConnectionState.conectandoComoMaster)
        {
            string s = System.Guid.NewGuid().ToString();

            EventAgregator.Publish(new GameEvent(EventKey.conexaoRealizada, s));
            
            PhotonNetwork.CreateRoom(s);
        }
        else if (connState == ConnectionState.conectandoParaJoin)
        {
            PhotonNetwork.JoinLobby();
            EventAgregator.Publish(new GameEvent(EventKey.entrandoNoLobby));
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("recebi room list com: "+roomList.Count+" salas");
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo R = roomList[i];
            Debug.Log(R.PlayerCount + " : "+R.Name+" : "+R.MaxPlayers ) ;
        }

        if (roomList.Count > 0)
        {
            PhotonNetwork.JoinRoom(roomList[0].Name);
            EventAgregator.Publish(new GameEvent(EventKey.entrandoNaSala,roomList[0].Name));

        }
    }

    public override void OnJoinedRoom()
    {
        SupportSingleton.Instance.InvokeInRealTime(IniciarControle, 1);
    }

    public override void OnJoinedLobby()
    {
        EventAgregator.Publish(new GameEvent(EventKey.entrouNoLobby));
        Debug.Log("Em Lobby: "+connState);
    }

    public override void OnCreatedRoom()
    {
        EventAgregator.Publish(new GameEvent(EventKey.salaCriada));
        SupportSingleton.Instance.InvokeInRealTime(IniciarControle, 1);
        
    }

    void IniciarControle()
    {
        EventAgregator.Publish(new GameEvent(EventKey.desligarHudPhoton));
        Vector3 pos = new Vector3(
            Random.Range(-49, 49),1,
            Random.Range(-49, 49)
            );

        GameObject G = PhotonNetwork.Instantiate("cuboPersonagem", pos, Quaternion.identity);

        CameraAplicator.cam.NewFocusForBasicCam(G.transform, 20, 20);

        
    }



}
