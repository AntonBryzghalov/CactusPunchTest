using TMPro;
using UnityEngine;

namespace TowerDefence.UI.CommonScreens
{
    public class SimpleCaptionScreen : BaseScreen, IScreenWithData<string>
    {
        [SerializeField] private TMP_Text captionText;

        public void SetData(string caption)
        {
            captionText.text = caption;
        }
    }
}
