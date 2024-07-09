using UnityEngine;
using UnityEngine.UI;
using System;

public class Panel_BagElement : MonoBehaviour {

    public int typeID;
    public int count;
    [SerializeField] Image BG_Normal;
    [SerializeField] Image BG_Selected;
    [SerializeField] Image icon;
    [SerializeField] Text txt_Count;
    [SerializeField] Button btn;


    public void Ctor(Action<int> OnclickGridHandle, int typeID, int count, Sprite sprite) {

        btn.onClick.AddListener(() => {
            OnclickGridHandle.Invoke(this.typeID);
        });

        Init(typeID, count, sprite);

    }

    public void Init(int typeID, int count, Sprite sprite) {
        this.typeID = typeID;
        this.count = count;
        if (count == 0) {
            txt_Count.text = "";
        } else {
            txt_Count.text = "X" + count.ToString();
        }

        if (sprite == null) {
            icon.sprite = null;
            icon.gameObject.SetActive(false);
        } else {
            icon.gameObject.SetActive(true);
            icon.sprite = sprite;
        }
    }
}