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

    public int groupCount;
    Panel_BagElement[] elements;
    [SerializeField] Panel_BagElement prefab;

    public void Ctor() {
        for (int i = 0; i < groupCount * 3; i++) {
            Transform trans;
            if (i >= groupCount * 2) {
                trans = eating_Group;
            } else if (i >= groupCount) {
                trans = sword_Group;
            } else {
                trans = shooter_Group;
            }
            Panel_BagElement ele = GameObject.Instantiate(prefab, trans);
            ele.Ctor(0);
        }

    }

    public Button GetSword_Btn() {
        return sword_Btn;
    }

    public void SetCurrentBtn(Button btn) {

        // 将所有按钮颜色设为透明
        shooter_Btn.image.color = new Color(1, 1, 1, (float)80 / 255);
        sword_Btn.image.color = new Color(1, 1, 1, (float)80 / 255);
        eating_Btn.image.color = new Color(1, 1, 1, (float)80 / 255);

        shooter_Group.gameObject.SetActive(false);
        sword_Group.gameObject.SetActive(false);
        eating_Group.gameObject.SetActive(false);
        // 将选中的按钮设为不透明
        btn.image.color = new Color(1, 1, 1, 1);
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



}