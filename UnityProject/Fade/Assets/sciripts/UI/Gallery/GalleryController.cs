using UnityEngine;
using TMPro;

public class GalleryController : MonoBehaviour
{
    public TMP_Text currentIslandText;

    private void Start()
    {
        currentIslandText.text = "현재 섬 : " + ConvertIslandName(IslandInfo.currentIsland);
    }

    private string ConvertIslandName(IslandType type)
    {
        switch (type)
        {
            case IslandType.TUTO: return "잊혀진 기억의 땅";
            case IslandType.GR: return "포근한 새싹의 들판";
            case IslandType.YL: return "햇살 같은 동심의 마을";
            case IslandType.BL: return "요동치는 불안의 심연";
            case IslandType.OR: return "새로운 도전의 언덕";
            case IslandType.RD: return "따스한 온기의 울타리";
            case IslandType.SK: return "잃어버린 빛의 하늘";
            case IslandType.PR: return "쓸쓸한 그림자의 성";
            case IslandType.BOSE: return "흩어진 기억의 파편";
            case IslandType.RAINBOW: return "색의 정원";
            default: return "???";
        }
    }
}
