using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title, _description;

    public void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    public void SetTitle(string title)
    {
        _title.text = title;
    }

    public void SetDescription(string description)
    {
        _description.text = description;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
