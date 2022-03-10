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
    private bool _initialized;

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
        if (_initialized == false) {
            _manager = manager;
            playerCollider = _manager.player.GetComponent<Collider2D>();
            enemies = transform.Find("Enemies").GetComponentsInChildren<Enemy>();
            _endZone.Setup(this);
            _initialized = true;
        }
        ResetRoom();
    }

    public void ResetRoom() {
        foreach (Enemy enemy in enemies) {
            enemy.Reset();
        }
        AudioManager.Instance.PlayAudio(AudioManager.GameSfx.spawn);
        _manager.player.GetComponent<WallJumper>().InstantMove(_spawnPoint.position);
        FindObjectOfType<PlayerBody>().Reset();
        playerInExitZone = false;
    }
    
    public void SetCameraOff() {
        _vcam.Priority = 0;
    }

    public void SetCameraOn() {
        _vcam.Priority = 10;
    }

    
}
