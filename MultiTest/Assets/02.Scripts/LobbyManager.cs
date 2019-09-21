using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private Button[] buttons;
    private Text[] texts;
    private string gameVersion = "1"; //게임 버전
    private Canvas[] canvases;
//    List<Photon.Realtime.Player> playerList = new List<Photon.Realtime.Player>();
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        buttons = GetComponentsInChildren<Button>();
        texts = GetComponentsInChildren<Text>();
        canvases = GetComponentsInChildren<Canvas>();
        buttons[0].onClick.AddListener(Connect);
        canvases[1].enabled = false;
    }

    //마스터 서버 접속 성공시 자동 실행
    public override void OnConnectedToMaster()
    {
        buttons[0].interactable = true;
        TextChange(texts, 3, "온라인 : 마스터 서버와 연결됨.");
    }

    private void TextChange(Text[] texts,int index,string ment)
    {
        texts[index].text = ment;
    }

    //마스터 서버 접속 실패 시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        buttons[0].interactable = false;
        TextChange(texts, 3, "오프라인 : 마스터 서버와 연결되지 않음 ");
        PhotonNetwork.ConnectUsingSettings();
    }

    
//        PhotonNetwork.LoadLevel("GameScene");
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
//        playerList.Add(newPlayer);
//        for (int i = 0; i < playerList.Count; i++)
//        {
//            JoinCanvasTextList[i].text = playerList[i].NickName;
//            if (JoinCanvasTextList[i].text != "대기중...")
//            {
//                JoinCanvasImage[i].color = Color.green;
//            }
//        }

//        JoinCanvasTextList[5].text = $"접속자 {playerList.Count}/4";
        TextChange(texts, 4, $"접속자 {PhotonNetwork.PlayerList.Length}/2");
//        if (PhotonNetwork.PlayerList.Length == 2)
//        {
//            PhotonNetwork.LoadLevel("02");
//        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //        playerList.Remove(otherPlayer);
        //        for (int i = 0; i < 4; i++)
        //        {
        //            JoinCanvasTextList[i].text = "대기중...";
        //        }
        //        for (int i = 0; i < playerList.Count; i++)
        //        {
        //            JoinCanvasTextList[i].text = playerList[i].NickName;
        //        }
        //        for (int i = 0; i < 4; i++)
        //        {
        //            if (JoinCanvasTextList[i].text == "대기중...")
        //            {
        //                JoinCanvasImage[i].color = Color.red;
        //            }
        //        }
        //        JoinCanvasTextList[5].text = $"접속자 {playerList.Count}/4";
        TextChange(texts, 4, $"접속자 {PhotonNetwork.PlayerList.Length}/2");
    }

    //룸 접속 시도
    public void Connect()
    {
//        PhotonNetwork.NickName = LobbyCanvasTextList[5].text;
        buttons[0].interactable = false;
//        LobbyCanvas.SetActive(false);
//        JoinCanvas.SetActive(true);
        if (PhotonNetwork.IsConnected)
        {
            TextChange(texts, 3, "룸에 접속...");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            TextChange(texts, 3, "오프라인 : 마스터 서버와 연결되지 않음");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //빈 방이 없어 랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //접속 상태 표시
//        TextChange(texts, 3, "빈 방이 없음, 새로운 방 생성...");
        //최대 3명을 수용 가능한 빈 방 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 3 });
    }
    //룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        canvases[0].enabled = false;
        canvases[1].enabled = true;
        TextChange(texts,4, $"접속자 {PhotonNetwork.PlayerList.Length}/2");
//        JoinCanvasTextList[].text = $"접속자 {playerList.Count}/4";
        TextChange(texts, 3, "방 참가 성공");
//        if (PhotonNetwork.PlayerList.Length == 2)
//        {
            PhotonNetwork.LoadLevel("02");
//        }
    }
}
