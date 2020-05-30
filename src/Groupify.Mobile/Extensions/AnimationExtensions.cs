using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Groupify.Mobile.Extensions
{
    public static class AnimationExtensions
    {
        /// <summary>
        /// Repeatedly bounces the visual element with a high as long as it has not gotten a cancellation request
        /// </summary>
        /// <param name="visualElement">The visual element to bounce</param>
        /// <param name="cancellationToken">The cancellation token that can be used to cancel the animation</param>
        /// <param name="height">The height to bounce</param>
        public static void Bounce(this VisualElement visualElement, CancellationToken cancellationToken, double height = 25)
        {
            new Animation {
                { 0, 0.25, new Animation (v => visualElement.TranslationY = v, 0, -height) },
                { 0.25, .5, new Animation (v => visualElement.TranslationY = v, -height, 0, easing: Easing.BounceOut) }
            }
            .Commit(visualElement, "BounceAnimation", length: 1000,repeat: () => !cancellationToken.IsCancellationRequested);
        }

        /// <summary>
        /// Shakes the visual element
        /// </summary>
        /// <param name="visualElement"></param>
        /// <returns>Awaitable task</returns>
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
                }
            .Commit(visualElement, "ShakeAnimation", length: 500, easing: Easing.Linear, finished: (x, y) => tcs.SetResult(true));
        return tcs.Task;
        }
    }
}
