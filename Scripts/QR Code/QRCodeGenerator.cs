using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;


public class QRCodeGenerator : MonoBehaviour
{
    [SerializeField] private RawImage receiver;
    [SerializeField] private TextMeshProUGUI codeText;

    private Texture2D storeCodeTexture;


    void Start()
    {
        storeCodeTexture = new Texture2D(256, 256);
    }

    public void OnClickEncode() 
    {
        EncodeTextToQRCode();
    }

    private void EncodeTextToQRCode()
    {
        if (codeText != null)
        {
            Color32[] convertPixelToTexture = Encode(codeText.text, storeCodeTexture.width, storeCodeTexture.height);
            storeCodeTexture.SetPixels32(convertPixelToTexture);
            storeCodeTexture.Apply();

            receiver.texture = storeCodeTexture;
        }
        else
        {
            Debug.LogError("Invalid text for QR generation!");
            return;
        }
    }

        private Color32[] Encode(string encodeText, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };

        return writer.Write(encodeText);
    }



}
