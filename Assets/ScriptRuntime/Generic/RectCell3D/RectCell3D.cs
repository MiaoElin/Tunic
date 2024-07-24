using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public struct RectCell3D : IEquatable<RectCell3D>, IComparable<RectCell3D> {
    public Vector2Int pos;
    public Vector3 worldPos;
    public float fCost;
    public float gCost;
    public float hCost;

    // public RectCell3D parent;

    public bool isBlock;
    public bool impassable;
    public bool isClose;

    public void Ctor(int x, int y, float sideLength) {
        this.pos = new Vector2Int(x, y);
        this.worldPos.x = x * sideLength + sideLength / 2;
        this.worldPos.z = y * sideLength + sideLength / 2;
    }

    public void Init(float f, float g, float h) {
        this.fCost = f;
        this.gCost = g;
        this.hCost = h;
    }

    public Vector2Int[] GetArround() {
        Vector2Int[] arround = new Vector2Int[8];
        arround[0] = new Vector2Int(pos.x - 1, pos.y + 1); // 左上
        arround[1] = new Vector2Int(pos.x, pos.y + 1);     // 上
        arround[2] = new Vector2Int(pos.x + 1, pos.y + 1); // 右上
        arround[3] = new Vector2Int(pos.x + 1, pos.y);     // 右中
        arround[4] = new Vector2Int(pos.x + 1, pos.y - 1); // 右下
        arround[5] = new Vector2Int(pos.x, pos.y - 1);     // 中下
        arround[6] = new Vector2Int(pos.x - 1, pos.y - 1); // 左下
        arround[7] = new Vector2Int(pos.x - 1, pos.y);     // 左中
        return arround;
    }

    public RectTemp[] GetArroundRect() {
        RectTemp[] arround = new RectTemp[8];
        RectTemp a0 = new RectTemp();
        a0.pos = new Vector2Int(pos.x - 1, pos.y + 1); // 左上
        arround[0] = a0;
        RectTemp a1 = new RectTemp();
        a1.pos = new Vector2Int(pos.x, pos.y + 1);
        arround[1] = a1;
        RectTemp a2 = new RectTemp();
        a2.pos = new Vector2Int(pos.x + 1, pos.y + 1); // 右上
        arround[2] = a2;
        RectTemp a3 = new RectTemp();
        a3.pos = new Vector2Int(pos.x + 1, pos.y);     // 右中
        arround[3] = a3;
        RectTemp a4 = new RectTemp();
        a4.pos = new Vector2Int(pos.x + 1, pos.y - 1); // 右下
        arround[4] = a4;
        RectTemp a5 = new RectTemp();
        a5.pos = new Vector2Int(pos.x, pos.y - 1);     // 中下
        arround[5] = a5;
        RectTemp a6 = new RectTemp();
        a6.pos = new Vector2Int(pos.x - 1, pos.y - 1); // 左下
        arround[6] = a6;
        RectTemp a7 = new RectTemp();
        a7.pos = new Vector2Int(pos.x - 1, pos.y);     // 左中
        arround[7] = a7;
        return arround;
    }


    public Vector3[] GetArroundWorldPos(float sideLength) {
        Vector3[] arround = new Vector3[8];
        arround[0] = new Vector3(worldPos.x - sideLength, worldPos.y, worldPos.z + sideLength); // 左上
        arround[1] = new Vector3(worldPos.x, worldPos.y, worldPos.z + sideLength);     // 上
        arround[2] = new Vector3(worldPos.x + sideLength, worldPos.y, worldPos.z + sideLength); // 右上
        arround[3] = new Vector3(worldPos.x + sideLength, worldPos.y, worldPos.z);     // 右中
        arround[4] = new Vector3(worldPos.x + sideLength, worldPos.y, worldPos.z - sideLength); // 右下
        arround[5] = new Vector3(worldPos.x, worldPos.y, worldPos.z - sideLength);     // 中下
        arround[6] = new Vector3(worldPos.x - sideLength, worldPos.y, worldPos.z - sideLength); // 左下
        arround[7] = new Vector3(worldPos.x - sideLength, worldPos.y, worldPos.z);     // 左中
        return arround;
    }


    bool IEquatable<RectCell3D>.Equals(RectCell3D other) {
        return pos == other.pos;
    }

    public override int GetHashCode() {
        return pos.GetHashCode();
    }



    // public bool Equals(RectCell2D other) {
    //     if (other == null) {
    //         return false;
    //     }

    //     if (this.pos == other.pos) {
    //         return true;

    //     } else {
    //         return false;
    //     }
    // }

    // int IComparable<RectCell2D>.CompareTo(RectCell2D other) {
    //     // 暂时这么写
    //     if (fCost < other.fCost) {
    //         return -1;
    //     } else if (fCost > other.fCost) {
    //         return 1;
    //     } else {
    //         if (hCost > other.hCost) {
    //             return 1;
    //         } else if (hCost < other.hCost) {
    //             return -1;
    //         } else {
    //             if (pos.x > other.pos.x) {
    //                 return 1;
    //             } else {
    //                 return -1;
    //             }
    //         }
    //     }
    // }


    int IComparable<RectCell3D>.CompareTo(RectCell3D other) {

        Bit128 fKey = new Bit128();
        fKey.i32_0 = pos.y;
        fKey.i32_1 = pos.x;
        fKey.f32_2 = hCost;
        fKey.f32_3 = fCost;

        Bit128 otherFKey = new Bit128();
        otherFKey.i32_0 = other.pos.y;
        otherFKey.i32_1 = other.pos.x;
        otherFKey.f32_2 = other.hCost;
        otherFKey.f32_3 = other.fCost;

        if (fKey < otherFKey) {
            return -1;
        } else if (fKey > otherFKey) {
            return 1;
        } else {
            return 0;
        }

        // if (fCost < other.fCost) {
        //     return -1;
        // } else if (fCost > other.fCost) {
        //     return 1;
        // } else {
        //     if (hCost < other.hCost) {
        //         return -1;
        //     } else if (hCost > other.hCost) {
        //         return 1;
        //     } else {
        //         if (pos.x > other.pos.x) {
        //             return -1;
        //         } else if (pos.x < other.pos.x) {
        //             return 1;
        //         } else {
        //             if (pos.y > other.pos.y) {
        //                 return -1;
        //             } else if (pos.y < other.pos.y) {
        //                 return 1;
        //             } else {
        //                 return 0;
        //             }
        //         }
        //     }
        // }
    }
}