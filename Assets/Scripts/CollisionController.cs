using UnityEngine;

public static class ColisionController
{
    public static Vector2 CulcVelocityEnter(Vector2 _preVelocity, Vector2 _normal, TileParamData tileParamData)
    {
        Debug.Log("CulcVelocityEnter");
        float _friction = tileParamData.getFriction();
        float _liftSpeed = tileParamData.getLiftSpeed();
        float[] _bounce = tileParamData.getBounce();
        // ★★★ ななめ方向から入射するとすり抜ける気がする。暫定処置で速度制限を30→27にしてみたので、効いているか経過観察
        Vector2 refV = Vector2.Reflect(_preVelocity, _normal);
        // top side
        if (Mathf.Approximately(_normal.y, 1f))
        {
            if (!Mathf.Approximately(_friction, 0f))
            {
                return new Vector2(_liftSpeed, 0);
            }
            return refV - Vector2.Dot(refV, _normal) * _normal * (1f - _bounce[0]);
        }
        // bottom side
        else if (Mathf.Approximately(_normal.y, -1f))
        {
            return refV - Vector2.Dot(refV, _normal) * _normal * (1f - _bounce[1]);
        }
        // left side
        else if (Mathf.Approximately(_normal.x, -1f))
        {
            return refV - Vector2.Dot(refV, _normal) * _normal * (1f - _bounce[2]);
        }
        // right side
        else if (Mathf.Approximately(_normal.x, 1f))
        {
            return refV - Vector2.Dot(refV, _normal) * _normal * (1f - _bounce[3]);
        }
        else
        {
            // 斜めブロックを実装した場合はここに来る(現在未実装)。
            return _preVelocity;
        }

    }
    public static Vector2 CulcVelocityStay(Vector2 _preVelocity, Vector2 _normal, TileParamData tileParamData)
    {
        if (Vector2.Dot(_preVelocity, _normal) > 0)
        {
            // もう壁から離れている
            return _preVelocity;
        }
        float _friction = tileParamData.getFriction();
        float _liftSpeed = tileParamData.getLiftSpeed();
        Vector2 refV = Vector2.Reflect(_preVelocity, _normal);
        Debug.Log($"CulcVelocityStay : _preVelocity = {_preVelocity}, _normal = {_normal}, tileParamData = {tileParamData}, refV = {refV}");
        // top side
        if (Mathf.Approximately(_normal.y, 1f))
        {
            if (!Mathf.Approximately(_friction, 0f))
            {
                return new Vector2(_liftSpeed, 0);
            }
        }
        else
        {
            // 横壁に接し続けた場合など
            return refV;
        }
        return _preVelocity;
    }
}
