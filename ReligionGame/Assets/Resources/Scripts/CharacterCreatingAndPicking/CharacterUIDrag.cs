using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterUIDrag : MonoBehaviour
{
    [SerializeField] Image CharacterPanel;
    public float speed;
    public float friction;
    public float lerpSpeed;
    private float xDeg;
    private float yDeg;
    private Quaternion fromRotation;
    private Quaternion toRotation;

    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = CharacterPanel.GetComponent<RectTransform>();
        Debug.Log(rectTransform);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //Debug.Log($"Rect: {rectTransform.rect}  --- mouse: {Input.mousePosition}");
            Rect rect = new Rect(
                new Vector2() { x = rectTransform.anchoredPosition.x, y = rectTransform.anchoredPosition.y },
                new Vector2() { x = rectTransform.sizeDelta.x, y = rectTransform.sizeDelta.y }
            );
            if (rect.Contains(Input.mousePosition))
            {
                xDeg -= Input.GetAxis("Mouse X") * speed * friction;
                //yDeg -= Input.GetAxis("Mouse Y") * speed * friction;
                fromRotation = transform.rotation;
                toRotation = Quaternion.Euler(yDeg, xDeg, 0);
                transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpSpeed);
            }
        }
    }
}
