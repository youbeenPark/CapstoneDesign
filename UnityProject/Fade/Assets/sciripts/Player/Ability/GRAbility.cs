using UnityEngine;

public class GRAbility : MonoBehaviour
{
    [Header("발판 설정")]
    [Tooltip("발판 프리팹(원본)을 여기에 연결하세요.")]
    public GameObject platformPrefab;

    [Tooltip("발판이 생성될 고정된 위치를 가진 오브젝트를 여기에 연결하세요.")]
    public Transform spawnPointTransform;

    [Header("입력 및 상태")]
    public KeyCode abilityKey = KeyCode.E;

    // 현재 맵에 존재하는 발판을 추적하는 변수 (중복 방지 핵심)
    private GameObject currentPlatform = null;


    void Update()
    {
        if (Input.GetKeyDown(abilityKey))
        {
            // ⭐ 핵심: 현재 발판이 존재하는지 여부만 체크합니다. ⭐
            if (currentPlatform != null)
            {
                Debug.Log("이미 발판이 존재합니다. 새 발판을 생성할 수 없습니다.");
                return;
            }

            // 발판이 존재하지 않으면 즉시 생성 함수 호출
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
        // ⭐ 핵심: currentPlatform을 null로 설정하여 즉시 재사용 가능하게 합니다.
        currentPlatform = null;

        Debug.Log($"발판 파괴 완료. 능력 즉시 사용 가능.");
    }
}