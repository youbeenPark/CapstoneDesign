using UnityEngine;

public class GRAbility : MonoBehaviour
{
    [Header("발판 설정")]
    [Tooltip("발판 프리팹(원본)을 여기에 연결하세요.")]
    public GameObject platformPrefab;

    [Tooltip("발판이 생성될 고정된 위치를 가진 오브젝트를 여기에 연결하세요.")]
    public Transform spawnPointTransform;

    [Header("입력 및 상태")]
    [Tooltip("발판 생성 능력을 사용할 키를 지정합니다.")]
    public KeyCode abilityKey = KeyCode.E;

    [Tooltip("발판이 파괴된 후 다시 사용할 수 있을 때까지의 시간 (초)")]
    public float cooldownTime = 3f;

    private float nextAvailableTime = 0f;
    private GameObject currentPlatform = null; // 현재 맵에 존재하는 발판 추적


    void Update()
    {
        if (Input.GetKeyDown(abilityKey))
        {
            // 1. 중복 방지 체크
            if (currentPlatform != null)
            {
                Debug.Log("이미 발판이 존재합니다.");
                return;
            }

            // 2. 쿨다운 체크
            if (Time.time < nextAvailableTime)
            {
                float remainingTime = nextAvailableTime - Time.time;
                Debug.Log($"쿨다운 중입니다. {remainingTime:F1}초 후 사용 가능합니다.");
                return;
            }

            // 모든 체크를 통과하면 생성 함수 호출
            InstantiatePlatform();
        }
    }

    void InstantiatePlatform()
    {
        if (platformPrefab == null || spawnPointTransform == null)
        {
            Debug.LogError("오류: 발판 프리팹 또는 생성 위치가 연결되지 않았습니다.");
            return;
        }

        // 1. 발판 생성 및 추적 변수에 저장
        GameObject newPlatform = Instantiate(
            platformPrefab,
            spawnPointTransform.position,
            spawnPointTransform.rotation
        );
        currentPlatform = newPlatform;

        // 2. 발판 스크립트를 가져와 자기 자신(GRAbility 스크립트)을 전달하며 초기화
        GRAbilityPlatform platformScript = newPlatform.GetComponent<GRAbilityPlatform>();
        if (platformScript != null)
        {
            platformScript.Initialize(this);
        }
        else
        {
            Debug.LogError("오류: 발판 프리팹에 'GRAbilityPlatform' 스크립트가 없습니다!");
        }

        Debug.Log("새 발판이 생성되었습니다.");
    }

    // 발판이 파괴될 때, 발판 스크립트에서 이 함수를 호출합니다.
    public void PlatformDestroyed()
    {
        // 1. 중복 방지 상태 해제
        currentPlatform = null;

        // 2. 쿨다운 시작 시간 설정
        nextAvailableTime = Time.time + cooldownTime;

        Debug.Log($"발판 파괴 완료. {cooldownTime}초 쿨다운 시작.");
    }
}