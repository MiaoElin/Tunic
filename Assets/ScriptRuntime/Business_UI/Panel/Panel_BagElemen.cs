using UnityEngine;
using UnityEngine.UI;

public class Panel_BagElement : MonoBehaviour {
    public int count;
    [SerializeField] Image BG_Normal;
    [SerializeField] Image BG_Selected;
    [SerializeField] Text txt_Count;

    public void Ctor(int count) {
        this.count = count;
        if (count == 0) {
            txt_Count.text = "";
        } else {
            txt_Count.text = count.ToString();
        }
    }
}