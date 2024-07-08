using System;
using UnityEngine;

public class UIContext {

    public Transform hudCanvas;
    public Transform screenCanvas;
    public Asset_Core asset;

    public UIRepo uIRepo;
    public HUD_HintsRepo hUD_HintsRepo;

    public UIContext() {
        uIRepo = new UIRepo();
        hUD_HintsRepo = new HUD_HintsRepo();
    }

    internal void Inject(Canvas hudCanvas, Canvas screenCanvas, Asset_Core asset) {
        this.hudCanvas = hudCanvas.transform;
        this.screenCanvas = screenCanvas.transform;
        this.asset = asset;
    }
}