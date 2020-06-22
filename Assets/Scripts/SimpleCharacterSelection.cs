using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ModuloKart.CustomVehiclePhysics;
using ModuloKart.HUD;
using ModuloKart.CountDown;

public enum AVerySimpleEnumOfCharacters
{
    Toby = 1,
    Felix = 2,
    Paul = 3,
    Maxine = 4,
    NotInGame = 0
}

public class SimpleCharacterSelection : MonoBehaviour
{
    public int PlayerID;
    ui_controller ui;
    public bool isCharacterSelected;
    public AVerySimpleEnumOfCharacters whichCharacterDidISelectDuringTheGameScene = AVerySimpleEnumOfCharacters.Toby;
    public Sprite[] CharacterSprites;

    public GameObject SimpleCharacterSelectionPanel;
    public Image selectedCharacterImage;
    public Image ToggleImage_PrevCharacter;
    public Image ToggleImage_NextCharacter;
    public Text currentCharacterSelectionText;
    public Text CharacterDisplayText;

    private VehicleBehavior vehicleBehavior;

    public void BeginCharacterSelection(VehicleBehavior v)
    {
        vehicleBehavior = v;
        SetCharacterSprite();
    }

    private void InitCharacterSeleection()
    {
        foreach (VehicleBehavior v in FindObjectsOfType<VehicleBehavior>())
        {
            if (v.PlayerID == PlayerID)
            {
                vehicleBehavior = v;
                BeginCharacterSelection(vehicleBehavior);
            }
        }
    }

    private void ToggleCharacterSelection()
    {
        if (vehicleBehavior.isControllerInitialized && !isCharacterSelected)
        {
            if (Input.GetButtonDown(vehicleBehavior.input_ItemNext))
            {
                AudioManager.instance.Play("Move_Through_Menu_Panels");
                CycleCharacters(isNext: true);
            }
            else if (Input.GetButtonDown(vehicleBehavior.input_ItemPrev))
            {
                AudioManager.instance.Play("Move_Through_Menu_Panels");
                CycleCharacters(isNext: false);
            }
        }
    }

    private void CycleCharacters(bool isNext)
    {
        if (isNext)
        {
            if (whichCharacterDidISelectDuringTheGameScene < (AVerySimpleEnumOfCharacters)4)
            {
                whichCharacterDidISelectDuringTheGameScene++;
            }
            else
                whichCharacterDidISelectDuringTheGameScene = (AVerySimpleEnumOfCharacters)1;
        }
        else
        {
            if (whichCharacterDidISelectDuringTheGameScene > (AVerySimpleEnumOfCharacters)1)
                whichCharacterDidISelectDuringTheGameScene--;
            else
                whichCharacterDidISelectDuringTheGameScene = (AVerySimpleEnumOfCharacters)4;
        }
        UpdatePlayerSelectionHUD(isNext);
    }

    private void UpdatePlayerSelectionHUD(bool isNext)
    {
        SetCharacterSprite();

        ToggleImageAnimateCO = ToggleImageAnimate(isNext);
        StartCoroutine(ToggleImageAnimateCO);
    }

    private void SetCharacterSprite()
    {
        Debug.Log((int)whichCharacterDidISelectDuringTheGameScene - 1);
        selectedCharacterImage.sprite = CharacterSprites[(int)whichCharacterDidISelectDuringTheGameScene - 1];
        switch (whichCharacterDidISelectDuringTheGameScene)
        {
            case AVerySimpleEnumOfCharacters.Felix:
                currentCharacterSelectionText.text = whichCharacterDidISelectDuringTheGameScene.ToString() + " has a much larger SpeedNip bar";
                break;

            case AVerySimpleEnumOfCharacters.Toby:
                currentCharacterSelectionText.text = whichCharacterDidISelectDuringTheGameScene.ToString() + " has one extra part and a slightly bigger SpeedNip Bar";
                break;

            case AVerySimpleEnumOfCharacters.Maxine:
                currentCharacterSelectionText.text = whichCharacterDidISelectDuringTheGameScene.ToString() + " has two extra parts";
                break;

            case AVerySimpleEnumOfCharacters.Paul:
                currentCharacterSelectionText.text = whichCharacterDidISelectDuringTheGameScene.ToString() + " has a shield to protect agaisnt a hit";
                break;
        }
    }

    private void ConfirmSelection()
    {
        if (!isCharacterSelected)
        {
            if (Input.GetButtonDown(vehicleBehavior.input_nitros))
            {
                AudioManager.instance.Play("Confirm_Option");
                CharacterDisplayText.text += whichCharacterDidISelectDuringTheGameScene.ToString();
                SimpleCharacterSelectionPanel.SetActive(false);
                isCharacterSelected = true;
                gameObject.GetComponentInChildren<ui_controller>().Initialize_Character(whichCharacterDidISelectDuringTheGameScene);
                GameManager.Instance.ReadyUp();

                if (whichCharacterDidISelectDuringTheGameScene == AVerySimpleEnumOfCharacters.Toby)
                {
                    vehicleBehavior.extra_nitros_meter_float = 25f;
                    Debug.Log("nitro1" + vehicleBehavior.extra_nitros_meter_float);
                    AudioManager.instance.Play("Tobias_Character_Selected");
                }
                if (whichCharacterDidISelectDuringTheGameScene == AVerySimpleEnumOfCharacters.Felix)
                {
                    vehicleBehavior.extra_nitros_meter_float = 50f;
                    Debug.Log("nitro2" + vehicleBehavior.extra_nitros_meter_float);
                    AudioManager.instance.Play("Felix_Character_Selected");
                }
                if (whichCharacterDidISelectDuringTheGameScene == AVerySimpleEnumOfCharacters.Paul)
                {
                    vehicleBehavior.extra_nitros_meter_float = 0f;
                    Debug.Log("nitro3" + vehicleBehavior.extra_nitros_meter_float);
                    AudioManager.instance.Play("Pauline_Character_Selected");
                }
                if (whichCharacterDidISelectDuringTheGameScene == AVerySimpleEnumOfCharacters.Maxine)
                {
                    vehicleBehavior.extra_nitros_meter_float = 0f;
                    Debug.Log("nitro3" + vehicleBehavior.extra_nitros_meter_float);
                    AudioManager.instance.Play("Max_Character_Selected");
                }
            }
        }
    }
    

    private IEnumerator ToggleImageAnimateCO;
    private IEnumerator ToggleImageAnimate(bool isNext)
    {
        if (isNext)
            ToggleImage_NextCharacter.color = Color.green;
        else
            ToggleImage_PrevCharacter.color = Color.green;
        yield return new WaitForSeconds(.1f);
        if (isNext)
            ToggleImage_NextCharacter.color = Color.grey;
        else
            ToggleImage_PrevCharacter.color = Color.grey;
    }

    private void Awake()
    {
        ui = GameObject.FindObjectOfType<ui_controller>();
        InitCharacterSeleection();
    }

    private void Update()
    {
        ToggleCharacterSelection();
        ConfirmSelection();
    }
}
