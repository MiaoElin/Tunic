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

    
}