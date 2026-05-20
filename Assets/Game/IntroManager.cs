using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // Thêm thư viện này nếu Intro của bạn dùng Video Player

public class IntroManager : MonoBehaviour
{
    [Header("CẤU HÌNH CHUYỂN CẢNH")]
    public string nextSceneName = "Select"; // Tên cảnh tiếp theo (Ví dụ: Select hoặc Menu)

    [Header("NẾU CÓ DÙNG VIDEO INTRO")]
    public VideoPlayer introVideo; // Kéo Video Player vào đây (nếu có)

    [Header("NẾU DÙNG THỜI GIAN ĐẾM NGƯỢC (KHÔNG CÓ VIDEO)")]
    public float introDuration = 5f; // Số giây hiển thị Intro rồi tự chuyển màn
    public bool allowSkip = true; // Cho phép bấm để bỏ qua Intro nhanh hay không

    private float timer = 0f;
    private bool isTransitioning = false;

    void Start()
    {
        // Nếu bạn có kéo Video vào, code sẽ tự động bắt sự kiện khi video chạy hết bài
        if (introVideo != null)
        {
            introVideo.loopPointReached += OnVideoFinished;
        }
    }

    void Update()
    {
        if (isTransitioning) return;

        // Trường hợp 1: Nếu không dùng Video, chạy đếm ngược bằng thời gian
        if (introVideo == null)
        {
            timer += Time.deltaTime;
            if (timer >= introDuration)
            {
                ChuyenDenCanhTiepTheo();
            }
        }

        // Trường hợp 2: Người chơi bấm phím bất kỳ hoặc click chuột để Skip
        if (allowSkip && Input.anyKeyDown)
        {
            ChuyenDenCanhTiepTheo();
        }
    }

    // Hàm tự động kích hoạt khi Video chạy đến giây cuối cùng
    void OnVideoFinished(VideoPlayer vp)
    {
        ChuyenDenCanhTiepTheo();
    }

    // Hàm thực hiện ép buộc đẩy qua màn Loading để chạy thanh Slider
    public void ChuyenDenCanhTiepTheo()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        // Bước 1: Nạp tên màn tiếp theo vào túi nhớ của SenceLoader
        SenceLoader.ScenceToLoad = nextSceneName;

        // Bước 2: Ép buộc mở ngay màn hình Loading ra chạy slider mượt mà từ trái sang
        SceneManager.LoadScene("Loading");
    }
}