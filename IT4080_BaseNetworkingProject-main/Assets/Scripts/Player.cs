using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour {

    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<int> playersInGame = new NetworkVariable<int>();
    public NetworkVariable<Color> PlayerColor = new NetworkVariable<Color>(Color.red);
    private GameManager _gameMgr;
    public float movespeed = 1.0f;
    public void Start()
    {
        ApplyPlayerColor();
        PlayerColor.OnValueChanged += OnPlayerColorChanged;

        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsClient)
            {
                playersInGame.Value++;
            }
        }; 
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsClient)
            {
                playersInGame.Value--;
            }
        };
        
        
        
    }
    public int PlayersInGame
    {
        get
        {
           return playersInGame.Value;
        }
    }
    private void Update()
    {
        if (IsOwner)
        {
            Vector3 move = calcmovement();
            if(move.magnitude > 0)
            {
                RequestPostionForMovementServerRpc(move);
            }
        }
        else
        {
            transform.position = Position.Value;
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _gameMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
            _gameMgr.RequestNewPayerColorServerRpc();
            Debug.Log("requested");
        }
    }
   
    Vector3 calcmovement()
    {
        Vector3 moveVect = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveVect *= movespeed;
            return moveVect;
    }
    [ServerRpc]
    void RequestPostionForMovementServerRpc(Vector3 movement)
    {
        Position.Value += movement;
        float planesize = 5f;
        Vector3 newPosition = Position.Value + movement;
        newPosition.x = Mathf.Clamp(newPosition.x, planesize * -1, planesize);
        newPosition.z = Mathf.Clamp(newPosition.z, planesize * -1, planesize);
        Position.Value = newPosition;
    }
    
    public void OnPlayerColorChanged(Color previous, Color current)
    {
        ApplyPlayerColor();

    }
    public void ApplyPlayerColor()
    {
        GetComponent<MeshRenderer>().material.color = PlayerColor.Value;
    }
   
}
