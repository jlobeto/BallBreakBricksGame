using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public static class UIExtensions
{
    public static void DoNumberTextAnimAsync(this TMP_Text text, int to, float duration)
    {
        text.DOKill();
        var initialValue = int.Parse(text.text);
        var lastValue = int.MinValue;
        var tween = DOVirtual.Int(initialValue, to, duration, value =>
        {
            if (value == lastValue) return;
            lastValue = value;
            text.SetText("{0}", value);
        });
    }
}
