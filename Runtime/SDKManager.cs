using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SDKManager : MonoBehaviour
{

    AndroidJavaObject unityActivity;
    AndroidJavaObject mSDKHandler;

    string ad_session_token;

    [SerializeField]
    GameObject loginUI;

    [SerializeField]
    GameObject AccountUI;

    [SerializeField]
    GameObject SDKUI;

    [SerializeField]
    GameObject ConfirmUI;

    [SerializeField]
    GameObject PurchaseUI;

    [SerializeField]
    InputField mID;

    [SerializeField]
    InputField mEmailAddress;

    [SerializeField]
    InputField mPassword;

    [SerializeField]
    InputField mConfirmPassword;

    [SerializeField]
    Text mMessageLog;

    [SerializeField]
    Text mBalance;

    [SerializeField]
    string advertisingId;

    [SerializeField]
    string authoizationKey;

    [SerializeField]
    bool ProdMode;

    GameObject canvas;
    AsyncOperation scene;
    GameObject startBtn;
    GameObject shopBtn;
    bool openLogin;
    bool openAccount;
    bool closeUI;
    float height;

    string purchase_item;

    void Awake(){
        mMessageLog.text = "";
        DontDestroyOnLoad( this.gameObject );
        loginUI.SetActive( false );
        AccountUI.SetActive( false );
        SDKUI.SetActive( true );
        openLogin = false;
        openAccount = false;
        closeUI = false;
        height = Screen.height;
    }

    void Start() {

        if (PlayerPrefs.GetInt("isFirstTimeKey", 1) == 1)
        {
            // Set the game object to true (you would replace "YourGameObject" with the name of your game object)
            SDKUI.transform.Find("instruction").gameObject.SetActive(true);

            // Save that the game has been loaded at least once
            PlayerPrefs.SetInt("isFirstTimeKey", 0);
        }

        loginUI.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height * 0.9f);
        loginUI.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -Screen.height );

        SDKUI.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

        GameObject sdk_logo, EmailField, IDField, PasswordField, ConfirmPasswordField, SignupButton, LoginButton, SwitchToSignUpButton, SwitchToLoginButton, MessageField, SignUpDescription, sdk_closeBtn, account_logo, account_close, account_profile, account_credit, account_deletion;

        sdk_logo = loginUI.transform.Find("sdk_logo").gameObject;
        sdk_logo.GetComponent<RectTransform>().sizeDelta = new Vector2( 200.0f, 200.0f );
        sdk_logo.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -Screen.height * 0.1f );

        sdk_closeBtn = loginUI.transform.Find("sdk_close").gameObject;
        sdk_closeBtn.GetComponent<RectTransform>().sizeDelta = new Vector2( 50.0f, 50.0f );
        sdk_closeBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2( Screen.width * 0.8f / 2f * 0.9f, -Screen.height * 0.1f );

        IDField = loginUI.transform.Find("IDField").gameObject;
        IDField.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, 80f);
        IDField.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -Screen.height * 0.2f );
        
        EmailField = loginUI.transform.Find("EmailField").gameObject;
        EmailField.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, 80f);
        EmailField.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -Screen.height * 0.3f );
        
        PasswordField = loginUI.transform.Find("PasswordField").gameObject;
        PasswordField.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, 80f);
        PasswordField.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -Screen.height * 0.4f );
        
        ConfirmPasswordField = loginUI.transform.Find("ConfirmPasswordField").gameObject;
        ConfirmPasswordField.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, 80f);
        ConfirmPasswordField.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -Screen.height * 0.5f );

        MessageField = loginUI.transform.Find("MessageLog").gameObject;
        MessageField.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, 80f);
        MessageField.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -Screen.height * 0.6f );
        
        SignupButton = loginUI.transform.Find("SignupButton").gameObject;
        SignupButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, 80f);
        SignupButton.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -Screen.height * 0.7f );
        
        LoginButton = loginUI.transform.Find("LoginButton").gameObject;
        LoginButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, 80f);
        LoginButton.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -Screen.height * 0.7f );

        SignUpDescription = loginUI.transform.Find("SignUpDescription").gameObject;
        SignUpDescription.GetComponent<RectTransform>().anchoredPosition = new Vector2( Screen.width * 0.8f / 2f - 80.0f - 300f, -Screen.height * 0.8f );
        
        SwitchToSignUpButton = loginUI.transform.Find("SwitchToSignUpButton").gameObject;
        SwitchToSignUpButton.GetComponent<RectTransform>().anchoredPosition = new Vector2( Screen.width * 0.8f / 2f - 80.0f, -Screen.height * 0.8f );

        SwitchToLoginButton = loginUI.transform.Find("SwitchToLoginButton").gameObject;
        SwitchToLoginButton.GetComponent<RectTransform>().anchoredPosition = new Vector2( Screen.width * 0.8f / 2f - 80.0f, -Screen.height * 0.8f );


        AccountUI.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height * 0.9f);
        AccountUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -Screen.height);

        account_logo = AccountUI.transform.Find("account_logo").gameObject;
        account_logo.GetComponent<RectTransform>().sizeDelta = new Vector2( 200.0f, 200.0f );
        account_logo.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -Screen.height * 0.1f );

        account_close = AccountUI.transform.Find("account_close").gameObject;
        account_close.GetComponent<RectTransform>().sizeDelta = new Vector2( 50.0f, 50.0f );
        account_close.GetComponent<RectTransform>().anchoredPosition = new Vector2( Screen.width * 0.8f / 2f * 0.9f, -Screen.height * 0.1f );

        account_profile = AccountUI.transform.Find("profile").gameObject;
        account_profile.GetComponent<RectTransform>().sizeDelta = new Vector2( Screen.height * 0.1f, Screen.height * 0.1f );
        account_profile.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -Screen.height * 0.2f );

        account_credit = AccountUI.transform.Find("credit").gameObject;
        account_credit.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, 300f);
        account_credit.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -Screen.height * 0.4f );
        account_credit.transform.Find("separateLine").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, 4f);
        account_credit.transform.Find("watchAds").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, 100f);

        account_deletion = AccountUI.transform.Find("account_deletion").gameObject;
        account_deletion.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, 150f);
        account_deletion.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -Screen.height * 0.7f );

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

        if (ProdMode){
            object[] unityParameters = new object[1];
            unityParameters[0] = unityActivity;
            mSDKHandler = new AndroidJavaObject( "com.sixuogames.mylibrary.SDKManager", unityParameters );
            var result = mSDKHandler.Call<string>("SetProd");
        }

        #elif UNITY_IPHONE

        if (ProdMode)
            IOSPluginInterface.Set_Prod();
        else
            IOSPluginInterface.Set_Demo();

        #endif

    }

    void Update(){
        if (openLogin){
            height -= Screen.height * 0.08f;
            if ( height <= Screen.height * 0.05f){
                height = Screen.height * 0.05f;
                openLogin = false;
            }
            loginUI.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -height ); 
            if (!openLogin){
                height = Screen.height;
            }
        }

        if (openAccount){
            height -= Screen.height * 0.08f;
            if ( height <= Screen.height * 0.05f){
                height = Screen.height * 0.05f;
                openAccount = false;
            }
            AccountUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -height);
            if (!openAccount){
                height = Screen.height;
            }
        }

        if (closeUI){
            height += Screen.height * 0.08f;
            if ( height >= Screen.height){
                height = Screen.height;
                closeUI = false;
            }
            if (AccountUI.activeSelf)
                AccountUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -height);
            if (loginUI.activeSelf)
                loginUI.GetComponent<RectTransform>().anchoredPosition = new Vector2( 0f, -height ); 
            if (!closeUI){
                loginUI.SetActive(false);
                AccountUI.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0)){
            SDKUI.transform.Find("instruction").gameObject.SetActive(false);
        }

    }

    public void InitializeAd(){
        play_ad();
        Invoke("CloseUI", 0.5f);
    }

    public void CloseUI(){
        closeUI = true;
        height = 0f;
    }

    // Authentication
    public void Login(){

        object[] parameters = new object[2];
        parameters[0] = mEmailAddress.text;
        parameters[1] = mPassword.text;
        
        #if UNITY_EDITOR

        loginUI.SetActive( false );
        SDKUI.SetActive( true );

        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.sixuogames.mylibrary.SDKManager", unityParameters );
        
        if (ProdMode)
            mSDKHandler.Call<string>("SetProd");

        mMessageLog.text = mSDKHandler.Call<string>("login", parameters);

        #elif UNITY_IPHONE

        mMessageLog.text = IOSPluginInterface.LoginAPI( mEmailAddress.text, mPassword.text );

        #endif

        if (mMessageLog.text == "Success"){
            loginUI.SetActive( false );
            SDKUI.SetActive( true );
            check_balance();
        }

    }

    public void Signup(){

        object[] parameters = new object[4];
        parameters[0] = mEmailAddress.text;
        parameters[1] = mID.text;
        parameters[2] = mPassword.text;
        parameters[3] = mConfirmPassword.text;

        #if UNITY_EDITOR

        Debug.Log( parameters[1] );

        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.sixuogames.mylibrary.SDKManager", unityParameters );

        if (ProdMode)
            mSDKHandler.Call<string>("SetProd");

        mMessageLog.text = mSDKHandler.Call<string>("signup", parameters);

        #elif UNITY_IPHONE

        mMessageLog.text = IOSPluginInterface.SignupAPI( mEmailAddress.text, mID.text, mPassword.text, mConfirmPassword.text );


        #endif

    }

    // Campaign

    public void initialize_ad(){

        object[] parameters = new object[1];
        parameters[0] = advertisingId;
        string result = "";

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.sixuogames.mylibrary.SDKManager", unityParameters );

        if (ProdMode)
            mSDKHandler.Call<string>("SetProd");

        result = mSDKHandler.Call<string>("campaign_initialize", parameters);

        #elif UNITY_IPHONE

        string message = IOSPluginInterface.InitializeCampaignAPI( advertisingId );


        #endif

        return;

    }

    public void play_ad(){

        object[] parameters = new object[1];
        parameters[0] = advertisingId;
        string result = "";

        #if UNITY_EDITOR

            StartCoroutine( setAd("https://cdn-icons-png.flaticon.com/512/9148/9148499.png") );

        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.sixuogames.mylibrary.SDKManager", unityParameters );

        if (ProdMode)
            mSDKHandler.Call<string>("SetProd");

        result = mSDKHandler.Call<string>("campaign_play_ad", parameters);

        if (result == "Success"){

            ad_session_token = mSDKHandler.Call<string>("getAd_session_token");

            if (!ProdMode)
                StartCoroutine( setAd("https://api-demo.6uogames.com:8000/campaign/" + ad_session_token + "/get_ad_resources") );
            else
                StartCoroutine( setAd("https://api.6uogames.com:8000/campaign/" + ad_session_token + "/get_ad_resources") );

        }else if (result == "DEMO OK"){
            if (ProdMode)
                StartCoroutine( setAd("https://api.6uogames.com:8000/campaign/preview/get_ad_resources") );
            else
                StartCoroutine( setAd("https://api-demo.6uogames.com:8000/campaign/preview/get_ad_resources") );
        }

        #elif UNITY_IPHONE

        ad_session_token = IOSPluginInterface.PlayCampaignAPI( advertisingId );

        if (ad_session_token != null){

            if (!ProdMode)
                StartCoroutine( setAd("https://api-demo.6uogames.com:8000/campaign/" + ad_session_token + "/get_ad_resources") );
            else
                StartCoroutine( setAd("https://api.6uogames.com:8000/campaign/" + ad_session_token + "/get_ad_resources") );
                
        }else {
            if (ProdMode)
                StartCoroutine( setAd("https://api.6uogames.com:8000/campaign/preview/get_ad_resources") );
            else
                StartCoroutine( setAd("https://api-demo.6uogames.com:8000/campaign/preview/get_ad_resources") );
        }

        #endif

        return;

    }

    public void finish_ad(){

        object[] parameters = new object[2];
        parameters[0] = advertisingId;
        parameters[1] = ad_session_token;
        string result = "";

        #if UNITY_EDITOR
        Debug.Log("Finish Ad is called");
        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.sixuogames.mylibrary.SDKManager", unityParameters );

        if (ProdMode)
            mSDKHandler.Call<string>("SetProd");

        result = mSDKHandler.Call<string>("campaign_finish_ad", parameters);

        #elif UNITY_IPHONE

        result = IOSPluginInterface.FinishCampaignAPI( advertisingId, ad_session_token );

        #endif

        check_balance();

        return;

    }

    IEnumerator setAd(string url){

        using(var www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    var texture = DownloadHandlerTexture.GetContent(www);
                    GameObject panel = new GameObject("Ad Panel");
                    GameObject temp = GameObject.Find("SDKCanvas");
                    panel.transform.SetParent( temp.transform );
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    Image i = panel.AddComponent<Image>();
                    i.sprite = sprite;
                    RectTransform rt = panel.GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2( 0, 0 );
                    rt.sizeDelta = new Vector2( Screen.width, Screen.height );

                    yield return new WaitForSeconds(5);

                    GameObject cross = new GameObject("Close Ad");
                    cross.transform.SetParent( temp.transform );
                    Image ii = cross.AddComponent<Image>();
                    Button btn = cross.AddComponent<Button>();
                    ii.sprite = Resources.Load<Sprite>("cross");
                    RectTransform crossRt = cross.GetComponent<RectTransform>();
                    crossRt.anchoredPosition = new Vector2( Screen.width * 0.8f / 2f * 0.9f, Screen.height / 2 * 0.9f );
                    btn.onClick.AddListener ( delegate(){
                        Invoke("finish_ad", 0.0f);
                        Destroy( panel );
                        Destroy( cross );
                    } );
                }
            }
        }

    }

    public void Purchcase_pending(string item){

        string result = "";

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        object[] parameters = new object[2];
        parameters[0] = item;
        parameters[1] = authoizationKey;
        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.sixuogames.mylibrary.SDKManager", unityParameters );

        if (ProdMode)
            mSDKHandler.Call<string>("SetProd");

        result = mSDKHandler.Call<string>("purchase_pending", parameters);

        #elif UNITY_IPHONE

        result = IOSPluginInterface.PurchasePending( item, authoizationKey );

        #endif

        if ( result == "DEMO OK" ){
            // DEMO

        }else if ( result == "Success" ){
            // Success

        }

        return;

    }

    public void Purchcase_confirm(){

        string result = "";

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        object[] parameters = new object[1];
        parameters[0] = authoizationKey;
        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.sixuogames.mylibrary.SDKManager", unityParameters );

        if (ProdMode)
            mSDKHandler.Call<string>("SetProd");

        result = mSDKHandler.Call<string>("purchase_confirm", parameters);

        #elif UNITY_IPHONE

        result = IOSPluginInterface.PurchaseConfirm( authoizationKey );

        #endif

        return;

    }

    public string check_balance(){

        string balance = "";

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.sixuogames.mylibrary.SDKManager", unityParameters );

        if (ProdMode)
            mSDKHandler.Call<string>("SetProd");

        string result = mSDKHandler.Call<string>("updateBalance");
        balance = mSDKHandler.Call<string>("getBalance");

        #elif UNITY_IPHONE

        balance = IOSPluginInterface.check_balance(  );

        #endif

        if (balance != ""){
            float balanceFloat = float.Parse( balance ) / 1000000000.0f;
            mBalance.text = balanceFloat.ToString();
        }

        return balance;

    }

    public void HideStartBtn(){
        startBtn = GameObject.Find("StartButton");
        startBtn.SetActive(false);
    }

    public void ShowStartBtn(){
        startBtn.SetActive(true);
    }

    public void HideShopBtn(){
        shopBtn = GameObject.Find("Shop");
        shopBtn.SetActive(false);
    }

    public void Protected(){

        string result = "";

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.sixuogames.mylibrary.SDKManager", unityParameters );

        if (ProdMode)
            mSDKHandler.Call<string>("SetProd");

        result = mSDKHandler.Call<string>("protect");

        #elif UNITY_IPHONE

        result = IOSPluginInterface.checkProtected(  );

        #endif

        if (result != "Success"){
            loginUI.SetActive( true );
            openLogin = true;
        }else{
            AccountUI.SetActive( true );
            openAccount = true;
            check_balance();
            initialize_ad();
        }
    }

    public void LinktoDeletion(){
        Application.OpenURL("https://6uogames.com/account-removal");
    }

    public void LinktoProfile(){
        Application.OpenURL("https://6uogames.com/profile");
    }

    public void LinkToAppStore(){
        #if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.uoapp");
        #elif UNITY_IPHONE
        Application.OpenURL("");
        #endif
    }

    public void HideLogin(){
        loginUI.SetActive(false);
        AccountUI.SetActive(false);
        ConfirmUI.SetActive(false);
        PurchaseUI.SetActive(false);
    }
    
}
