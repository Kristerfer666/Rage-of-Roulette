using UnityEngine;
using System.IO;

public class Screenshot : MonoBehaviour
{
    [SerializeField] private UnityEngine.InputSystem.Key _screenshotKey = UnityEngine.InputSystem.Key.F12;

    private string _screenshotFolder;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _screenshotFolder = Path.Combine(Application.dataPath, "../screenshots");
        if (!Directory.Exists(_screenshotFolder))
            Directory.CreateDirectory(_screenshotFolder);
    }

    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current[_screenshotKey].wasPressedThisFrame)
            TakeScreenshot();
    }

    void TakeScreenshot()
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string basePath  = Path.Combine(_screenshotFolder, $"screenshot_{timestamp}");
        string path      = $"{basePath}.png";

        // Avoid collisions if multiple shots happen in the same second
        int i = 1;
        while (File.Exists(path))
            path = $"{basePath}_{i++}.png";

        ScreenCapture.CaptureScreenshot(path);
        Debug.Log($"Screenshot saved: {path}");
    }
}
