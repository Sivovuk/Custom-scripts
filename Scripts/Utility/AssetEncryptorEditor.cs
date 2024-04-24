using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.Cryptography;

public class ScriptEncryptorEditor : EditorWindow
{
    // The script file you want to encrypt
    public MonoScript originalScript;

    // Output path for the encrypted script
    public string encryptedScriptPath = "Assets/Resources/EncryptedScripts/encryptedScript.txt";

    // Encryption key (replace this with a secure key management system)
    private static readonly string encryptionKey = "1234567890123456";

    [MenuItem("Window/Script Encryptor")]
    public static void ShowWindow()
    {
        // GetWindow<T> creates a window of type T or brings it to the front if it already exists
        EditorWindow.GetWindow(typeof(ScriptEncryptorEditor), false, "Script Encryptor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Script Encryptor", EditorStyles.boldLabel);

        originalScript = EditorGUILayout.ObjectField("Original Script", originalScript, typeof(MonoScript), false) as MonoScript;
        encryptedScriptPath = EditorGUILayout.TextField("Encrypted Script Path", encryptedScriptPath);

        if (GUILayout.Button("Encrypt Script"))
        {
            EncryptScript();
        }
    }

    private void EncryptScript()
    {
        if (originalScript == null)
        {
            Debug.LogError("Please assign the original script to encrypt.");
            return;
        }

        string scriptContent = File.ReadAllText(AssetDatabase.GetAssetPath(originalScript));

        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(encryptionKey);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes;

            // Encrypt the original script content
            byte[] encryptedData = EncryptStringToBytes_Aes(scriptContent, aesAlg.Key, aesAlg.IV);

            // Save the encrypted data to a new file
            File.WriteAllBytes(encryptedScriptPath, encryptedData);

            Debug.Log("Script encrypted and saved at: " + encryptedScriptPath);
        }
    }

    private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] key, byte[] iv)
    {
        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }

                encrypted = msEncrypt.ToArray();
            }
        }

        return encrypted;
    }
}
