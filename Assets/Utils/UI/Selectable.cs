using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gram.Utils.UI
{
    public class Selectable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        public delegate void SelectableEvent(PointerEventData data);

        public event SelectableEvent OnSelected;
        public event SelectableEvent OnSelectMore;
        public event SelectableEvent OnSelectMoreEnd;
        

        private bool _hasHold = false;
        private Coroutine _holdCoroutine = null;
        
        public void OnPointerDown(PointerEventData data) {
            if(_holdCoroutine == null && !_hasHold)
            {
                _holdCoroutine = StartCoroutine(HoldCoroutineUtility(data));
            }
        }

        private IEnumerator HoldCoroutineUtility(PointerEventData pointerEventData) {
            _hasHold = false;

            float deltaTime = 0;
            while (deltaTime < 3) {
                deltaTime += Time.deltaTime;
                yield return null;
            }
            _hasHold = true;
            _holdCoroutine = null;
            OnSelectMore?.Invoke(pointerEventData);
            
            yield return null;
        }

        public void OnPointerUp(PointerEventData data) {
            
            if (_holdCoroutine != null) {
                StopCoroutine(_holdCoroutine);
                _holdCoroutine = null;
            }

            if (_hasHold) {
                OnSelectMoreEnd?.Invoke(data);
            } else {
                OnSelected?.Invoke(data);
            }
            
            _hasHold = false;
        }

        
    }
}
