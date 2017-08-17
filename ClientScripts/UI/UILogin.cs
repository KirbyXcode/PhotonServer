using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using UnityEngine.SceneManagement;

public class UILogin : MonoBehaviour 
{
    private GameObject mPanel_Register;
    private LoginRequest uiLoginRequest;
    private InputField mInput_Username;
    private InputField mInput_Password;
    private Text mText_HintMessage;

    void Start()
    {
        mPanel_Register = GameObject.Find("Panel_Register");
        uiLoginRequest = GetComponent<LoginRequest>();
        mInput_Username = transform.Find("Username").GetComponentInChildren<InputField>();
        mInput_Password = transform.Find("Password").GetComponentInChildren<InputField>();
        mText_HintMessage = transform.Find("HintMessage").GetComponent<Text>();
    }

    public void OnLoginButton()
    {
        mText_HintMessage.text = "";
        uiLoginRequest.Username = mInput_Username.text;
        uiLoginRequest.Password = mInput_Password.text;
        uiLoginRequest.OnSendRequest();
    }

    public void OnRegisterButton()
    {
        mText_HintMessage.text = "";
        mInput_Username.text = "";
        mInput_Password.text = "";
        gameObject.SetActive(false);
        mPanel_Register.SetActive(true);
    }

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if(returnCode == ReturnCode.Success)
        {
            PhotonEngine.username = uiLoginRequest.Username;
            SceneManager.LoadScene("MainCity");
        }
        else
        {
            mText_HintMessage.text = "账号或密码输入错误，请重新输入";
        }
    }

}
