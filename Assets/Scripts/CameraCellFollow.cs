using UnityEngine;

public class CameraCellFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Cell Settings")]
    [SerializeField] private Vector2 cellSize = new Vector2(10f, 6f);

    [Header("Move Settings")]
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField]
    private AnimationCurve easeCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector2Int currentCell;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float moveTime;
    private bool isMoving;

    private void Start()
    {
        currentCell = GetCell(player.position);
        targetPos = GetCellCenter(currentCell);

        transform.position = new Vector3(
            targetPos.x,
            targetPos.y,
            transform.position.z
        );
    }

    private void LateUpdate()
    {
        Vector2Int newCell = GetCell(player.position);

        if (newCell != currentCell)
        {
            currentCell = newCell;
            StartMove(GetCellCenter(currentCell));
        }

        if (isMoving)
        {
            UpdateMove();
        }
    }

    // ----------------------------
    // Cell calculation
    // ----------------------------

    private Vector2Int GetCell(Vector3 pos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(pos.x / cellSize.x),
            Mathf.FloorToInt(pos.y / cellSize.y)
        );
    }

    private Vector3 GetCellCenter(Vector2Int cell)
    {
        return new Vector3(
            cell.x * cellSize.x + cellSize.x * 0.5f,
            cell.y * cellSize.y + cellSize.y * 0.5f,
            0f
        );
    }

    // ----------------------------
    // Camera move (smooth)
    // ----------------------------

    private void StartMove(Vector3 newTarget)
    {
        startPos = transform.position;
        targetPos = new Vector3(
            newTarget.x,
            newTarget.y,
            transform.position.z
        );

        moveTime = 0f;
        isMoving = true;
    }

    private void UpdateMove()
    {
        if (moveDuration <= 0f)
        {
            transform.position = targetPos;
            isMoving = false;
            return;
        }

        moveTime += Time.deltaTime;
        float t = Mathf.Clamp01(moveTime / moveDuration);
        float eased = easeCurve.Evaluate(t);

        transform.position = Vector3.Lerp(startPos, targetPos, eased);

        if (t >= 1f)
        {
            isMoving = false;
        }
    }

    // ----------------------------
    // External API
    // ----------------------------

    /// <summary>
    /// プレイヤー位置に即座にカメラを合わせる
    /// （moveDuration = 0 相当）
    /// </summary>
    public void SnapToPlayer()
    {
        currentCell = GetCell(player.position);
        Vector3 snapPos = GetCellCenter(currentCell);

        isMoving = false;
        moveTime = 0f;

        transform.position = new Vector3(
            snapPos.x,
            snapPos.y,
            transform.position.z
        );
    }
}
