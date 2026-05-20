using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// 1. KHUÔN CHỨA DỮ LIỆU CỦA TỪNG BẢN LƯU
[System.Serializable]
public class GameSaveData
{
    public string currentSceneName;
    public int totalDeaths;
}

// 2. FILE GỐC ĐỂ GHI NHỚ SỐ LƯỢNG BẢN LƯU
[System.Serializable]
public class MasterSlotConfig
{
    public int totalSlotsCreated = 0;
}

// 3. TRÌNH QUẢN LÝ LƯU GAME VÀ GIAO DIỆN
public class SaveManager : MonoBehaviour
{
    [Header("THIẾT LẬP NÚT TỰ ĐỘNG")]
    public GameObject buttonPrefab;
    public Transform buttonContainer;

    [Header("GIAO DIỆN UI")]
    public GameObject panelThongBao;
    public TextMeshProUGUI textThongBao;
    public GameObject panelChonMan;

    private string masterFilePath;
    private string saveFilePrefix;

    void Awake()
    {
        masterFilePath = Application.persistentDataPath + "/MasterSlotConfig.txt";
        saveFilePrefix = Application.persistentDataPath + "/TrapSave_Slot_";
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Select")
        {
            HienThiDanhSachBanLuu();
        }
    }

    // ==========================================
    // PHẦN 1: QUẢN LÝ GIAO DIỆN (NÚT BẤM UI)
    // ==========================================

    // Hàm này gắn vào nút "TIẾP TỤC" ở bảng chọn ngoài cùng
    public void LoadAndContinueGame()
    {
        MasterSlotConfig masterData = DocMasterConfig();

        if (masterData.totalSlotsCreated > 0)
        {
            // CÓ BẢN LƯU: Bật bảng chọn màn (các nút sẽ tự động sinh ra trong này)
            if (panelChonMan != null) panelChonMan.SetActive(true);
        }
        else
        {
            // KHÔNG CÓ BẢN LƯU: Bật bảng thông báo
            if (panelThongBao != null)
            {
                panelThongBao.SetActive(true);
                if (textThongBao != null)
                {
                    textThongBao.text = "Hiện chưa có bản sao lưu nào.\nHãy bấm TẠO MỚI!!!!!!!";
                }
            }
        }
    }

    // Hàm này gắn vào nút "ĐÓNG" trên bảng thông báo
    public void DongThongBao()
    {
        if (panelThongBao != null)
        {
            panelThongBao.SetActive(false);
        }
    }

    // Hàm này gắn vào nút "ĐÓNG" trên bảng chọn màn (Nếu bạn có làm nút Đóng ở bảng đó)
    public void DongBangChonMan()
    {
        if (panelChonMan != null)
        {
            panelChonMan.SetActive(false);
        }
    }


    // ==========================================
    // PHẦN 2: XỬ LÝ LƯU VÀ SINH NÚT TỰ ĐỘNG
    // ==========================================

    public void TaoBanLuuMoiTrongKhiChoi(string sceneName, int deaths)
    {
        MasterSlotConfig masterData = DocMasterConfig();
        masterData.totalSlotsCreated++;
        GhiMasterConfig(masterData);

        GameSaveData newSave = new GameSaveData();
        newSave.currentSceneName = sceneName;
        newSave.totalDeaths = deaths;

        string jsonText = JsonUtility.ToJson(newSave);
        string slotPath = saveFilePrefix + masterData.totalSlotsCreated + ".txt";
        File.WriteAllText(slotPath, jsonText);

        Debug.Log("Đã tạo bản lưu tùy biến mới tại: " + slotPath);
    }

    public void HienThiDanhSachBanLuu()
    {
        XoaCacNutCuChuaChay();
        MasterSlotConfig masterData = DocMasterConfig();

        for (int i = 1; i <= masterData.totalSlotsCreated; i++)
        {
            int slotNumber = i;
            string slotPath = saveFilePrefix + slotNumber + ".txt";

            if (File.Exists(slotPath))
            {
                string jsonText = File.ReadAllText(slotPath);
                GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(jsonText);

                GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
                TMP_Text btnText = newButton.GetComponentInChildren<TMP_Text>();
                if (btnText != null)
                {
                    btnText.text = "Bản Lưu " + slotNumber + " - Màn: " + saveData.currentSceneName;
                }

                UnityEngine.UI.Button btnComponent = newButton.GetComponent<UnityEngine.UI.Button>();
                if (btnComponent != null)
                {
                    btnComponent.onClick.AddListener(() => LoadSlotGame(slotPath));
                }
            }
        }
    }

    void LoadSlotGame(string path)
    {
        if (File.Exists(path))
        {
            string jsonText = File.ReadAllText(path);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(jsonText);

            SenceLoader.ScenceToLoad = data.currentSceneName;
            SceneManager.LoadScene("Loading");
        }
    }

    void XoaCacNutCuChuaChay()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
    }

    MasterSlotConfig DocMasterConfig()
    {
        if (File.Exists(masterFilePath))
        {
            string json = File.ReadAllText(masterFilePath);
            return JsonUtility.FromJson<MasterSlotConfig>(json);
        }
        return new MasterSlotConfig();
    }

    void GhiMasterConfig(MasterSlotConfig data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(masterFilePath, json);
    }
}