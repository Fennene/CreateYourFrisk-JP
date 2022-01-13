using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using MoonSharp.Interpreter;

public class OptionsScript : MonoBehaviour {
    // used to prevent the player from erasing real/almighty globals or their save by accident
    private int RealGlobalCooldown;
    private int AlMightyGlobalCooldown;
    private int SaveCooldown;

    // used to update the Description periodically
    private int DescriptionTimer;

    // game objects
    public GameObject ResetRG, ResetAG, ClearSave, Safe, Retro, Fullscreen, Scale, Discord, Exit;
    public Text Description;

    // Use this for initialization
    private void Start() {
        // add button functions

        // reset RealGlobals
        ResetRG.GetComponent<Button>().onClick.AddListener(() => {
            if (RealGlobalCooldown > 0) {
                LuaScriptBinder.ClearVariables();
                RealGlobalCooldown = 60 * 2;
                ResetRG.GetComponentInChildren<Text>().text = !GlobalControls.crate ? "Real Globals Erased!" : "REEL GOLBELZ DELEET!!!!!";
            } else {
                RealGlobalCooldown = 60 * 2;
                ResetRG.GetComponentInChildren<Text>().text = !GlobalControls.crate ? "Are you sure?" : "R U SUR???";
            }
        });

        // reset AlMightyGlobals
        ResetAG.GetComponent<Button>().onClick.AddListener(() => {
            if (AlMightyGlobalCooldown > 0) {
                LuaScriptBinder.ClearAlMighty();
                AlMightyGlobalCooldown = 60 * 2;
                ResetAG.GetComponentInChildren<Text>().text = !GlobalControls.crate ? "AlMighty Globals Erased!" : "ALMEIGHTIZ DELEET!!!!!";
            } else {
                AlMightyGlobalCooldown = 60 * 2;
                ResetAG.GetComponentInChildren<Text>().text = !GlobalControls.crate ? "Are you sure?" : "R U SUR???";
            }
        });

        // clear Save
        ClearSave.GetComponent<Button>().onClick.AddListener(() => {
            if (SaveCooldown > 0) {
                File.Delete(Application.persistentDataPath + "/save.gd");
                SaveCooldown = 60 * 2;
                ClearSave.GetComponentInChildren<Text>().text = !GlobalControls.crate ? "Save wiped!" : "RIP";
            } else {
                SaveCooldown = 60 * 2;
                ClearSave.GetComponentInChildren<Text>().text = !GlobalControls.crate ? "Are you sure?" : "R U SUR???";
            }
        });

        // toggle safe mode
        Safe.GetComponent<Button>().onClick.AddListener(() => {
            ControlPanel.instance.Safe = !ControlPanel.instance.Safe;

            // save Safe Mode preferences to AlMighties
            LuaScriptBinder.SetAlMighty(null, "CYFSafeMode", DynValue.NewBoolean(ControlPanel.instance.Safe), true);

            Safe.GetComponentInChildren<Text>().text = !GlobalControls.crate
                ? ("Safe mode: " + (ControlPanel.instance.Safe ? "On" : "Off"))
                : ("SFAE MDOE: " + (ControlPanel.instance.Safe ? "ON" : "OFF"));
        });
        Safe.GetComponentInChildren<Text>().text = !GlobalControls.crate
            ? ("Safe mode: " + (ControlPanel.instance.Safe ? "On" : "Off"))
            : ("SFAE MDOE: " + (ControlPanel.instance.Safe ? "ON" : "OFF"));

        // toggle retrocompatibility mode
        Retro.GetComponent<Button>().onClick.AddListener(() => {
            GlobalControls.retroMode =!GlobalControls.retroMode;

            // save RetroMode preferences to AlMighties
            LuaScriptBinder.SetAlMighty(null, "CYFRetroMode", DynValue.NewBoolean(GlobalControls.retroMode), true);

            Retro.GetComponentInChildren<Text>().text = !GlobalControls.crate
                ? ("Retrocompatibility Mode: " + (GlobalControls.retroMode ? "On" : "Off"))
                : ( "RETORCMOAPTIILBIYT MOD: " + (GlobalControls.retroMode ? "ON" : "OFF"));
        });
        Retro.GetComponentInChildren<Text>().text = !GlobalControls.crate
            ? ("Retrocompatibility Mode: " + (GlobalControls.retroMode ? "On" : "Off"))
            : ( "RETORCMOAPTIILBIYT MOD: " + (GlobalControls.retroMode ? "ON" : "OFF"));

        // toggle pixel-perfect fullscreen
        Fullscreen.GetComponent<Button>().onClick.AddListener(() => {
            ScreenResolution.perfectFullscreen =!ScreenResolution.perfectFullscreen;

            // save RetroMode preferences to AlMighties
            LuaScriptBinder.SetAlMighty(null, "CYFPerfectFullscreen", DynValue.NewBoolean(ScreenResolution.perfectFullscreen), true);

            Fullscreen.GetComponentInChildren<Text>().text = !GlobalControls.crate
                ? ( "Blurless Fullscreen: " + (ScreenResolution.perfectFullscreen ? "On" : "Off"))
                : ("NOT UGLEE FULLSCREEN: " + (ScreenResolution.perfectFullscreen ? "ON" : "OFF"));
        });
        Fullscreen.GetComponentInChildren<Text>().text = !GlobalControls.crate
            ? ( "Blurless Fullscreen: " + (ScreenResolution.perfectFullscreen ? "On" : "Off"))
            : ("NOT UGLEE FULLSCREEN: " + (ScreenResolution.perfectFullscreen ? "ON" : "OFF"));

        // change window scale
        Scale.GetComponent<Button>().onClick.AddListener(() => {
            double maxScale = System.Math.Floor(Screen.currentResolution.height / 480.0);
            if (ScreenResolution.windowScale < maxScale)
                ScreenResolution.windowScale += 1;
            else
                ScreenResolution.windowScale = 1;

            if (Screen.height != ScreenResolution.windowScale * 480 && !Screen.fullScreen)
                ScreenResolution.SetFullScreen(false);

            // save RetroMode preferences to AlMighties
            LuaScriptBinder.SetAlMighty(null, "CYFWindowScale", DynValue.NewNumber(ScreenResolution.windowScale), true);

            Scale.GetComponentInChildren<Text>().text = !GlobalControls.crate
                ? ( "Window Scale: " + ScreenResolution.windowScale.ToString() + "x")
                : ("WEENDO STRECH: " + ScreenResolution.windowScale.ToString() + "X");
        });
        ScreenResolution.windowScale--;
        Scale.GetComponent<Button>().onClick.Invoke();

        // Discord Rich Presence
        // Change Discord Status Visibility
        Discord.GetComponent<Button>().onClick.AddListener(() => {
            Discord.GetComponentInChildren<Text>().text = (!GlobalControls.crate ? "Discord Display: " : "DEESCORD DESPLAY: ") + DiscordControls.ChangeVisibilitySetting(1);
        });
        Discord.GetComponentInChildren<Text>().text = (!GlobalControls.crate ? "Discord Display: " : "DEESCORD DESPLAY: ") + DiscordControls.ChangeVisibilitySetting(0);

        // exit
        Exit.GetComponent<Button>().onClick.AddListener(() => {SceneManager.LoadScene("ModSelect");});

        // Crate Your Frisk
        if (!GlobalControls.crate) return;
        // labels
        GameObject.Find("OptionsLabel").GetComponent<Text>().text     = "OPSHUNS";
        GameObject.Find("DescriptionLabel").GetComponent<Text>().text = "MORE TXET";

        // buttons
        ResetRG.GetComponentInChildren<Text>().text   = "RESTE RELA GOLBALZ";
        ResetAG.GetComponentInChildren<Text>().text   = "RESTE ALMIGTY GOLBALZ";
        ClearSave.GetComponentInChildren<Text>().text = "WYPE SAV";
        Exit.GetComponentInChildren<Text>().text      = "EXIT TOO MAD SELCT";
    }

