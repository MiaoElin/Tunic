using UnityEngine;

public class GameFSMComponent {
    public GameStatus status;

    public bool isEnterNormal;

    public GameFSMComponent() {
        status = new GameStatus();
    }

    public void EnterNormal() {
        status = GameStatus.Normal;
        isEnterNormal = true;
    }

}