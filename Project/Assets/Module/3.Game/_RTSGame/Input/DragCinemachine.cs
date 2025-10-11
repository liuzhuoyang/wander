using UnityEngine;
using Unity.Cinemachine;

using PlayerInteraction;

[RequireComponent(typeof(CinemachineCamera))]
public class CinemachineDrag : MonoBehaviour
{
    [SerializeField] private float confine = 0.1f;
    [SerializeField] private float dragLerp = 5;

    private bool isCameraMoving = false;
    private Vector2 touchStartMouseWorldPos;
    private Vector3 touchStartWorldPos;
    private Vector3 targetPos;
    private Vector3 originPos;
    private PlayerInputControl playerInputControl;

    void OnEnable()
    {
        //每次开启时，以当前位置重置拖拽中心
        originPos = transform.position;
        targetPos = transform.position;

        playerInputControl = PlayerInputManager.Instance.m_currentPlayerInput;
        playerInputControl.onClickEmpty += OnFingerDown;
        playerInputControl.onReleaseEmpty += OnFingerUp;
        playerInputControl.onMoveEmpty += OnFingerMove;
    }
    void OnDisable()
    {
        playerInputControl.onClickEmpty -= OnFingerDown;
        playerInputControl.onReleaseEmpty -= OnFingerUp;
        playerInputControl.onMoveEmpty -= OnFingerMove;
        isCameraMoving = false;
    }

    void OnFingerDown(bool isOverUI)
    {
        if(!isOverUI)
        {
            isCameraMoving = true;
            touchStartMouseWorldPos = playerInputControl.m_pointerWorldPos;
            touchStartWorldPos = transform.position;
        }
    }

    void OnFingerUp()
    {
        isCameraMoving = false;
    }

    void OnFingerMove(Vector2 currentMouseWorldPos)
    {
        if(!isCameraMoving)
            return;

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
}
