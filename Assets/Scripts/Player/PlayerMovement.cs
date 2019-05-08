using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {

    public int speed;
    public NetworkIdentity networkIdentity;
    public GameObject graphics;
    public Camera camera;

    public Vector3 newposition;
    [SyncVar]
    Vector3 syncpos;


    private PlayerEvents playerEvents;

	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
        {
            camera.enabled = false;
            return;
        }
        newposition = transform.position;
        networkIdentity = GetComponent<NetworkIdentity>();
        playerEvents = GetComponent<PlayerEvents>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        CheckInput();
        
        
    }
    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            CmdSetDestination(transform.position);
        }
        Move();
    }

    private void Move()
    {
        if (transform.position == newposition)
        {
            return;  
        }
        if (!isLocalPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, syncpos, speed * Time.deltaTime);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, newposition, speed * Time.deltaTime);
    }

    void CheckInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = this.camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "navmesh")
            {
                newposition = hit.point;
            }
        }
    }


    [Command]
    void CmdSetDestination(Vector3 pos)
    {
        syncpos = pos;
    }
}
