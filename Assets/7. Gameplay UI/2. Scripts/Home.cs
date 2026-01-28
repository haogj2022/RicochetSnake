using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    [SerializeField] private TMP_Text CurrentLevelText;
    [SerializeField] private Button ContinueButton;
    [SerializeField] private GameObject Snake;

    private void Start()
    {
        ContinueButton.onClick.AddListener(LoadLevel);
        CurrentLevelText.text = GameManager.Instance.GetCurrentLevel().ToString();

        Snake.transform.DOLocalMoveY(-345, 0.5f).SetEase(Ease.InFlash).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        ContinueButton.onClick.RemoveListener(LoadLevel);
        DOTween.KillAll();
    }

    private void LoadLevel()
    {
        LevelLoader.Instance.LoadNextLevel("Gameplay");
    }
}
