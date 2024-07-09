using UnityEngine;

public class GameFSMComponent {
    public GameStatus status;

    public bool isEnterNormal;

    public bool isEnterOpenBag;

    public GameFSMComponent() {
        status = new GameStatus();
    }

    public void EnterNormal() {
        status = GameStatus.Normal;
        isEnterNormal = true;
    }

    public void EnterOpenBag() {
        status = GameStatus.OpenBag;
        isEnterOpenBag = true;
    }

}