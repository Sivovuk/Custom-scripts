using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodeGenerator : MonoBehaviour
{
    private string characters = "0123456789abcdefghijklmnopqrstuvwxABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [SerializeField] private QRCodeGenerator qrCodeGenerator;
    [SerializeField] private TextMeshProUGUI codeText;

    public void OnCallGenerateCode() 
    {
        CodeGenerate();
    }

    private void  CodeGenerate()
    {
        string code = "";

        for (int i = 0; i < 20; i++) {
            int a = Random.Range(0, characters.Length);
            code = code + characters[a];
        }

        codeText.text = code;
        qrCodeGenerator.OnClickEncode();

        Debug.Log(code);
    }
}
