using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private Image AmmoImage;
    [SerializeField] private GameObject AmmoContainer;
    private int AmmoCount;
    private List<Image> AmmoImageList = new();

    private void Update()
    {
        AmmoCount = GameManager.Instance.GetAmmoCount();

        if (AmmoImageList.Count < AmmoCount)
        {
            Image newAmmoImage = PoolingSystem.Spawn<Image>(
                AmmoImage.gameObject,
                AmmoContainer.transform,
                Vector3.one,
                Vector3.zero,
                Quaternion.identity);
            AmmoImageList.Add(newAmmoImage);
        }

        if (AmmoImageList.Count > AmmoCount)
        {
            PoolingSystem.Despawn(AmmoImage.gameObject, AmmoImageList[AmmoImageList.Count - 1].gameObject);
            AmmoImageList.RemoveAt(AmmoImageList.Count - 1);
        }
    }
}
