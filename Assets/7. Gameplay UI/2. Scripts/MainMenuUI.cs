using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text TotalGoldText;

    private void Start()
    {
        TotalGoldText.text = GameManager.Instance.GetTotalGold().ToString();
    }
}
