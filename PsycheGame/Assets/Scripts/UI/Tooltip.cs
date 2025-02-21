using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title, _description;

    private void Rebuild()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    public void SetTitle(string title)
    {
        _title.text = title;
        Rebuild();
    }

    public void SetDescription(string description)
    {
        _description.text = description;
        Rebuild();
    }

    public void SetPosition(Vector3 position)
    {
        Vector3[] worldCorners = new Vector3[4];
        (transform.parent as RectTransform).GetWorldCorners(worldCorners);

        Rect rect = (transform as RectTransform).rect;
        position.x = Mathf.Clamp(position.x, worldCorners[0].x + rect.width / 2, worldCorners[2].x - rect.width / 2);
        position.y = Mathf.Clamp(position.y, worldCorners[0].y + rect.height / 2, worldCorners[2].y - rect.height / 2);

        transform.position = position;
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
