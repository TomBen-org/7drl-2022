using Cinemachine;
using UnityEngine;

public class Room : MonoBehaviour {
    private CinemachineVirtualCamera _vcam;
    private EndZone _endZone;
    public Collider2D playerCollider;
    private Transform _spawnPoint;
    private RoomManager _manager;

    public bool playerInExitZone;
    public Enemy[] enemies;

    public Ability[] abilities;

    void Awake() {
        _spawnPoint = transform.Find("SpawnPoint");
        _vcam = GetComponentInChildren<CinemachineVirtualCamera>();
        _endZone = GetComponentInChildren<EndZone>();
        
        Grid grid = FindObjectOfType<Grid>();
        Vector3Int cell = grid.WorldToCell(_spawnPoint.transform.position);
        Vector2 newPos = grid.GetCellCenterWorld(cell);
        _spawnPoint.transform.position = newPos;
    }

    public void Init(RoomManager manager) {
        playerInExitZone = false;
        _manager = manager;
        _manager.player.GetComponent<WallJumper>().InstantMove(_spawnPoint.position);
        AudioManager.Instance.PlayAudio(AudioManager.GameSfx.spawn);
        playerCollider = _manager.player.GetComponent<Collider2D>();
        enemies = transform.Find("Enemies").GetComponentsInChildren<Enemy>();
        _endZone.Setup(this);
    }
    
    public void SetCameraOff() {
        _vcam.Priority = 0;
    }

    public void SetCameraOn() {
        _vcam.Priority = 10;
    }

    
}
