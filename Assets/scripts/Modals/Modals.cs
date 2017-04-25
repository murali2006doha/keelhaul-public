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
    internal static string statsModalPath = "Prefabs/UI/StatsModal";

    public static Dictionary<ModalsEnum, string> typeToModalPrefab = new Dictionary<ModalsEnum, string>(){
        { ModalsEnum.pauseModal, "Prefabs/Modals/PauseModal"},
        { ModalsEnum.notificationModal, "Prefabs/Modals/NotificationModal"},
        { ModalsEnum.settingsModal, "Prefabs/Modals/SettingsModalNew"},
        { ModalsEnum.statsModal, "Prefabs/UI/StatsModal"},
        { ModalsEnum.disconnectModal, "Prefabs/Modals/DisconnectModal"}
    };


}

