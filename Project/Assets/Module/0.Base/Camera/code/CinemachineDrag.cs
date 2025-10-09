using UnityEngine.EventSystems;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

[RequireComponent(typeof(CinemachineCamera))]
public class CinemachineDrag : MonoBehaviour
{
    [SerializeField] private float confine = 0.1f;
    [SerializeField] private float dragLerp = 5;

    [SerializeField, ReadOnly] private bool isCameraMoving = false;
    private Vector2 touchScrPos;
    private Vector2 touchStartMouseWorldPos;
    private Vector3 touchStartWorldPos;
    private Vector3 targetPos;
    private Vector3 originPos;
    private PlayerInputAction.TouchActions touchAction;

    void Awake()
    {
        touchAction = new PlayerInputAction().Touch;
        touchAction.TouchPress.performed += OnFingerDown;
        touchAction.TouchPress.canceled += OnFingerUp;
        touchAction.TouchPosition.performed += OnFingerMove;
    }
    void OnEnable()
    {
        //每次开启时，以当前位置重置拖拽中心
        touchAction.Enable();
        originPos = transform.position;
        targetPos = transform.position;
    }
    void OnDisable()
    {
        touchAction.Disable();
        isCameraMoving = false;
    }
    void OnDestroy()
    {
        touchAction.TouchPress.performed -= OnFingerDown;
        touchAction.TouchPress.canceled -= OnFingerUp;
        touchAction.TouchPosition.performed -= OnFingerMove;
    }

    void OnFingerDown(InputAction.CallbackContext context)
    {
        if (IsPointerOverUI(touchScrPos))
        {
            return;
        }
        isCameraMoving = true;
        touchStartMouseWorldPos = CameraManager.Instance.ScreenToWorldPos(touchScrPos);
        touchStartWorldPos = transform.position;
    }

    void OnFingerUp(InputAction.CallbackContext context)
    {
        isCameraMoving = false;
    }

    void OnFingerMove(InputAction.CallbackContext context)
    {
        touchScrPos = context.ReadValue<Vector2>();
        if(!isCameraMoving)
            return;

        Vector2 currentMouseWorldPos = CameraManager.Instance.ScreenToWorldPos(touchScrPos);
        //移动向量
        Vector2 delta = touchStartMouseWorldPos - currentMouseWorldPos;
        if (delta.magnitude > 0.1f)
        {
            targetPos = touchStartWorldPos + (Vector3)delta;
            targetPos.x = Mathf.Clamp(targetPos.x, originPos.x-confine*0.5f, originPos.x+confine*0.5f);
            targetPos.y = Mathf.Clamp(targetPos.y, originPos.y-confine*0.5f, originPos.y+confine*0.5f);
        }
    }
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime*dragLerp);
    }
    public void ResetPos()
    {
        transform.position = originPos;
        targetPos = originPos;
    }
    bool IsPointerOverUI(Vector2 screenPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = screenPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }
}
