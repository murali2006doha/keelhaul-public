using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class Modals : MonoBehaviour
{

    //Others
    internal static string pauseModal = "sprites/character_portraits_assets/press_A";
    internal static string notificationModal = "sprites/Character Select UI/deselect";

    public static Dictionary<ModalsEnum, string> typeToModalPrefab = new Dictionary<ModalsEnum, string>(){
        { ModalsEnum.pauseModal, "Prefabs/Modals/PauseModal"},
        {  ModalsEnum.notificationModal, "Prefabs/Modals/NotificationModal"},
        {  ModalsEnum.settingsModal, "Prefabs/Modals/SettingsModal"},
    };


}

