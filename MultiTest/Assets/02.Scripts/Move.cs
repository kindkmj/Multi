using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviourPun, User
{
    public User Status;
    [SerializeField] private float roteSpeed;
    [SerializeField] private float moveSpeed;
    public CinemachineVirtualCamera cinemachine;
    public Material[] _Materials;
    private GameObject gameObject;
    private PhotonView _photonView;
    private Transform[] bulleTransform;
    private Renderer[] renderers;
    private Move[] Player;
    private Slider _slider;
    private Vector3 curpos;
    public int HP { get; set; } = 100;
    public int Attack { get; }
    private Transform playerTransform;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponents<Move>();
           renderers = GetComponentsInChildren<Renderer>();
        _photonView = GetComponent<PhotonView>();
        if (_photonView.IsMine)
        {
            playerTransform = GameObject.FindWithTag("bulletPosition").GetComponent<Transform>();
            bulleTransform = GetComponentsInChildren<Transform>();
            gameObject = GetComponent<GameObject>();
            cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
            cinemachine.Follow = transform;
            cinemachine.LookAt = transform;
            renderers[1].material = _Materials[0];
            _slider = GetComponent<Slider>();
        }
        else
        {
            renderers[1].material = _Materials[1];
        }

        roteSpeed = 100f;
        moveSpeed = 10f;
    }
    void Update()
    {
        if (_photonView.IsMine)
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");
            hor = hor * roteSpeed * Time.deltaTime;
            ver = ver * moveSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up * hor);
            transform.Translate(Vector3.forward * ver);
            Fire();
            FirePosition();
        }
        //부드럽게 위치를 동기화
        //        else if ((transform.position - curpos).sqrMagnitude >= 100) transform.position = curpos;
        //        else transform.position = Vector3.Lerp(transform.position, curpos, Time.deltaTime * 10);
    }

    private void FirePosition()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerTransform.transform.Rotate(playerTransform.transform.rotation.x-2, playerTransform.transform.rotation.y, playerTransform.transform.rotation.z);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            playerTransform.transform.Rotate(playerTransform.transform.rotation.x + 2, playerTransform.transform.rotation.y, playerTransform.transform.rotation.z);
        }
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PhotonNetwork.Instantiate("Bullet", bulleTransform[2].transform.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (_photonView.IsMine==false&&collider.tag == "Bullet"&&collider.GetComponent<PhotonView>().IsMine)
        {
            collider.GetComponent<Move>().Hit();
        }
    }

    public void Hit()
    {
        _slider.value -= 10;
        if (_slider.value <= 0)
        {
            _photonView.RPC("Die",RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    public void Die() => Destroy(gameObject);

//    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        if (stream.IsWriting)
//        {
//            stream.SendNext(transform.position);
//            stream.SendNext(_slider.value);
//        }
//        else
//        {
//            curpos = (Vector3) stream.ReceiveNext();
//            _slider.value = (float) stream.ReceiveNext();
//        }
//    }
}
