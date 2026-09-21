using UnityEngine;
using TMPro;

public class BlinkingTextFX : MonoBehaviour
{
    [SerializeField] private float blinkSpeed = 2.0f;

    private TextMeshProUGUI _textObject;

    void Start()
    {
        _textObject = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        BlinkText();
    }

    private void BlinkText()
    {
        var color = _textObject.color;

        color.a = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);

        _textObject.color = color;
    }
}
