using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    public Image mask;
    private float originalSize;

    public static UIHealthBar instance { get; private set; }

    public bool hasTaken;

    public int fixedNum;
    
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(float fillPrecent)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * fillPrecent);
    }
}
