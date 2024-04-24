using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;

public class QRScannerController : MonoBehaviour
{
    [SerializeField] private RawImage bgRawImage;
    [SerializeField] private AspectRatioFitter aspectRatioFitter;
    [SerializeField] private TextMeshProUGUI textOut;
    [SerializeField] private RectTransform scanZone;

    private bool isCameraAvaible = false;
    private WebCamTexture cameraTexture;

    private void Update()
    {
        UpdateCameraRenderr();
    }

    public void OnClickScan()
    {
        Scan();
    }

    private void UpdateCameraRenderr()
    {
        if (!isCameraAvaible)
        {
            return;
        }

        float ratio = (float)cameraTexture.width / (float)cameraTexture.height;

        aspectRatioFitter.aspectRatio = ratio;

        int rotation = -cameraTexture.videoRotationAngle;

        bgRawImage.rectTransform.localEulerAngles = new Vector3(0, 0, rotation);
    }

    private void SetupCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            isCameraAvaible = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                cameraTexture = new WebCamTexture(devices[i].name, (int)scanZone.rect.width, (int)scanZone.rect.height);
            }
        }

        cameraTexture.Play();
        bgRawImage.texture = cameraTexture;
        isCameraAvaible = true;
    }

    private void Scan()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(cameraTexture.GetPixels32(), cameraTexture.width, cameraTexture.height);

            if (result != null)
            {
                textOut.text = result.Text;
            }
            else
            {
                Debug.LogError("Failed to read the QR code");
                textOut.text = "Failed to read the QR code";
            }
        }
        catch
        {
            Debug.LogError("Failed to TRY read the QR code");
            textOut.text = "Failed to TRY read the QR code";
        }
    }

}
