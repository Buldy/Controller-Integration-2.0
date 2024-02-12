using UnityEngine;
using UnityEngine.InputSystem;
using VTS.Core;
using VTS.Unity;

public class ControllerData : UnityVTSPlugin
{
    VTSCustomParameter[] parametry = new VTSCustomParameter[13];
    VTSParameterInjectionValue[] updateParam = new VTSParameterInjectionValue[13];
    string[] nazvy = { "Param16", "Param17", "Param18", "Param21", "Param20", "Param19", "Param22", "Param23", "Param24", "Param26", "Param29", "Param28", "Param27" };
    //string[] nazvy = { "Ukazovacek", "Prostrednicek", "Prstenicek", "Malicek", "Green", "RedB", "Yellow", "Blue", "Orange", "PravaRuka", "GOSwitch", "LevaRuka", "Whammy" };

    float ukazovacek = 0f;
    float prostrednicek = 0f;
    float prstenicek = 0f;
    float malicek = 0f;
    float levaRuka = 0f;
    float fretGO = 0;
    float pravaRuka = 0f;
    float GL = 0f;
    float RL = 0f;
    float YL = 0f;
    float BL = 0f;
    float OL = 0f;
    float whammy = 0f;

    //int timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        
        for (int i = 0; i < 13; i++) {
            parametry[i] = new VTSCustomParameter();
            parametry[i].defaultValue = 0;
            parametry[i].min = 0;
            parametry[i].max = 1;
            parametry[i].parameterName = nazvy[i];
        }
        parametry[11].min = -1;
        Initialize(new WebSocketSharpImpl(this.Logger), new NewtonsoftJsonUtilityImpl(), new TokenStorageImpl(Application.persistentDataPath), () => {
        for (int i = 0; i < 13; i++) {
            AddCustomParameter(parametry[i], (r) => { }, (e) => { Debug.Log(e.data.message); });
            }
            Debug.Log("Connected");
        }, () => { Debug.Log("Disconnected"); }, (error) => { Debug.Log("error"); });
    }

    // Update is called once per frame
    void Update() {
        if (Keyboard.current.sKey.isPressed) {
            Initialize(new WebSocketSharpImpl(this.Logger), new NewtonsoftJsonUtilityImpl(), new TokenStorageImpl(Application.persistentDataPath), () => {
                for (int i = 0; i < 13; i++) {
                    AddCustomParameter(parametry[i], (r) => { }, (e) => { Debug.Log(e.data.message); });
                }
                Debug.Log("Connected");
            }, () => { Debug.Log("Disconnected"); }, (error) => { Debug.Log("error"); });
        }
        
        if (Gamepad.current.aButton.isPressed) {
            pravaRuka = 0f;
            GL = 1f;
            ukazovacek = 1f;
        }
        else {
            GL = 0f;
            ukazovacek = 0f;
            fretGO = 0f;
        }

        if (Gamepad.current.bButton.isPressed) {
            RL = 1f;
            if (pravaRuka == 0f) {
                prostrednicek = 1f;
            }
            else {
                ukazovacek = 1f;
            }

        }
        else {
            RL = 0f;
            if (pravaRuka == 0f) {
                prostrednicek = 0f;
            }
            else {
                ukazovacek = 0f;
            }
        }

        if (Gamepad.current.yButton.isPressed) {
            YL = 1f;
            if (pravaRuka == 0f) {
                prstenicek = 1f;
            }
            else {
                prostrednicek = 1f;
            }

        }
        else {
            YL = 0f;
            if (pravaRuka == 0f) {
                prstenicek = 0f;
            }
            else {
                prostrednicek = 0f;
            }
        }

        if (Gamepad.current.xButton.isPressed) {
            BL = 1f;
            if (pravaRuka == 0f) {
                malicek = 1f;
            }
            else {
                prstenicek = 1f;
            }

        }
        else {
            BL = 0f;
            if (pravaRuka == 0f) {
                malicek = 0f;
            }
            else {
                prstenicek = 0f;
            }
        }

        if ((Gamepad.current.leftShoulder.isPressed) && (Gamepad.current.aButton.isPressed)) {
            pravaRuka = 0f;
            OL = 1f;
            malicek = 1f;
            fretGO = 1f;
        }
        else if (Gamepad.current.leftShoulder.isPressed) {
            pravaRuka = 1f;
            OL = 1f;
            malicek = 1f;
        }
        else if (pravaRuka == 1f) {
            OL = 0f;
            malicek = 0f;
        }
        else if (pravaRuka == 0f) {
            fretGO = 0f;
        }

        if (Gamepad.current.dpad.y.ReadValue() > 0f) {
            levaRuka = 1f;
        }
        else if (Gamepad.current.dpad.y.ReadValue() < 0f) {
            levaRuka = -1f;
        }
        else {
            levaRuka = 0f;
        }

        whammy = (Gamepad.current.rightStick.x.ReadValue() + 1f) / 2f;

        SendData();
        
    }

    void SendData() {
        for (int i = 0; i < 13; i++) {
            updateParam[i] = new VTSParameterInjectionValue();
            updateParam[i].id = nazvy[i];
            updateParam[i].weight = 1f;
        }

        updateParam[0].value = ukazovacek;
        updateParam[1].value = prostrednicek;
        updateParam[2].value = prstenicek;
        updateParam[3].value = malicek;
        updateParam[4].value = GL;
        updateParam[5].value = RL;
        updateParam[6].value = YL;
        updateParam[7].value = BL;
        updateParam[8].value = OL;
        updateParam[9].value = pravaRuka;
        updateParam[10].value = fretGO;
        updateParam[11].value = levaRuka;
        updateParam[12].value = whammy;

        InjectParameterValues(updateParam, (r) => { Debug.Log("Data sent"); }, (e) => { Debug.Log(e.data.message); });
    }
}
