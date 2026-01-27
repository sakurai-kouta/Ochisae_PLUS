using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// マウスカーソルが UI 要素に重なっているかを判定するユーティリティ
/// static なのでどこからでも利用可能
/// </summary>
public static class UIMouseOverUtility
{
    /// <summary>
    /// マウスカーソルが Canvas 上の UI 要素に重なっているか
    /// </summary>
    public static bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

    /// <summary>
    /// 特定の Tag を持つ UI 要素に重なっているか
    /// </summary>
    public static bool IsPointerOverUIWithTag(string tag)
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag(tag))
                return true;
        }

        return false;
    }

    /// <summary>
    /// マウスカーソルが特定の UI 要素（自身 or 子要素）に重なっているか
    /// 使用例:
    /// bool flg = UIMouseOverUtility.IsPointerOverUI(this);
    /// </summary>
    public static bool IsPointerOverUI(Component target)
    {
        if (target == null)
            return false;

        return IsPointerOverUI(target.gameObject);
    }

    /// <summary>
    /// マウスカーソルが特定の UI GameObject（自身 or 子要素）に重なっているか
    /// </summary>
    public static bool IsPointerOverUI(GameObject target)
    {
        if (target == null || EventSystem.current == null)
            return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            // 自身、または子要素にヒットしていれば OK
            if (result.gameObject == target ||
                result.gameObject.transform.IsChildOf(target.transform))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// マウスカーソル下に存在する UI オブジェクト一覧を取得（デバッグ用）
    /// </summary>
    public static List<GameObject> GetUIUnderPointer()
    {
        List<GameObject> list = new();

        if (EventSystem.current == null)
            return list;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            list.Add(result.gameObject);
        }

        return list;
    }
}
