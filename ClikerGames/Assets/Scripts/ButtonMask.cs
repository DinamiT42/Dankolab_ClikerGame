using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonMask : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float _alphaLevel = 1f;
    private Image _imageButton;

    void Start()
    {
        _imageButton = GetComponent<Image>();
        _imageButton.alphaHitTestMinimumThreshold = _alphaLevel;
    }
}