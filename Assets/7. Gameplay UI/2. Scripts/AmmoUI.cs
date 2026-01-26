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
        GameManager.Instance.OnAmmoPurchased += OnAmmoPurchased;
        AmmoCount = GameManager.Instance.GetAmmoCount();
        IncreaseAmmoCount();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnSnakeShot -= OnSnakeShot;
        GameManager.Instance.OnAmmoPurchased -= OnAmmoPurchased;
    }

    private void OnSnakeShot()
    {
        AmmoCount = GameManager.Instance.GetAmmoCount();
        DecreaseAmmoCount();
    }

    private void OnAmmoPurchased()
    {
        AmmoCount = GameManager.Instance.GetAmmoCount();
        IncreaseAmmoCount();
    }

    private void IncreaseAmmoCount()
    {
        for (int i = 0; i < AmmoCount; i++)
        {
            Image newAmmoImage = PoolingSystem.Spawn<Image>(
                AmmoImagePrefab,
                AmmoContainer.transform,
                AmmoImagePrefab.transform.localScale,
                Vector3.zero,
                Quaternion.identity);
            AmmoImageList.Add(newAmmoImage);
        }
    }

    private void DecreaseAmmoCount()
    {
        if (AmmoImageList.Count > 0)
        {
            PoolingSystem.Despawn(AmmoImagePrefab, AmmoImageList[0].gameObject);
            AmmoImageList.RemoveAt(0);
        }
    }
}
