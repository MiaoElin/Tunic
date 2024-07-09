using UnityEngine;
using UnityEngine.UI;
using System;

public class Panel_Bag : MonoBehaviour {
    [SerializeField] Text title;
    [SerializeField] Button shooter_Btn;
    [SerializeField] Button sword_Btn;
    [SerializeField] Button eating_Btn;

    public Action OnclickShooterHandle;
    public Action OnclickSwordHandle;
    public Action OnclickEatingHandle;

    public Transform shooter_Group;
    public Transform sword_Group;
    public Transform eating_Group;

    public int gridCountPerGroup;
    Panel_BagElement[] elements;
    [SerializeField] Panel_BagElement prefab;


    public Action<int> OnclickGridHandle;


    public void Ctor() {
        elements = new Panel_BagElement[60];
        gridCountPerGroup = CommonConst.BAG_GRIDCOUNT_PERGROUP;

        shooter_Btn.onClick.AddListener(() => {
            SetCurrentBtn(shooter_Btn);
        });

        sword_Btn.onClick.AddListener(() => {
            SetCurrentBtn(sword_Btn);
        });

        eating_Btn.onClick.AddListener(() => {
            SetCurrentBtn(eating_Btn);
        });

        for (int i = 0; i < gridCountPerGroup * 3; i++) {
            Transform trans;
            if (i >= gridCountPerGroup * 2) {
                trans = eating_Group;
            } else if (i >= gridCountPerGroup) {
                trans = sword_Group;
            } else {
                trans = shooter_Group;
            }
            Panel_BagElement ele = GameObject.Instantiate(prefab, trans);
            ele.Ctor(OnclickGridHandle, -1, 0, null);
            elements[i] = ele;
        }

    }

    public void ClickGrid(int typeID) {
        OnclickGridHandle.Invoke(typeID);
    }

    public Button GetSword_Btn() {
        return sword_Btn;
    }

    public void SetCurrentBtn(Button btn) {

        // 将所有按钮颜色设为透明
        shooter_Btn.image.color = new Color(1, 1, 1, (float)80 / 255);
        sword_Btn.image.color = new Color(1, 1, 1, (float)80 / 255);
        eating_Btn.image.color = new Color(1, 1, 1, (float)80 / 255);
        // 将所有elementGroup隐藏
        shooter_Group.gameObject.SetActive(false);
        sword_Group.gameObject.SetActive(false);
        eating_Group.gameObject.SetActive(false);
        // 将选中的按钮设为不透明
        btn.image.color = new Color(1, 1, 1, 1);
        // 显示对应的elementGroup
        if (btn.name == "shooter_Btn") {
            shooter_Group.gameObject.SetActive(true);
        }
        if (btn.name == "sword_Btn") {
            sword_Group.gameObject.SetActive(true);
        }
        if (btn.name == "eating_Btn") {
            eating_Group.gameObject.SetActive(true);
        }
    }

    public void ElementsCtor() {
        foreach (var ele in elements) {
            Debug.Log(ele.typeID);
            if (ele.typeID == -1) {
                return;
            }
            ele.Init(-1, 0, null);
        }
    }

    public void Init(int index, int typeID, int count, Sprite sprite) {
        if (typeID == -1) {
            elements[index].Init(-1, 0, null);
            return;
        }
        elements[index].Init(typeID, count, sprite);
    }
}