using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Canvas mainCanvas;
    //panels
    public GameObject UIpanel, menuPanel, legoPanel, helpPanel, avatarPanel, kitPanel;

    //top menu buttons
    public Button menuButton, legoButton, undoButton, zoomInButton, zoomOutButton;
    //Lego  buttons
    public Button onex1, onex2, onex3, onex4, onex6, onex8, twox2, twox3, twox4, twox6, twox8;
    //Main menu buttons
    public Button kit, multiplayer, avatar, help, quit, sandbox;
    //Avatar buttons
    public Button hatButton, backButton, handButton, faceButton, bodyButton, legButton, saveButton, avatarBackButton;
    //Avatar pieces
    public GameObject hat, face, back, hand, chest, leg, skin, demoAvatar;
    //kit buttons
    public GameObject sphynxButton, pyramidButton, kitBackButton;

    public GameObject a_hat,a_face,a_back,a_hand,a_chest,a_leg,a_skin;
    public GameObject cameraParent, minifig;

    public int [] avatar_color_array = new int [6];
    public int [] avatar_piece_array = new int [4];

    public int [] temp_color_array = new int [6];
    public int [] temp_piece_array = new int [4];
    public  Material[] face_array;
    public Material[] avatar_colors;
    public  GameObject[] item_array;
    public  GameObject[] back_array;
    public  GameObject[] hat_array;
    
    UnityEngine.Color blackDark = new Color32(54, 54, 54, 255);
    UnityEngine.Color blackLight = new Color32(100, 100, 100, 255);
    UnityEngine.Color clear = new Color32(39, 39, 39, 0);
    UnityEngine.Color purple = new Color32(180, 104, 236, 100);
    UnityEngine.Color purple2 = new Color32(187, 134, 252, 114);

    public static bool menuMode;
    public static int selectedBlock;
    float timer, nextInputTimer;
    public static int scale = 0;
    public static bool last_open = false;

    public AudioSource audioSource;
    public AudioClip scrollSound, selectMenuSound, multiConnectSound, multiJoinSound, exitSound, selectUISound;

    void Start()
    {
        timer = 0;
        nextInputTimer = 0;
        selectedBlock = 0;
        HideMainMenu();
        HideLegoMenu();
        HideHelpMenu();
        HideAvatarMenu();
        HideKitMenu();
        LoadAvatar();
        //ResetData();
        audioSource.PlayOneShot(multiConnectSound);
    }


    void Update()
    {
        
        timer += Time.deltaTime;
        if(nextInputTimer < 5)
            nextInputTimer += Time.deltaTime;
        if(nextInputTimer > .1)
            last_open = false;
        
        
        if(!PlayerMovement.lego_selected && !LegoController.kit_mode)
        {
            if(Input.GetButtonUp("Y"))
            {
                if(!menuMode)
                {
                    EnterMenuMode();
                    audioSource.PlayOneShot(scrollSound);
                }
                else if(!menuPanel.activeInHierarchy & !legoPanel.activeInHierarchy & !helpPanel.activeInHierarchy & !avatarPanel.activeInHierarchy & !kitPanel.activeInHierarchy)
                {
                    SwapMenu();
                    audioSource.PlayOneShot(scrollSound);
                }
                else if(avatarPanel.activeInHierarchy)
                {
                    demoAvatar.transform.Rotate(0, 0, 45);
                    audioSource.PlayOneShot(selectMenuSound);
                }
            }
            else if(Input.GetButtonUp("A") & menuMode)
            {
                ExitMenuMode();
                audioSource.PlayOneShot(selectMenuSound);
            }
            else if(Input.GetButtonUp("B") & menuMode)
            {
                if(menuPanel.activeInHierarchy)
                {
                    SelectMainMenu();
                    audioSource.PlayOneShot(selectUISound);
                }
                else if(legoPanel.activeInHierarchy)
                {
                    SelectLego();
                    audioSource.PlayOneShot(selectMenuSound);
                }
                else if (helpPanel.activeInHierarchy)
                {
                    audioSource.PlayOneShot(selectMenuSound);
                    HideHelpMenu();
                    menuPanel.SetActive(true);
                    help.GetComponent<Image>().color = blackLight;
                }
                else if(avatarPanel.activeInHierarchy)
                {
                    SelectAvatar();
                    audioSource.PlayOneShot(selectMenuSound);
                }
                else if(kitPanel.activeInHierarchy)
                {
                    SelectKit();
                    audioSource.PlayOneShot(selectMenuSound);
                }
                else
                {
                    SelectMenu();
                    audioSource.PlayOneShot(selectUISound);
                }
                
            }
            else if(Input.GetButtonUp("X") & menuMode)
            {
                SelectAvatarColor();
                audioSource.PlayOneShot(selectMenuSound);
            }

            var temp1 = Input.GetAxis("Horizontal");
            var temp2 = Input.GetAxis("Vertical");

            if((temp1 != 0 | temp2 != 0) & timer > 0.3 & legoPanel.activeInHierarchy)
            {
                timer = 0;
                if(Mathf.Abs(temp1) > Mathf.Abs(temp2))
                {
                    if(temp1 > 0)
                    {
                        SwapLegoHorizontalR();
                        audioSource.PlayOneShot(scrollSound);
                    }
                    else
                    {
                        SwapLegoHorizontalL();
                        audioSource.PlayOneShot(scrollSound);
                    }
                }
                else
                {
                    SwapLegoVertical();
                    audioSource.PlayOneShot(scrollSound);
                }
            }
            else if((temp1 != 0 | temp2 != 0) & timer > 0.3 & avatarPanel.activeInHierarchy)
            {
                timer = 0;
                if(Mathf.Abs(temp1) > Mathf.Abs(temp2))
                {
                   
                    SwapAvatarHorizontal();
                    audioSource.PlayOneShot(scrollSound);
                   
                }
                else
                {
                    if(temp2 >0)
                    {
                        SwapAvatarVerticalU();
                        audioSource.PlayOneShot(scrollSound);
                    }
                    else
                    {
                        SwapAvatarVerticalD();
                        audioSource.PlayOneShot(scrollSound);
                    }
                }
            }

            else if((temp1 != 0 | temp2 != 0) & timer > 0.3 & menuPanel.activeInHierarchy)
            {
                timer = 0;
                if(Mathf.Abs(temp1) > Mathf.Abs(temp2))
                {
                    if(temp1 > 0)
                    {
                        SwapMainMenuR();
                        audioSource.PlayOneShot(scrollSound);
                    }
                    else
                    {
                        SwapMainMenuL();
                        audioSource.PlayOneShot(scrollSound);
                    }
                }
                else
                {
                    if(temp2 > 0)
                    {
                        SwapMainMenuUp();
                        audioSource.PlayOneShot(scrollSound);
                    }
                    else
                    {
                        SwapMainMenuDown();
                        audioSource.PlayOneShot(scrollSound);
                    }
                }
            }

            else if((temp1 != 0 | temp2 != 0) & timer > 0.3 & kitPanel.activeInHierarchy)
            {
                timer = 0;
                if(Mathf.Abs(temp1) > Mathf.Abs(temp2))
                {
                    SwapKitHorizontal();
                    audioSource.PlayOneShot(scrollSound);
                }
                else
                {
                    SwapKitVerticle();
                    audioSource.PlayOneShot(scrollSound);
                }
            }

        }
    }

    public void SelectLego()
    {
        if(onex1.GetComponent<Image>().color != clear)
        {
            selectedBlock = 0;
        }
        else if(onex2.GetComponent<Image>().color != clear)
        {
            selectedBlock = 1;
        }
        else if(onex3.GetComponent<Image>().color != clear)
        {
            selectedBlock = 2;
        }
        else if(onex4.GetComponent<Image>().color != clear)
        {
            selectedBlock = 3;
        }
        else if(onex6.GetComponent<Image>().color != clear)
        {
            selectedBlock = 4;
        }
        else if(onex8.GetComponent<Image>().color != clear)
        {
            selectedBlock = 5;
        }
        else if(twox2.GetComponent<Image>().color != clear)
        {
            selectedBlock = 6;
        }
        else if(twox3.GetComponent<Image>().color != clear)
        {
            selectedBlock = 7;
        }
        else if(twox4.GetComponent<Image>().color != clear)
        {
            selectedBlock = 8;
        }
        else if(twox6.GetComponent<Image>().color != clear)
        {
            selectedBlock = 9;
        }
        else if(twox8.GetComponent<Image>().color != clear)
        {
            selectedBlock = 10;
        }
        HideMainMenu();
        HideLegoMenu();
        HideKitMenu();
        HideHelpMenu();
        HideAvatarMenu();
        menuButton.GetComponent<Image>().color = blackDark;
        legoButton.GetComponent<Image>().color = blackDark;
        undoButton.GetComponent<Image>().color = blackDark;
        zoomInButton.GetComponent<Image>().color = blackDark;
        zoomOutButton.GetComponent<Image>().color = blackDark;
    }

    public void SwapLegoHorizontalR()
    {
        if(onex1.GetComponent<Image>().color != clear)
        {
            onex1.GetComponent<Image>().color = clear;
            onex2.GetComponent<Image>().color = purple;
        }
        else if(onex2.GetComponent<Image>().color != clear)
        {
            onex2.GetComponent<Image>().color = clear;
            onex3.GetComponent<Image>().color = purple;
        }
        else if(onex3.GetComponent<Image>().color != clear)
        {
            onex3.GetComponent<Image>().color = clear;
            onex4.GetComponent<Image>().color = purple;
        }
        else if(onex4.GetComponent<Image>().color != clear)
        {
            onex4.GetComponent<Image>().color = clear;
            onex6.GetComponent<Image>().color = purple;
        }
        else if(onex6.GetComponent<Image>().color != clear)
        {
            onex6.GetComponent<Image>().color = clear;
            onex8.GetComponent<Image>().color = purple;
        }
        else if(onex8.GetComponent<Image>().color != clear)
        {
            onex8.GetComponent<Image>().color = clear;
            onex1.GetComponent<Image>().color = purple;
        }
        else if(twox2.GetComponent<Image>().color != clear)
        {
            twox2.GetComponent<Image>().color = clear;
            twox3.GetComponent<Image>().color = purple;
        }
        else if(twox3.GetComponent<Image>().color != clear)
        {
            twox3.GetComponent<Image>().color = clear;
            twox4.GetComponent<Image>().color = purple;
        }
        else if(twox4.GetComponent<Image>().color != clear)
        {
            twox4.GetComponent<Image>().color = clear;
            twox6.GetComponent<Image>().color = purple;
        }
        else if(twox6.GetComponent<Image>().color != clear)
        {
            twox6.GetComponent<Image>().color = clear;
            twox8.GetComponent<Image>().color = purple;
        }
        else if(twox8.GetComponent<Image>().color != clear)
        {
            twox8.GetComponent<Image>().color = clear;
            twox2.GetComponent<Image>().color = purple;
        }
    }

    public void SwapLegoHorizontalL()
    {
        if(onex1.GetComponent<Image>().color != clear)
        {
            onex1.GetComponent<Image>().color = clear;
            onex8.GetComponent<Image>().color = purple;
        }
        else if(onex2.GetComponent<Image>().color != clear)
        {
            onex2.GetComponent<Image>().color = clear;
            onex1.GetComponent<Image>().color = purple;
        }
        else if(onex3.GetComponent<Image>().color != clear)
        {
            onex3.GetComponent<Image>().color = clear;
            onex2.GetComponent<Image>().color = purple;
        }
        else if(onex4.GetComponent<Image>().color != clear)
        {
            onex4.GetComponent<Image>().color = clear;
            onex3.GetComponent<Image>().color = purple;
        }
        else if(onex6.GetComponent<Image>().color != clear)
        {
            onex6.GetComponent<Image>().color = clear;
            onex4.GetComponent<Image>().color = purple;
        }
        else if(onex8.GetComponent<Image>().color != clear)
        {
            onex8.GetComponent<Image>().color = clear;
            onex6.GetComponent<Image>().color = purple;
        }
        else if(twox2.GetComponent<Image>().color != clear)
        {
            twox2.GetComponent<Image>().color = clear;
            twox8.GetComponent<Image>().color = purple;
        }
        else if(twox3.GetComponent<Image>().color != clear)
        {
            twox3.GetComponent<Image>().color = clear;
            twox2.GetComponent<Image>().color = purple;
        }
        else if(twox4.GetComponent<Image>().color != clear)
        {
            twox4.GetComponent<Image>().color = clear;
            twox3.GetComponent<Image>().color = purple;
        }
        else if(twox6.GetComponent<Image>().color != clear)
        {
            twox6.GetComponent<Image>().color = clear;
            twox4.GetComponent<Image>().color = purple;
        }
        else if(twox8.GetComponent<Image>().color != clear)
        {
            twox8.GetComponent<Image>().color = clear;
            twox6.GetComponent<Image>().color = purple;
        }
    }

    public void SwapLegoVertical()
    {
        if(onex1.GetComponent<Image>().color != clear)
        {
            onex1.GetComponent<Image>().color = clear;
            twox2.GetComponent<Image>().color = purple;
        }
        else if(onex2.GetComponent<Image>().color != clear)
        {
            onex2.GetComponent<Image>().color = clear;
            twox3.GetComponent<Image>().color = purple;
        }
        else if(onex3.GetComponent<Image>().color != clear)
        {
            onex3.GetComponent<Image>().color = clear;
            twox4.GetComponent<Image>().color = purple;
        }
        else if(onex4.GetComponent<Image>().color != clear)
        {
            onex4.GetComponent<Image>().color = clear;
            twox6.GetComponent<Image>().color = purple;
        }
        else if(onex6.GetComponent<Image>().color != clear)
        {
            onex6.GetComponent<Image>().color = clear;
            twox8.GetComponent<Image>().color = purple;
        }
        else if(onex8.GetComponent<Image>().color != clear)
        {
            onex8.GetComponent<Image>().color = clear;
            twox8.GetComponent<Image>().color = purple;
        }
        else if(twox2.GetComponent<Image>().color != clear)
        {
            twox2.GetComponent<Image>().color = clear;
            onex1.GetComponent<Image>().color = purple;
        }
        else if(twox3.GetComponent<Image>().color != clear)
        {
            twox3.GetComponent<Image>().color = clear;
            onex2.GetComponent<Image>().color = purple;
        }
        else if(twox4.GetComponent<Image>().color != clear)
        {
            twox4.GetComponent<Image>().color = clear;
            onex3.GetComponent<Image>().color = purple;
        }
        else if(twox6.GetComponent<Image>().color != clear)
        {
            twox6.GetComponent<Image>().color = clear;
            onex4.GetComponent<Image>().color = purple;
        }
        else if(twox8.GetComponent<Image>().color != clear)
        {
            twox8.GetComponent<Image>().color = clear;
            onex6.GetComponent<Image>().color = purple;
        }
    }

    public void SelectMenu()
    {
        if(menuButton.GetComponent<Image>().color != blackDark)
        {
            ShowMainMenu();
        }
        else if(legoButton.GetComponent<Image>().color != blackDark)
        {
            ShowLegoMenu();
        }
        else if(undoButton.GetComponent<Image>().color != blackDark)
        {
            
            if(PlayerMovement.placed_legos.Count != 0)
            {
                
                GameObject.DestroyImmediate(PlayerMovement.placed_legos.Pop().gameObject);

            }
        }
        else if(zoomInButton.GetComponent<Image>().color != blackDark)
        {
            // 0 = max zoomin, 2 = max zoomout
            if(scale > 0)
            {
                scale -= 1;
                cameraParent.transform.position = minifig.transform.position + new Vector3(0, 2.92f + (3 * scale), 1.03f + (-4 * scale));

            }
        }
        else if(zoomOutButton.GetComponent<Image>().color != blackDark)
        {
            if(scale < 2)
            {
                scale += 1;
                cameraParent.transform.position = minifig.transform.position + new Vector3(0, 2.92f + (3 * scale), 1.03f + (-4 * scale));
            }
        
        }
    }

    public void SwapMenu()
    {
        if(menuButton.GetComponent<Image>().color != blackDark)
        {
            menuButton.GetComponent<Image>().color = blackDark;
            legoButton.GetComponent<Image>().color = purple;
        }
        else if(legoButton.GetComponent<Image>().color != blackDark)
        {
            legoButton.GetComponent<Image>().color = blackDark;
            undoButton.GetComponent<Image>().color = purple;
        }
        else if(undoButton.GetComponent<Image>().color != blackDark)
        {
            undoButton.GetComponent<Image>().color = blackDark;
            zoomInButton.GetComponent<Image>().color = purple;
        }
        else if(zoomInButton.GetComponent<Image>().color != blackDark)
        {
            zoomInButton.GetComponent<Image>().color = blackDark;
            zoomOutButton.GetComponent<Image>().color = purple;
        }
        else if(zoomOutButton.GetComponent<Image>().color != blackDark)
        {
            zoomOutButton.GetComponent<Image>().color = blackDark;
            menuButton.GetComponent<Image>().color = purple;
        }
    }

    public void SwapMainMenuR()
    {
        if(kit.GetComponent<Image>().color != blackDark)
        {
            kit.GetComponent<Image>().color = blackDark;
            multiplayer.GetComponent<Image>().color = blackLight;
        }
        else if(multiplayer.GetComponent<Image>().color != blackDark)
        {
            multiplayer.GetComponent<Image>().color = blackDark;
            avatar.GetComponent<Image>().color = blackLight;
        }
        else if(avatar.GetComponent<Image>().color != blackDark)
        {
            avatar.GetComponent<Image>().color = blackDark;
            kit.GetComponent<Image>().color = blackLight;
        }
        else if(help.GetComponent<Image>().color != blackDark)
        {
            help.GetComponent<Image>().color = blackDark;
            quit.GetComponent<Image>().color = blackLight;
        }
        else if(quit.GetComponent<Image>().color != blackDark)
        {
            quit.GetComponent<Image>().color = blackDark;
            help.GetComponent<Image>().color = blackLight;
        }
    }

    public void SwapMainMenuL()
    {
        if(kit.GetComponent<Image>().color != blackDark)
        {
            kit.GetComponent<Image>().color = blackDark;
            avatar.GetComponent<Image>().color = blackLight;
        }
        else if(multiplayer.GetComponent<Image>().color != blackDark)
        {
            multiplayer.GetComponent<Image>().color = blackDark;
            kit.GetComponent<Image>().color = blackLight;
        }
        else if(avatar.GetComponent<Image>().color != blackDark)
        {
            avatar.GetComponent<Image>().color = blackDark;
            multiplayer.GetComponent<Image>().color = blackLight;
        }
        else if(help.GetComponent<Image>().color != blackDark)
        {
            help.GetComponent<Image>().color = blackDark;
            quit.GetComponent<Image>().color = blackLight;
        }
        else if(quit.GetComponent<Image>().color != blackDark)
        {
            quit.GetComponent<Image>().color = blackDark;
            help.GetComponent<Image>().color = blackLight;
        }
    }

    public void SwapMainMenuUp()
    {
        if(kit.GetComponent<Image>().color != blackDark)
        {
            kit.GetComponent<Image>().color = blackDark;
            help.GetComponent<Image>().color = blackLight;
        }
        else if(multiplayer.GetComponent<Image>().color != blackDark)
        {
            multiplayer.GetComponent<Image>().color = blackDark;
            help.GetComponent<Image>().color = blackLight;
        }
        else if(avatar.GetComponent<Image>().color != blackDark)
        {
            avatar.GetComponent<Image>().color = blackDark;
            help.GetComponent<Image>().color = blackLight;
        }
        else if(help.GetComponent<Image>().color != blackDark)
        {
            help.GetComponent<Image>().color = blackDark;
            sandbox.GetComponent<Image>().color = blackLight;
        }
        else if(quit.GetComponent<Image>().color != blackDark)
        {
            quit.GetComponent<Image>().color = blackDark;
            sandbox.GetComponent<Image>().color = blackLight;
        }
        else if(sandbox.GetComponent<Image>().color != purple2)
        {
            sandbox.GetComponent<Image>().color = purple2;
            kit.GetComponent<Image>().color = blackLight;
        }
    }

    public void SwapMainMenuDown()
    {
        if(kit.GetComponent<Image>().color != blackDark)
        {
            kit.GetComponent<Image>().color = blackDark;
            sandbox.GetComponent<Image>().color = blackLight;
        }
        else if(multiplayer.GetComponent<Image>().color != blackDark)
        {
            multiplayer.GetComponent<Image>().color = blackDark;
            sandbox.GetComponent<Image>().color = blackLight;
        }
        else if(avatar.GetComponent<Image>().color != blackDark)
        {
            avatar.GetComponent<Image>().color = blackDark;
            sandbox.GetComponent<Image>().color = blackLight;
        }
        else if(help.GetComponent<Image>().color != blackDark)
        {
            help.GetComponent<Image>().color = blackDark;
            kit.GetComponent<Image>().color = blackLight;
        }
        else if(quit.GetComponent<Image>().color != blackDark)
        {
            quit.GetComponent<Image>().color = blackDark;
            kit.GetComponent<Image>().color = blackLight;
        }
        else if(sandbox.GetComponent<Image>().color != purple2)
        {
            sandbox.GetComponent<Image>().color = purple2;
            help.GetComponent<Image>().color = blackLight;
        }
    }

    public void SelectMainMenu()
    {
        if(kit.GetComponent<Image>().color != blackDark)
        {
            kit.GetComponent<Image>().color = blackDark;
            ShowKitMenu();
        }
        else if(multiplayer.GetComponent<Image>().color != blackDark)
        {
            //TODO
        }
        else if(avatar.GetComponent<Image>().color != blackDark)
        {
            avatar.GetComponent<Image>().color = blackDark;
            ShowAvatarMenu();
        }
        else if(help.GetComponent<Image>().color != blackDark)
        {
            help.GetComponent<Image>().color = blackDark;
            ShowHelpMenu();
        }
        else if(sandbox.GetComponent<Image>().color != purple2)
        {
            ExitMenuMode();
        }
        else if(quit.GetComponent<Image>().color != blackDark)
        {
            Application.Quit();
        }
    }

    public void EnterMenuMode()
    {
        menuMode = true;
        legoButton.GetComponent<Image>().color = purple;
    }

    public void ExitMenuMode()
    {
        HideMainMenu();
        HideLegoMenu();
        HideHelpMenu();
        HideAvatarMenu();
        HideKitMenu();
        menuButton.GetComponent<Image>().color = blackDark;
        legoButton.GetComponent<Image>().color = blackDark;
        undoButton.GetComponent<Image>().color = blackDark;
        zoomInButton.GetComponent<Image>().color = blackDark;
        zoomOutButton.GetComponent<Image>().color = blackDark;
        nextInputTimer = 0;
        last_open = true;
        menuMode = false;
    }

    public void HideMainMenu()
    {
        kit.GetComponent<Image>().color = blackDark;
        multiplayer.GetComponent<Image>().color = blackDark;
        avatar.GetComponent<Image>().color = blackDark;
        help.GetComponent<Image>().color = blackDark;
        sandbox.GetComponent<Image>().color = purple2;

        menuPanel.SetActive(false);
        UIpanel.SetActive(true);
        menuButton.GetComponent<Image>().color = blackDark;
        menuMode = false;
    }

    public void ShowMainMenu()
    {
        UIpanel.SetActive(false);
        menuPanel.SetActive(true);
        kit.GetComponent<Image>().color = blackLight;
    }

    public void HideLegoMenu()
    {
        legoPanel.SetActive(false);
        legoButton.GetComponent<Image>().color = blackDark;
        onex1.GetComponent<Image>().color = clear;
        onex2.GetComponent<Image>().color = clear;
        onex3.GetComponent<Image>().color = clear;
        onex4.GetComponent<Image>().color = clear;
        onex6.GetComponent<Image>().color = clear;
        onex8.GetComponent<Image>().color = clear;
        twox2.GetComponent<Image>().color = clear;
        twox3.GetComponent<Image>().color = clear;
        twox4.GetComponent<Image>().color = clear;
        twox6.GetComponent<Image>().color = clear;
        twox8.GetComponent<Image>().color = clear;
    }

    public void ShowLegoMenu()
    {
        legoPanel.SetActive(true);
        switch(selectedBlock)
        {
            case(0):
                onex1.GetComponent<Image>().color = purple;
                break;
            case(1):
                onex2.GetComponent<Image>().color = purple;
                break;
            case(2):
                onex3.GetComponent<Image>().color = purple;
                break;
            case(3):
                onex4.GetComponent<Image>().color = purple;
                break;
            case(4):
                onex6.GetComponent<Image>().color = purple;
                break;
            case(5):
                onex8.GetComponent<Image>().color = purple;
                break;
            case(6):
                twox2.GetComponent<Image>().color = purple;
                break;
            case(7):
                twox3.GetComponent<Image>().color = purple;
                break;
            case(8):
                twox4.GetComponent<Image>().color = purple;
                break;
            case(9):
                twox6.GetComponent<Image>().color = purple;
                break;
            case(10):
                twox8.GetComponent<Image>().color = purple;
                break;
        }
    }

    public void ShowHelpMenu()
    {
        menuPanel.SetActive(false);
        helpPanel.SetActive(true);
    }
    
    public void HideHelpMenu()
    {
        helpPanel.SetActive(false);
    }

    public void ShowAvatarMenu()
    {
        Array.Copy(avatar_color_array, temp_color_array, avatar_color_array.Length);
        Array.Copy(avatar_piece_array, temp_piece_array, avatar_piece_array.Length);
        Menu_Minifig_Setup(0);
        Menu_Minifig_Setup(1);
        Menu_Minifig_Setup(2);
        Menu_Minifig_Setup(3);
        Menu_Minifig_Setup(4);
        Menu_Minifig_Setup(5);
        avatarPanel.SetActive(true);
        hatButton.GetComponent<Image>().color = blackLight;
        menuPanel.SetActive(false);
    }
    public void HideAvatarMenu()
    {

        avatarBackButton.GetComponent<Image>().color = blackDark;
        handButton.GetComponent<Image>().color = blackDark;
        faceButton.GetComponent<Image>().color = blackDark;
        backButton.GetComponent<Image>().color = blackDark;
        bodyButton.GetComponent<Image>().color = blackDark;
        legButton.GetComponent<Image>().color = blackDark;
        saveButton.GetComponent<Image>().color = purple2;
        avatarPanel.SetActive(false);
    }

    public void SwapAvatarHorizontal()
    {
        if(hatButton.GetComponent<Image>().color != blackDark)
        {
            hatButton.GetComponent<Image>().color = blackDark;
            faceButton.GetComponent<Image>().color = blackLight;
        }
        else if(backButton.GetComponent<Image>().color != blackDark)
        {
            backButton.GetComponent<Image>().color = blackDark;
            bodyButton.GetComponent<Image>().color = blackLight;
        }
        else if(handButton.GetComponent<Image>().color != blackDark)
        {
            handButton.GetComponent<Image>().color = blackDark;
            legButton.GetComponent<Image>().color = blackLight;
        }
        else if(faceButton.GetComponent<Image>().color != blackDark)
        {
            faceButton.GetComponent<Image>().color = blackDark;
            hatButton.GetComponent<Image>().color = blackLight;
        }
        else if(bodyButton.GetComponent<Image>().color != blackDark)
        {
            bodyButton.GetComponent<Image>().color = blackDark;
            backButton.GetComponent<Image>().color = blackLight;
        }
        else if(legButton.GetComponent<Image>().color != blackDark)
        {
            legButton.GetComponent<Image>().color = blackDark;
            handButton.GetComponent<Image>().color = blackLight;
        }
        else if(saveButton.GetComponent<Image>().color != purple2)
        {
            saveButton.GetComponent<Image>().color = purple2;
            avatarBackButton.GetComponent<Image>().color = blackLight;
        }
        else if(avatarBackButton.GetComponent<Image>().color != blackDark)
        {
            avatarBackButton.GetComponent<Image>().color = blackDark;
            saveButton.GetComponent<Image>().color = blackLight;
        }

    }
    public void SwapAvatarVerticalU()
    {
        if(hatButton.GetComponent<Image>().color != blackDark)
        {
            hatButton.GetComponent<Image>().color = blackDark;
            avatarBackButton.GetComponent<Image>().color = blackLight;
        }
        else if(backButton.GetComponent<Image>().color != blackDark)
        {
            backButton.GetComponent<Image>().color = blackDark;
            hatButton.GetComponent<Image>().color = blackLight;
        }
        else if(handButton.GetComponent<Image>().color != blackDark)
        {
            handButton.GetComponent<Image>().color = blackDark;
            backButton.GetComponent<Image>().color = blackLight;
        }
        else if(faceButton.GetComponent<Image>().color != blackDark)
        {
            faceButton.GetComponent<Image>().color = blackDark;
            saveButton.GetComponent<Image>().color = blackLight;
        }
        else if(bodyButton.GetComponent<Image>().color != blackDark)
        {
            bodyButton.GetComponent<Image>().color = blackDark;
            faceButton.GetComponent<Image>().color = blackLight;
        }
        else if(legButton.GetComponent<Image>().color != blackDark)
        {
            legButton.GetComponent<Image>().color = blackDark;
            bodyButton.GetComponent<Image>().color = blackLight;
        }
        else if(saveButton.GetComponent<Image>().color != purple2)
        {
            saveButton.GetComponent<Image>().color = purple2;
            legButton.GetComponent<Image>().color = blackLight;
        }
        else if(avatarBackButton.GetComponent<Image>().color != blackDark)
        {
            avatarBackButton.GetComponent<Image>().color = blackDark;
            handButton.GetComponent<Image>().color = blackLight;
        }
    }
    public void SwapAvatarVerticalD()
    {
        if(hatButton.GetComponent<Image>().color != blackDark)
        {
            hatButton.GetComponent<Image>().color = blackDark;
            backButton.GetComponent<Image>().color = blackLight;
        }
        else if(backButton.GetComponent<Image>().color != blackDark)
        {
            backButton.GetComponent<Image>().color = blackDark;
            handButton.GetComponent<Image>().color = blackLight;
        }
        else if(handButton.GetComponent<Image>().color != blackDark)
        {
            handButton.GetComponent<Image>().color = blackDark;
            avatarBackButton.GetComponent<Image>().color = blackLight;
        }
        else if(faceButton.GetComponent<Image>().color != blackDark)
        {
            faceButton.GetComponent<Image>().color = blackDark;
            bodyButton.GetComponent<Image>().color = blackLight;
        }
        else if(bodyButton.GetComponent<Image>().color != blackDark)
        {
            bodyButton.GetComponent<Image>().color = blackDark;
            legButton.GetComponent<Image>().color = blackLight;
        }
        else if(legButton.GetComponent<Image>().color != blackDark)
        {
            legButton.GetComponent<Image>().color = blackDark;
            saveButton.GetComponent<Image>().color = blackLight;
        }
        else if(saveButton.GetComponent<Image>().color != purple2)
        {
            saveButton.GetComponent<Image>().color = purple2;
            faceButton.GetComponent<Image>().color = blackLight;
        }
        else if(avatarBackButton.GetComponent<Image>().color != blackDark)
        {
            avatarBackButton.GetComponent<Image>().color = blackDark;
            hatButton.GetComponent<Image>().color = blackLight;
        }
    }
    // 0 = hat 1 = face and skin 2 = back 3 = item 4 = chest 5 = legs
    // 0 = hat 1 = face 2 = back 3 = item
    //hat,face,back,hand,chest,leg,skin
    public void SelectAvatar()
    {
        //spot 0
        if(hatButton.GetComponent<Image>().color != blackDark)
        {
            if(temp_piece_array[0] < 3)
                temp_piece_array[0]++;
            else
                temp_piece_array[0] = 0;
            Menu_Minifig_Setup(0);
        }
        //spot 1
        else if(faceButton.GetComponent<Image>().color != blackDark)
        {
            if(temp_piece_array[1] < 2)
                temp_piece_array[1]++;
            else
                temp_piece_array[1] = 0;
            Menu_Minifig_Setup(1);
        }
        //spot 2
        else if(backButton.GetComponent<Image>().color != blackDark)
        {
            if(temp_piece_array[2] < 3)
                temp_piece_array[2]++;
            else
                temp_piece_array[2] = 0;
            Menu_Minifig_Setup(2);
        }
        //spot 3
        else if(handButton.GetComponent<Image>().color != blackDark)
        {
            if(temp_piece_array[3] < 3)
                temp_piece_array[3]++;
            else
                temp_piece_array[3] = 0;
            Menu_Minifig_Setup(3);
        }
        else if(saveButton.GetComponent<Image>().color != purple2)
        {
            Array.Copy(temp_color_array, avatar_color_array, avatar_color_array.Length);
            Array.Copy(temp_piece_array, avatar_piece_array, avatar_piece_array.Length);
            SwapAvatarModel();
            SaveAvatar();
            HideAvatarMenu();
            ExitMenuMode();
        }
        else if(avatarBackButton.GetComponent<Image>().color != blackDark)
        {
            HideAvatarMenu();
            menuPanel.SetActive(true);
            avatar.GetComponent<Image>().color = blackLight;
        }
    }
    public void SelectAvatarColor()
    {
        //spot 0
        if(hatButton.GetComponent<Image>().color != blackDark)
        {
            if(temp_color_array[0] < avatar_colors.Length - 1)
                temp_color_array[0]++;
            else
                temp_color_array[0] = 0;
            Menu_Minifig_Setup(0);
        }
        //spot 1
        else if(faceButton.GetComponent<Image>().color != blackDark)
        {
             if(temp_color_array[1] < avatar_colors.Length - 1)
                temp_color_array[1]++;
            else
                temp_color_array[1] = 0;
            Menu_Minifig_Setup(1);
        }
        //spot 2
        else if(backButton.GetComponent<Image>().color != blackDark)
        {
             if(temp_color_array[2] < avatar_colors.Length - 1)
                temp_color_array[2]++;
            else
                temp_color_array[2] = 0;
            Menu_Minifig_Setup(2);
        }
        //spot 3
        else if(handButton.GetComponent<Image>().color != blackDark)
        {
             if(temp_color_array[3] < avatar_colors.Length - 1)
                temp_color_array[3]++;
            else
                temp_color_array[3] = 0;
            Menu_Minifig_Setup(3);
        }
        //spot 4
        else if(bodyButton.GetComponent<Image>().color != blackDark)
        {
             if(temp_color_array[4] < avatar_colors.Length - 1)
                temp_color_array[4]++;
            else
                temp_color_array[4] = 0;
            Menu_Minifig_Setup(4);
        }
        //spot 5
        else if(legButton.GetComponent<Image>().color != blackDark)
        {
             if(temp_color_array[5] < avatar_colors.Length - 1)
                temp_color_array[5]++;
            else
                temp_color_array[5] = 0;
            Menu_Minifig_Setup(5);
        }
        
    }
    public void Menu_Minifig_Setup(int piece)
    {
        switch(piece)
        {
            case 0:
                if(hat.transform.childCount > 0)
                {
                    DestroyImmediate(hat.transform.GetChild(0).gameObject);
                }
                if(temp_piece_array[0] < 3)
                {
                    GameObject tempHat = Instantiate (hat_array[temp_piece_array[0]]) as GameObject;
                    tempHat.transform.SetParent(hat.transform, false);
                    tempHat.GetComponent<Renderer>().material = avatar_colors[temp_color_array[0]];
                }
                break;
            case 1:
                face.GetComponent<Renderer>().material = face_array[temp_piece_array[1]];
                skin.GetComponent<Renderer>().material = avatar_colors[temp_color_array[1]];
                break;
            case 2:
                if(back.transform.childCount > 0)
                {
                    DestroyImmediate(back.transform.GetChild(0).gameObject);
                }

                if(temp_piece_array[2] < 3)
                {
                    GameObject tempBack = Instantiate (back_array[temp_piece_array[2]]) as GameObject;
                    tempBack.transform.SetParent(back.transform, false);
                    if(temp_piece_array[2] == 1)
                        tempBack.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = avatar_colors[temp_color_array[2]];
                    else
                        tempBack.GetComponent<Renderer>().material = avatar_colors[temp_color_array[2]];
                }
                break;
            case 3:
                if(hand.transform.childCount > 0)
                {
                    DestroyImmediate(hand.transform.GetChild(0).gameObject);
                }
                if(temp_piece_array[3] < 3)
                {
                    GameObject tempHand = Instantiate (item_array[temp_piece_array[3]]) as GameObject;
                    tempHand.transform.SetParent(hand.transform, false);
                    tempHand.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = avatar_colors[temp_color_array[3]];
                }
                break;
            case 4:
                chest.GetComponent<Renderer>().material = avatar_colors[temp_color_array[4]];
                break;
            case 5:
                leg.GetComponent<Renderer>().material = avatar_colors[temp_color_array[5]];
                break;
        }
    }
    public void SwapAvatarModel()
    {
        
        if(a_hat.transform.childCount > 0)
        {
            DestroyImmediate(a_hat.transform.GetChild(0).gameObject);
        }
        if(avatar_piece_array[0] < 3)
        {
            GameObject tempHat = Instantiate (hat_array[avatar_piece_array[0]]) as GameObject;
            tempHat.transform.SetParent(a_hat.transform, false);
            tempHat.GetComponent<Renderer>().material = avatar_colors[avatar_color_array[0]];
        }
        
    
        a_face.GetComponent<Renderer>().material = face_array[avatar_piece_array[1]];
        a_skin.GetComponent<Renderer>().material = avatar_colors[avatar_color_array[1]];
        
    
        if(a_back.transform.childCount > 0)
        {
            DestroyImmediate(a_back.transform.GetChild(0).gameObject);
        }

        if(avatar_piece_array[2] < 3)
        {
            GameObject tempBack = Instantiate (back_array[avatar_piece_array[2]]) as GameObject;
            tempBack.transform.SetParent(a_back.transform, false);
            if(avatar_piece_array[2] == 1)
                tempBack.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = avatar_colors[avatar_color_array[2]];
            else
                tempBack.GetComponent<Renderer>().material = avatar_colors[avatar_color_array[2]];
        }
        
        if(a_hand.transform.childCount > 0)
        {
            DestroyImmediate(a_hand.transform.GetChild(0).gameObject);
        }
        if(avatar_piece_array[3] < 3)
        {
            GameObject tempHand = Instantiate (item_array[avatar_piece_array[3]]) as GameObject;
            tempHand.transform.SetParent(a_hand.transform, false);
            tempHand.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = avatar_colors[avatar_color_array[3]];
        }
        
        a_chest.GetComponent<Renderer>().material = avatar_colors[avatar_color_array[4]];
        
    
        a_leg.GetComponent<Renderer>().material = avatar_colors[avatar_color_array[5]];
                
    }

    public void ShowKitMenu()
    {
        menuPanel.SetActive(false);
        kitPanel.SetActive(true);
        sphynxButton.GetComponent<Image>().color = blackLight;
    }

    public void SwapKitHorizontal()
    {
        if(sphynxButton.GetComponent<Image>().color != blackDark)
        {
            sphynxButton.GetComponent<Image>().color = blackDark;
            pyramidButton.GetComponent<Image>().color = blackLight;
        }
        else if(pyramidButton.GetComponent<Image>().color != blackDark)
        {
            pyramidButton.GetComponent<Image>().color = blackDark;
            sphynxButton.GetComponent<Image>().color = blackLight;
        }
    }

    public void SwapKitVerticle()
    {
        if(sphynxButton.GetComponent<Image>().color != blackDark)
        {
            sphynxButton.GetComponent<Image>().color = blackDark;
            kitBackButton.GetComponent<Image>().color = blackLight;
        }
        else if(pyramidButton.GetComponent<Image>().color != blackDark)
        {
            pyramidButton.GetComponent<Image>().color = blackDark;
            kitBackButton.GetComponent<Image>().color = blackLight;
        }
        else if(kitBackButton.GetComponent<Image>().color != blackDark)
        {
            kitBackButton.GetComponent<Image>().color = blackDark;
            sphynxButton.GetComponent<Image>().color = blackLight;
        }
    }

    public void SelectKit()
    {
        if(sphynxButton.GetComponent<Image>().color != blackDark)
        {

            LegoController.kit_selection = 0;
            LegoController.bottom_level = 6;
            sphynxButton.GetComponent<Image>().color = blackDark;
            kitPanel.SetActive(false);
            ExitMenuMode();
            LegoController.kit_mode = true;
        }
        else if(pyramidButton.GetComponent<Image>().color != blackDark)
        {
            LegoController.kit_selection = 1;
            LegoController.bottom_level = 3;
            pyramidButton.GetComponent<Image>().color = blackDark;
            kitPanel.SetActive(false);
            ExitMenuMode();
            LegoController.kit_mode = true;
        }
        else if(kitBackButton.GetComponent<Image>().color != blackDark)
        {
            kitBackButton.GetComponent<Image>().color = blackDark;
            kitPanel.SetActive(false);
            ShowMainMenu();
        }
    }

    public void HideKitMenu()
    {
        sphynxButton.GetComponent<Image>().color = blackDark;
        pyramidButton.GetComponent<Image>().color = blackDark;
        kitBackButton.GetComponent<Image>().color = blackDark;
        kitPanel.SetActive(false);
    }

    public void SaveAvatar()
    {
        //color_array
        //piece_array
        for(int i = 0; i < avatar_color_array.Length; i++)
        {
            PlayerPrefs.SetInt("color" + i, avatar_color_array[i]);
        }
        for(int i = 0; i < avatar_piece_array.Length; i++)
        {
            PlayerPrefs.SetInt("piece" + i, avatar_piece_array[i]);
        }
        PlayerPrefs.Save();
    }

    public void LoadAvatar()
    {
        if(PlayerPrefs.HasKey("color0"))
        {
            for(int i = 0; i < avatar_color_array.Length; i++)
            {
                avatar_color_array[i] = PlayerPrefs.GetInt("color" + i);
            }
            for(int i = 0; i < avatar_piece_array.Length; i++)
            {
                avatar_piece_array[i] = PlayerPrefs.GetInt("piece" + i);
            }
        }
        SwapAvatarModel();
    }

    void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }

}
