using UnityEngine;
using UnityEngine.UI;
using FayvitEventAgregator;
using System;

public class HudPhoton : MonoBehaviour
{
    [SerializeField] private Text mainText;
    [SerializeField] private GameObject botoesPrincipais;
    
    // Start is called before the first frame update
    void Start()
    {
        EventAgregator.AddListener(EventKey.iniciandoConexao, OnStartConnect);
        EventAgregator.AddListener(EventKey.conexaoRealizada, OnConnect);
        EventAgregator.AddListener(EventKey.salaCriada, OnCreateRoom);
        EventAgregator.AddListener(EventKey.desligarHudPhoton, OnRequestOffHud);
        EventAgregator.AddListener(EventKey.conectandoParaJoin, OnStartConnectToJoin);
        EventAgregator.AddListener(EventKey.entrandoNoLobby, OnStartEnterLobby);
        EventAgregator.AddListener(EventKey.entrouNoLobby, OnEnterInLobby);
        EventAgregator.AddListener(EventKey.entrandoNaSala, OnEnterInTheRoom);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.iniciandoConexao, OnStartConnect);
        EventAgregator.RemoveListener(EventKey.conexaoRealizada, OnConnect);
        EventAgregator.RemoveListener(EventKey.salaCriada, OnCreateRoom);
        EventAgregator.RemoveListener(EventKey.desligarHudPhoton, OnRequestOffHud);
        EventAgregator.RemoveListener(EventKey.conectandoParaJoin, OnStartConnectToJoin);
        EventAgregator.RemoveListener(EventKey.entrandoNoLobby, OnStartEnterLobby);
        EventAgregator.RemoveListener(EventKey.entrouNoLobby, OnEnterInLobby);
        EventAgregator.RemoveListener(EventKey.entrandoNaSala, OnEnterInTheRoom);
    }

    private void OnEnterInTheRoom(IGameEvent obj)
    {
        string s = (string)obj.MySendObjects[0];
        mainText.text += "\n\r Entrando na sala: \n\r"+s;
    }

    private void OnEnterInLobby(IGameEvent obj)
    {
        mainText.text += "\n\r Entrou no Lobby, buscando salas";
    }

    private void OnStartEnterLobby(IGameEvent obj)
    {
        mainText.text += "\n\r Iniciando entrada no Lobby";
    }

    private void OnStartConnectToJoin(IGameEvent obj)
    {
        mainText.text = "Iniciando Conexão...";
        botoesPrincipais.SetActive(false);
    }

    private void OnRequestOffHud(IGameEvent obj)
    {
        gameObject.SetActive(false);
    }

    private void OnCreateRoom(IGameEvent obj)
    {
        mainText.text += "\n\r Sala criada";
        
    }

    private void OnConnect(IGameEvent obj)
    {
        string s = (string)obj.MySendObjects[0];
        mainText.text += "\n\r Conexão realizada\n\r criando a sala: "+s;
    }

    private void OnStartConnect(IGameEvent obj)
    {
        mainText.text = "Iniciando Conexão...";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
