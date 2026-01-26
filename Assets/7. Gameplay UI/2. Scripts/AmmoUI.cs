using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private GameObject AmmoImagePrefab;
    [SerializeField] private GameObject AmmoContainer;
    private int AmmoCount;
    private List<Image> AmmoImageList = new();

    private void Start()
    {
        GameManager.Instance.OnSnakeShot += OnSnakeShot;
        AmmoCount = GameManager.Instance.GetAmmoCount();

        for (int i = 0; i < AmmoCount; i++)
        {
            Image newAmmoImage = Instantiate(AmmoImagePrefab, AmmoContainer.transform).GetComponent<Image>();
            AmmoImageList.Add(newAmmoImage);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnSnakeShot -= OnSnakeShot;
    }

    private void OnSnakeShot()
    {
        AmmoCount = GameManager.Instance.GetAmmoCount();
        DecreaseAmmoCount();
    }

    private void DecreaseAmmoCount()
    {
        if (AmmoImageList.Count > 0)
        {
            AmmoImageList[0].gameObject.SetActive(false);
            AmmoImageList.RemoveAt(0);
        }
    }
}