    // Gets the text the description should use based on what button is currently being hovered over
    private string GetDescription(string buttonName) {
        string response;
        switch(buttonName) {
            case "ResetRG":
                response = "全ての Real Globals を削除します。\n\n"
                         + "Real Globals は\nCYFを開いている間存続する変数です。\n"
                         + "ただし、CYFを閉じると削除されます。";
                return !GlobalControls.crate ? response : Temmify.Convert(response);
            case "ResetAG":
                response = "全ての AlMighty Globals を削除します。\n\n"
                         + "AlMighty Globals は\nファイルに保存される変数です。\n"
                         + "CYFを閉じてもデータは残ります。\n\n"
                         + "このオプションも AlMighty Globals として保存されています。";
                return !GlobalControls.crate ? response : Temmify.Convert(response);
            case "ClearSave":
                response = "セーブファイルを初期化します。\n\n"
                         + "CYFのオーバーワールドのセーブファイルを\n削除します。\n\n"
                         + "セーブファイルの場所は以下の通り:\n\n";
                if (!GlobalControls.crate)
                    // return response + Application.persistentDataPath + "/save.gd</size></b>";
                    return response + "<b><size='14'>" + Application.persistentDataPath + "/save.gd</size></b>";
                else
                    return Temmify.Convert(response) + "<b><size='14'>" + Application.persistentDataPath + "/save.gd</size></b>";
            case "Safe":
                response = "セーフモードの切り替え\n\n"
                         + "このモード自体に機能はありませんが、\n"
                         + "Mod作成者がこの設定を確認して、\n"
                         + "流血表現や冒涜的な内容などに\n"
                         + "フィルタリングする可能性があります。";
                return !GlobalControls.crate ? response : Temmify.Convert(response);
            case "Retro":
                response = "Retrocompatibility Mode の切り替え\n\n"
                         + "このモードは Unitale v0.2.1a で作成されたコンテンツを動作させるための\nモードです。\n\n\n\n";
                if (!GlobalControls.crate)
                    return response + "<b>警告!\n</b> CYFで作られたModではこのモードを\n<b>無効(off)</b>にしてください。\nこの機能は <b>Unitale v0.2.1a</b> で作られたModのみに使用してください。";
                else
                    return Temmify.Convert(response) + "<b>" + Temmify.Convert("CAUTION!\nDISABLE") + "</b> " + Temmify.Convert("this for mods made for CYF.");
            case "Fullscreen":
                response = "鮮明なフルスクリーンモードの切り替え\n\n"
                         + "これはフルスクリーンにしたときにぼやけるかぼやけないようにするか決められます。\n\n\n"
                         + "<b>F4キー</b>または<b>Alt+Enterキー</b>でフルスクリーンになります";
                return !GlobalControls.crate ? response : Temmify.Convert(response);
            case "Scale":
                response = "ウィンドウの表示倍率\n\n"
                         + "大きなスクリーン(例えば4kのスクリーン)で\n表示する場合に役に立ちます。\n\n"
                         + "フルスクリーンでは何の影響もありません。";
                return !GlobalControls.crate ? response : Temmify.Convert(response);
            case "Discord":
                response = "CYFをプレイ中にDiscordでどのように表示するのか決めます。(Discord Rich Presence)\n\n"
                         + "<b>Everything</b>: 全て表示: 遊んでいるModや時間、説明の全てを表示\n\n"
                         + "<b>Game Only</b>: CreateYourFriskで遊んでいることだけを表示します。\n\n"
                         + "<b>Nothing</b>: 何も表示しません。\n\n"
                         + "CYFのDiscorfへの接続が切断された場合、正しくDiscordに表示させるにはCYFを再起動する必要性があります。";
                return !GlobalControls.crate ? response : Temmify.Convert(response);
            case "Exit":
                response = "Mod選択画面に戻ります。";
                return !GlobalControls.crate ? response : Temmify.Convert(response);
            default:
                return !GlobalControls.crate ? "オプションにマウスを合わせると、\nここに説明が表示されます。" : "HOVR OVR DA TING N GET TEXT HEAR!!";
        }
    }

