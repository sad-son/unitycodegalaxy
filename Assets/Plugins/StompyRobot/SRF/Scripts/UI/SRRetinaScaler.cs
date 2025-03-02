namespace SRF.UI
{
    using Internal;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Detects when a screen dpi exceeds what the developer considers
    /// a "retina" level display, and scales the canvas accordingly.
    /// </summary>
    [RequireComponent(typeof (CanvasScaler))]
    [AddComponentMenu(ComponentMenuPaths.RetinaScaler)]
    public class SRRetinaScaler : SRMonoBehaviour
    {
        [SerializeField] private bool _disablePixelPerfect = false;

        [SerializeField] private int _designDpi = 120;

        private void Start()
        {
            ApplyScaling();
        }

        private void ApplyScaling()
        {
            var dpi = Screen.dpi;

            Debug.Log($"SR_RetinaScaler ::  dpi = {dpi}");

            if (dpi <= 0)
            {
                return;
            }

#if !UNITY_EDITOR && UNITY_IOS
            // No iOS device has had low dpi for many years - Unity must be reporting it wrong.
            if(dpi < 120)
            {
                dpi = 321;
            }
#endif
            var scaler = GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

            var preciseScale = dpi / _designDpi;
            Debug.Log($"SR_RetinaScaler ::  precise scale = {preciseScale}");

            // Round scale to nearest 0.5
            var roundedScale = Mathf.Min(Mathf.Max(1, Mathf.Round(preciseScale * 2) / 2.0f), 2);
            Debug.Log($"SR_RetinaScaler ::  rounded scale = {roundedScale}");

            scaler.scaleFactor = roundedScale;

            if (_disablePixelPerfect)
            {
                GetComponent<Canvas>().pixelPerfect = false;
            }
        }
    }
}
