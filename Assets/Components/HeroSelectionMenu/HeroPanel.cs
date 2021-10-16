using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HeroSelectionMenu
{
    public class HeroPanel : MonoBehaviour
    {

        
        
        [SerializeField]
        private Image PanelImage;
        
        private Coroutine _holdCoroutine = null;
        
        public void OnPointerDown() {
            Debug.Log("on point down");
            if(_holdCoroutine == null)
            {
                _holdCoroutine = StartCoroutine(HoldCoroutineUtility());
            }

            PanelImage.color = Color.white;
            
        }

        private IEnumerator HoldCoroutineUtility() {

            float deltaTime = 0;
            while (deltaTime < 3) {
                deltaTime += Time.deltaTime;
                yield return null;
            }
            _holdCoroutine = null;
            yield return null;
        }

        public void OnClick() {
            if (_holdCoroutine != null) {
                StopCoroutine(_holdCoroutine);
                _holdCoroutine = null;
            }
        }
    }
}