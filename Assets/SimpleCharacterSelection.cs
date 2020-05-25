using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ModuloKart.CustomVehiclePhysics;
using ModuloKart.HUD;
using ModuloKart.CountDown;

public enum AVerySimpleEnumOfCharacters
{
    First_The_Worst = 1,
    Second_The_Best = 2,
    Third_The_One_With_The_Hairy_Chest = 3,
    Maxine = 4,
}

public class SimpleCharacterSelection : MonoBehaviour
{
    public int PlayerID;
    public bool isCharacterSelected;
    public AVerySimpleEnumOfCharacters whichCharacterDidISelectDuringTheGameScene = AVerySimpleEnumOfCharacters.First_The_Worst;
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
        if (vehicleBehavior.isControllerInitialized)
        {
            if (Input.GetButtonDown(vehicleBehavior.input_ItemNext))
            {
                CycleCharacters(isNext: true);
            }
            else if (Input.GetButtonDown(vehicleBehavior.input_ItemPrev))
            {
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
        currentCharacterSelectionText.text = whichCharacterDidISelectDuringTheGameScene.ToString();
    }

    private void ConfirmSelection()
    {
        if (!isCharacterSelected)
        {
            if (Input.GetButtonDown(vehicleBehavior.input_nitros))
            {
                CharacterDisplayText.text += whichCharacterDidISelectDuringTheGameScene.ToString();
                SimpleCharacterSelectionPanel.SetActive(false);
                isCharacterSelected = true;
                GameManager.Instance.ReadyUp();
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
        InitCharacterSeleection();
    }

    private void Update()
    {
        ToggleCharacterSelection();
        ConfirmSelection();
    }
}
