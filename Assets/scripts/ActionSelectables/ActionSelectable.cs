using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public abstract class ActionSelectable : MonoBehaviour {

	protected bool mouseHovering = false;

	public abstract void doAction () ;

	public void setHoveringTrue() {
		mouseHovering = true;
	}


	public void setHoveringFalse() {
		mouseHovering = false;
	}


	public bool isMouseHovering() {
		return mouseHovering;
	}
}