using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class Move : MonoBehaviourPun
{
   public User Status ;
    [SerializeField]
    private float roteSpeed;
    [SerializeField]
    private float moveSpeed;

    private BoxCollider[] boxColliders;

    public CinemachineVirtualCamera cinemachine;
    public Material[] _Materials;
    private PhotonView _photonView;
    private Renderer[] renderers;
    private Move[] Player;
    void Start()
    {
        boxColliders = GetComponentsInChildren<BoxCollider>();

        Player = GameObject.FindWithTag("Player").GetComponents<Move>();
           renderers = GetComponentsInChildren<Renderer>();

        _photonView = GetComponent<PhotonView>();
        if (_photonView.IsMine)
        {
            cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
            cinemachine.Follow = transform;
            cinemachine.LookAt = transform;
            renderers[1].material = _Materials[0];

//            this.GetComponent<Renderer>().material = _Materials[0];
        }
        else
        {
            renderers[1].material = _Materials[1];


            //            this.GetComponent<Renderer>().material = _Materials[1];
        }

        Changebody1();
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
        }

        for (int i = 0; i < boxColliders.Length; i++)
        {
            Debug.Log(boxColliders[i].name);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Player[1].Status.HP -= Status.Attack;
            Debug.Log(Player[1].Status.HP -= Status.Attack);
            Debug.Log(Player[1].Status.HP);
        }
    }

    [PunRPC]
    public void changebody()
    {
        if (_photonView.IsMine)
        {
            boxColliders[1].gameObject.SetActive(false);
        }
        else
        {
            boxColliders[2].gameObject.SetActive(false);

        }
    }

    [PunRPC]
    public virtual void Changebody1()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("changebody", RpcTarget.Others, boxColliders);
            photonView.RPC("Changebody1", RpcTarget.Others, boxColliders);

        }
    }
}
