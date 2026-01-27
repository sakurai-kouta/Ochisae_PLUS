using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FloorData", menuName = "Scriptable Objects/Floor Data")]
public class FloorData : ScriptableObject
{
    [System.Serializable]
    public class Floor
    {
        public string floorName;
        public string floorName_EN;
        public float minHeight;  // このフロアの最低高度
    }

    [Header("フロアリスト（下から順に並べること）")]
    public List<Floor> floors = new List<Floor>();

    /// <summary>
    /// 指定した Y 座標がどのフロアに属するかを返す
    /// </summary>
    /// <param name="yPos">プレイヤーのY座標</param>
    /// <returns>フロア名、見つからなければ null</returns>
    public string GetFloorName(float yPos)
    {
        if (floors.Count == 0) return null;

        // 下から順に確認
        for (int i = floors.Count - 1; i >= 0; i--)
        {
            if (yPos >= floors[i].minHeight)
            {
                return floors[i].floorName;
            }
        }

        // 全てのフロアより下の場合
        return null;
    }
    public string GetFloorName_EN(float yPos)
    {
        if (floors.Count == 0) return null;

        // 下から順に確認
        for (int i = floors.Count - 1; i >= 0; i--)
        {
            if (yPos >= floors[i].minHeight)
            {
                return floors[i].floorName_EN;
            }
        }

        // 全てのフロアより下の場合
        return null;
    }
}
