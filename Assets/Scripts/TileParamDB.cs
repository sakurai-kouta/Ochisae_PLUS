using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileParamDB", menuName = "Scriptable Objects/TileParamDB")]
public class TileParamDB : ScriptableObject
{
    [SerializeField] private TileParamData[] tilePramData;

    public TileParamData searchData(TileBase _tile) 
    {
        foreach (var item in tilePramData) 
        {
            if ( item == null ) continue;
            if (item.getTile() == _tile)
            {
                return item;
            }
        }
        Debug.Log("ŠY“–ƒ^ƒCƒ‹‚È‚µ");
        return null;
    }
}

[System.Serializable]
public class TileParamData 
{
    [SerializeField] private TileBase tile;
    [SerializeField] private float friction;
    [SerializeField] private float liftSpeed = 0f;
    [SerializeField] private float[] bounce = { 0, 1, 1, 1};
    public TileBase getTile()
    {
        return tile;
    }
    public float getFriction()
    {
        return friction;
    }
    public float[] getBounce()
    {
        return bounce;
    }
    public float getLiftSpeed() 
    {
        return liftSpeed;
    }
    public void dprint() 
    {
        Debug.Log("friction = " + friction);
        Debug.Log("bounce = " + bounce);
    }

}
