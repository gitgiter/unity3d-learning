using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTilt : MonoBehaviour
{

    public Vector2 range = new Vector2(50f, 30f);

    Transform mTrans;
    Quaternion mStart;
    Vector2 mRot = Vector2.zero;

    // Use this for initialization
    void Start()
    {
        mTrans = transform;
        mStart = mTrans.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Input.mousePosition;
        //Debug.Log(pos);        

        float halfWidth = Screen.width * 0.5f;
        float halfHeight = Screen.height * 0.5f;

        if (pos.x - halfWidth > 0) return;

        float x = Mathf.Clamp((pos.x - halfWidth) / halfWidth, -1f, 1f);
        float y = Mathf.Clamp((pos.y - halfHeight) / halfHeight, -1f, 1f);
        mRot = Vector2.Lerp(mRot, new Vector2(x, y), Time.deltaTime * 5f);

        mTrans.localRotation = mStart * Quaternion.Euler(-mRot.y * range.y, -mRot.x * range.x, 0f);
    }
}