    // Used to animate scrolling left or right.
    private void Update() {
        // update the description every 1/6th of a second
        if (DescriptionTimer > 0)
            DescriptionTimer--;
        else {
            DescriptionTimer = 10;

            // try to find which button the player is hovering over
            string hoverItem = "";
            // if the player is within the range of the buttons
            int mousePosX = (int)((ScreenResolution.mousePosition.x / ScreenResolution.displayedSize.x) * 640);
            int mousePosY = (int)((Input.mousePosition.y / ScreenResolution.displayedSize.y) * 480);
            if (mousePosX >= 40 && mousePosX <= 290) {
                // ResetRG
                if      (mousePosY <= 420 && mousePosY > 380)
                    hoverItem = "ResetRG";
                // ResetAG
                else if (mousePosY <= 380 && mousePosY > 340)
                    hoverItem = "ResetAG";
                // ClearSave
                else if (mousePosY <= 340 && mousePosY > 300)
                    hoverItem = "ClearSave";
                // Safe
                else if (mousePosY <= 300 && mousePosY > 260)
                    hoverItem = "Safe";
                // Retro
                else if (mousePosY <= 260 && mousePosY > 220)
                    hoverItem = "Retro";
                // Fullscreen
                else if (mousePosY <= 220 && mousePosY > 180)
                    hoverItem = "Fullscreen";
                // Scale
                else if (mousePosY <= 180 && mousePosY > 140)
                    hoverItem = "Scale";
                // Discord
                else if (mousePosY <= 140 && mousePosY > 100)
                    hoverItem = "Discord";
                // Exit
                else if (mousePosY <=  60 && mousePosY >  20)
                    hoverItem = "Exit";
            }

            Description.GetComponent<Text>().text = GetDescription(hoverItem);
        }

        // make the player click twice to reset RG or AG, or to wipe their save
        if (RealGlobalCooldown > 0)
            RealGlobalCooldown -= 1;
        else if (RealGlobalCooldown == 0) {
            RealGlobalCooldown = -1;
            ResetRG.GetComponentInChildren<Text>().text = !GlobalControls.crate ? "Reset Real Globals" : "RSETE RAEL GLOBALS";
        }

        if (AlMightyGlobalCooldown > 0)
            AlMightyGlobalCooldown -= 1;
        else if (AlMightyGlobalCooldown == 0) {
            AlMightyGlobalCooldown = -1;
            ResetAG.GetComponentInChildren<Text>().text = !GlobalControls.crate ? "Reset AlMighty Globals" : "RESET ALIMGHTY";
        }

        if (SaveCooldown > 0)
            SaveCooldown -= 1;
        else if (SaveCooldown == 0) {
            SaveCooldown = -1;
            ClearSave.GetComponentInChildren<Text>().text = !GlobalControls.crate ? "Wipe Save" : "WYPE SAV";
        }
    }
}
