
//using UnityEngine;
//using UnityEngine.UI;

//public class BgmVolumeController : MonoBehaviour
//{
//    [Header("UI")]
//    public Slider bgmSlider;
//    public Button speakerButton;
//    public Image speakerIcon;

//    [Header("Speaker Icons")]
//    public Sprite iconMute;      // 볼륨 0
//    public Sprite iconLow;       // 볼륨 0.01~0.3
//    public Sprite iconMid;       // 0.31~0.7
//    public Sprite iconHigh;      // 0.71~1.0

//    [Header("Audio")]
//    public AudioSource bgmSource;

//    private float lastVolume = 0.7f;
//    private bool isMuted = false;

//    private void Start()
//    {
//        // 저장된 값 불러오기
//        float savedVolume = PlayerPrefs.GetFloat("Volume_BGM", 0.7f);

//        bgmSlider.value = savedVolume;
//        bgmSource.volume = savedVolume;

//        // 볼륨에 따라 스피커 이미지 업데이트
//        UpdateSpeakerIcon(savedVolume);

//        // 슬라이더 이벤트
//        bgmSlider.onValueChanged.AddListener(OnBgmChanged);

//        // 스피커 버튼 이벤트
//        speakerButton.onClick.AddListener(OnSpeakerPressed);
//    }

//    // 볼륨 슬라이더 조정
//    private void OnBgmChanged(float value)
//    {
//        if (isMuted == false)
//            bgmSource.volume = value;

//        PlayerPrefs.SetFloat("Volume_BGM", value);
//        UpdateSpeakerIcon(value);
//    }

//    // 스피커 버튼 눌렀을 때
//    private void OnSpeakerPressed()
//    {
//        if (isMuted == false)
//        {
//            // 현재 볼륨 저장 후 음소거
//            lastVolume = bgmSlider.value;
//            bgmSource.volume = 0;
//            bgmSlider.value = 0;
//            isMuted = true;
//            UpdateSpeakerIcon(0);
//        }
//        else
//        {
//            // 원래 볼륨으로 복귀
//            bgmSource.volume = lastVolume;
//            bgmSlider.value = lastVolume;
//            isMuted = false;
//            UpdateSpeakerIcon(lastVolume);
//        }
//    }

//    // 볼륨 크기에 따라 아이콘 바뀜
//    private void UpdateSpeakerIcon(float volume)
//    {
//        if (volume <= 0.01f)
//        {
//            speakerIcon.sprite = iconMute;
//        }
//        else if (volume <= 0.3f)
//        {
//            speakerIcon.sprite = iconLow;
//        }
//        else if (volume <= 0.7f)
//        {
//            speakerIcon.sprite = iconMid;
//        }
//        else
//        {
//            speakerIcon.sprite = iconHigh;
//        }
//    }
//}

using UnityEngine;
using UnityEngine.UI;

public class BgmVolumeController : MonoBehaviour
{
    [Header("UI")]
    public Slider bgmSlider;
    public Button speakerButton;
    public Image speakerIcon;

    [Header("Speaker Icons")]
    public Sprite iconMute;
    public Sprite iconLow;
    public Sprite iconMid;
    public Sprite iconHigh;

    private AudioSource bgmSource;
    private float lastVolume = 0.7f;
    private bool isMuted = false;

    private void OnEnable()
    {
        // 현재 씬의 BGM 오브젝트 자동 찾기
        GameObject bgmObj = GameObject.FindWithTag("BGM");
        if (bgmObj != null)
        {
            bgmSource = bgmObj.GetComponent<AudioSource>();
        }

        // 저장된 볼륨 load
        float savedVolume = PlayerPrefs.GetFloat("Volume_BGM", 0.7f);

        bgmSlider.value = savedVolume;
        if (bgmSource != null)
            bgmSource.volume = savedVolume;

        UpdateSpeakerIcon(savedVolume);

        // 이벤트 재등록(중복 방지 위해 Clear → Add)
        bgmSlider.onValueChanged.RemoveAllListeners();
        bgmSlider.onValueChanged.AddListener(OnBgmChanged);

        speakerButton.onClick.RemoveAllListeners();
        speakerButton.onClick.AddListener(OnSpeakerPressed);
    }

    private void OnBgmChanged(float value)
    {
        if (!isMuted && bgmSource != null)
            bgmSource.volume = value;

        PlayerPrefs.SetFloat("Volume_BGM", value);
        UpdateSpeakerIcon(value);
    }

    private void OnSpeakerPressed()
    {
        if (!isMuted)
        {
            lastVolume = bgmSlider.value;
            bgmSlider.value = 0;
            if (bgmSource != null)
                bgmSource.volume = 0;
            isMuted = true;
            UpdateSpeakerIcon(0);
        }
        else
        {
            bgmSlider.value = lastVolume;
            if (bgmSource != null)
                bgmSource.volume = lastVolume;
            isMuted = false;
            UpdateSpeakerIcon(lastVolume);
        }
    }

    private void UpdateSpeakerIcon(float volume)
    {
        if (volume <= 0.01f)
            speakerIcon.sprite = iconMute;
        else if (volume <= 0.3f)
            speakerIcon.sprite = iconLow;
        else if (volume <= 0.7f)
            speakerIcon.sprite = iconMid;
        else
            speakerIcon.sprite = iconHigh;
    }
}

