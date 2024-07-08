using UnityEngine;
using UnityEngine.UI;

public class Panel_BagElement : MonoBehaviour {
    public int count;
    [SerializeField] Image BG_Normal;
    [SerializeField] Image BG_Selected;
    [SerializeField] Image icon;
    [SerializeField] Text txt_Count;

    public void Ctor(int count, Sprite sprite) {
        this.count = count;
        if (count == 0) {
            txt_Count.text = "";
        } else {
            txt_Count.text = "X" + count.ToString();
        }

        if (sprite == null) {
            icon.gameObject.SetActive(false);
        } else {
            icon.gameObject.SetActive(true);
            icon.sprite = sprite;
        }
    }
}