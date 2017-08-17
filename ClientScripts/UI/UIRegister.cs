using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class UIRegister : MonoBehaviour 
{
    private GameObject mPanel_Login;
    private RegisterRequest registerRequest;
    private InputField mInput_Username;
    private InputField mInput_Password;
    private Text mText_HintMessage;

	void Start () 
    {
        mPanel_Login = GameObject.Find("Panel_Login");
        mInput_Username = transform.Find("Username").GetComponentInChildren<InputField>();
        mInput_Password = transform.Find("Password").GetComponentInChildren<InputField>();
        registerRequest = GetComponent<RegisterRequest>();
        mText_HintMessage = transform.Find("HintMessage").GetComponent<Text>();

        gameObject.SetActive(false);
	}

    public void OnRegisterButton()
    {
        mText_HintMessage.text = "";
        registerRequest.Username = mInput_Username.text;
        registerRequest.Password = mInput_Password.text;
        registerRequest.OnSendRequest();
    }

    public void OnBackButton()
    {
        gameObject.SetActive(false);
        mPanel_Login.SetActive(true);
        mInput_Username.text = "";
        mInput_Password.text = "";
        mText_HintMessage.text = "";
    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        switch(returnCode)
        {
            case ReturnCode.Success:
                mInput_Username.text = "";
                mInput_Password.text = "";
                mText_HintMessage.text = "账号注册成功，请返回登录";
                break;
            case  ReturnCode.Failed:
                mText_HintMessage.text = "用户名重复，请更改用户名重新注册";
                break;
            case ReturnCode.Exception:
                mText_HintMessage.text = "用户名或密码不能为空";
                break;
        }
    }
}
