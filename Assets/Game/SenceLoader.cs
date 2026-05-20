using UnityEngine;
using UnityEngine.SceneManagement;

public class SenceLoader : MonoBehaviour
{
    public static string ScenceToLoad;

    public void BackToMenu()
    {
        // Ghi nhớ điểm đến là Menu và gọi ngay cảnh Loading ra chạy slider
        ScenceToLoad = "Menu";

        SceneManager.LoadScene("Loading");
    }

    public void LoadTargetScene(string sceneName)
    {
        // BƯỚC 1: Bắt buộc phải lưu tên màn hình trước
        ScenceToLoad = sceneName;

        // BƯỚC 2: Sau đó mới ra lệnh gọi màn Loading
        SceneManager.LoadScene("Loading");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}