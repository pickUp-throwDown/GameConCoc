using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public string sceneToLoad;
    public Slider progressBar;
    void Start()
    {
        sceneToLoad = SenceLoader.ScenceToLoad;
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            sceneToLoad = "Menu";
        }
        // Đảm bảo thanh slider bắt đầu từ bên trái cùng (bằng 0)
        if (progressBar != null)
        {
            progressBar.value = 0f;
        }
        StartCoroutine(LoadAsyncOperation());
    }
    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(sceneToLoad);
        if (gameLevel == null)
        {
            SceneManager.LoadScene("Menu");
            yield break;
        }
        gameLevel.allowSceneActivation = false;
        float progressTarget = 0f;
        while (!gameLevel.isDone)
        {
            // Tiến trình tải thực tế của Unity (tối đa đạt 0.9f)
            progressTarget = Mathf.Clamp01(gameLevel.progress / 0.9f);
            // Ép Slider tăng tiến trình mượt mà từ trái sang phải thay vì nhảy lập tức
            if (progressBar != null)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, progressTarget, Time.deltaTime * 1.5f);
            }
            // Chỉ kích hoạt mở màn chơi mới khi cả Unity đã tải xong VÀ Slider đã chạy đầy hẳn về bên phải
            if (gameLevel.progress >= 0.9f && Mathf.Approximately(progressBar.value, 1f))
            {
                // Giữ lại khoảng 0.3 giây cuối cho hiệu ứng mượt mắt rồi mới chuyển hẳn
                yield return new WaitForSeconds(0.3f);
                gameLevel.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}