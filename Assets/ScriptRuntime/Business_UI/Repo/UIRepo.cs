using UnityEngine;
using System.Collections.Generic;

public class UIRepo {

    Dictionary<string, GameObject> all;

    public UIRepo() {
        all = new Dictionary<string, GameObject>();
    }

    public void Add(string name, GameObject ui) {
        all.Add(name, ui);
    }

    public void Remove(string name) {
        all.Remove(name);
    }

    public T TryGet<T>() where T : MonoBehaviour {
        var name = typeof(T).Name;
        all.TryGetValue(name, out GameObject ui);
        if (ui == null) {
            return null;
        } else {
            return ui.GetComponent<T>();
        }
    }
}