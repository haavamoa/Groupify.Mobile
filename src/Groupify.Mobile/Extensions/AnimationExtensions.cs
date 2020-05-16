using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Groupify.Mobile.Extensions
{
    public static class AnimationExtensions
    {
        public static Task Shake(this VisualElement visualElement)
        {
            var tcs = new TaskCompletionSource<bool>();
            new Animation {
        { 0, 0.125, new Animation (v => visualElement.TranslationX = v, 0, -13) },
        { 0.125, 0.250, new Animation (v => visualElement.TranslationX = v, -13, 13) },
        { 0.250, 0.375, new Animation (v => visualElement.TranslationX = v, 13, -11) },
        { 0.375, 0.5, new Animation (v => visualElement.TranslationX = v, -11, 11) },
        { 0.5, 0.625, new Animation (v => visualElement.TranslationX = v, 11, -7) },
        { 0.625, 0.75, new Animation (v => visualElement.TranslationX = v, -7, 7) },
        { 0.75, 0.875, new Animation (v => visualElement.TranslationX = v, 7, -5) },
        { 0.875, 1, new Animation (v => visualElement.TranslationX = v, -5, 0) }
    }.Commit(visualElement, "AppleShakeChildAnimations", length: 500, easing: Easing.Linear, finished: (x, y) => tcs.SetResult(true));
            return tcs.Task;
        }
    }
}
