using UnityEngine;

public class BaseSlotEntity : MonoBehaviour {

    public int typeID;
    public int id;
    public BaseSlotType baseSlotType;
    public Vector3Int pos;

    public void Ctor() {

    }

    public void SetPosInt(Vector3Int pos) {
        transform.position = pos;
    }

}
