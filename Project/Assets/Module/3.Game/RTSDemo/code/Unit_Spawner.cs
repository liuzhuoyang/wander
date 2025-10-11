using UnityEngine;
using BattleActor.Unit;
using UnityEngine.InputSystem;
using PlayerInteraction;

public class Unit_Spawner : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private InputAction spawnInput;
    [SerializeField] private UnitData unitData;
    [SerializeField] private bool isEnemy;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }
    void OnEnable()
    {
        spawnInput.performed += SpawnHandler;
        spawnInput.Enable();
    }
    void OnDisable()
    {
        spawnInput.performed -= SpawnHandler;
        spawnInput.Disable();
    }
    void SpawnHandler(InputAction.CallbackContext callback)
    {
        Vector2 mouseScrPos = callback.ReadValue<Vector2>();
        if (!PlayerInputService.IsPointerOverUI(mouseScrPos))
        {
            target.position = (Vector2)mainCam.ScreenToWorldPoint(mouseScrPos);
            UnitManager.Instance.CreateUnit(unitData.m_actorKey, target.position, isEnemy, 1, true);
        }
    }
    public void DestroyAllUnit()
    {
        UnitManager.Instance.RemoveAllUnit();
    }
}
