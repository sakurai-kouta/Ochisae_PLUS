using UnityEngine;

/// <summary>
/// チェックポイント長押し中のカメラ演出（プレイヤー中心固定版）
/// </summary>
public class CameraHoldEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Camera targetCamera;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomOrthoSize = 3.5f;

    [Header("Shake Settings")]
    [SerializeField] private float shakePower = 0.1f;

    private bool isHolding;

    // ★ ホールド開始時の保存情報
    private Vector3 baseCameraPos;
    private float baseOrthoSize;

    // ★ ズーム中の基準位置（必ずプレイヤー中心）
    private Vector3 holdCenterPos;

    /// <summary>
    /// 長押し開始
    /// </summary>
    public void BeginHold()
    {
        if (isHolding) return;
        isHolding = true;

        // 元の状態保存
        baseCameraPos = transform.position;
        baseOrthoSize = targetCamera.orthographicSize;

        // 即ズーム
        targetCamera.orthographicSize = zoomOrthoSize;

        // ★ 画面横幅計算（ズーム後）
        float halfWidth =
            targetCamera.orthographicSize * targetCamera.aspect;

        // ★ プレイヤーが「左から2/3」に来るカメラ位置
        float cameraX =
            player.position.x - (halfWidth / 3f);

        holdCenterPos = new Vector3(
            cameraX,
            player.position.y,
            transform.position.z
        );

        transform.position = holdCenterPos;
    }


    /// <summary>
    /// 長押し終了
    /// </summary>
    public void EndHold()
    {
        if (!isHolding) return;
        isHolding = false;

        // 即元に戻す
        transform.position = baseCameraPos;
        targetCamera.orthographicSize = baseOrthoSize;
    }

    private void LateUpdate()
    {
        if (!isHolding) return;

        // 振動のみ（プレイヤー中心基準）
        Vector2 shake = Random.insideUnitCircle * shakePower;

        transform.position =
            holdCenterPos +
            new Vector3(shake.x, shake.y, 0f);
    }
}
