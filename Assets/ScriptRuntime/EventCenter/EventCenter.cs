using UnityEngine;
using System;

public class EventCenter {

    public Action<int> OnClickBagGridHandle;
    public void Panel_Bag_OnClickGrid(int typeID) {
        if (typeID == -1) {
            return;
        }
        OnClickBagGridHandle.Invoke(typeID);
    }
}