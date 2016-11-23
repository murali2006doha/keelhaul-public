﻿using System;
using UnityEngine;
using InControl;


public abstract class AbstractInputManager : MonoBehaviour {
    public PlayerActions actions { get; set; }
    public Action<Vector3> onRotateChanged { get; set; }
    public Action onBlueButton { get; set; }
}
