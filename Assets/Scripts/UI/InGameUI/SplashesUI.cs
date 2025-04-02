using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SplashesUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _splashCanvasGroup;
    [SerializeField] private CanvasGroup[] _restOfUI;
    [SerializeField] private GameObject [] _buttons;
    [SerializeField] private GameObject _waveSplash;
    [SerializeField] private GameObject _defeatSplash;
    [SerializeField] private GameObject _victorySplash;

    public void ShowWave()
    {   
        StartCoroutine(Wave());
    }
    public void ShowDefeat()
    {
        StartCoroutine(Defeat());
    }
    public void ShowVictory()
    {
        StartCoroutine(Victory());
    }
    IEnumerator Wave()
    {
            _splashCanvasGroup.DOFade(1,0.6f);

        yield return new WaitForSeconds(1f);
            
            _waveSplash.SetActive(true);

        yield return new WaitForSeconds(1f);

            _splashCanvasGroup.DOFade(0,0.6f).OnComplete(() => _waveSplash.SetActive(false));
        
        yield return null;
    }
    IEnumerator Defeat()
    {
        DisableUI();
        _splashCanvasGroup.DOFade(1,0.6f);
            
        yield return new WaitForSeconds(1f);
            _splashCanvasGroup.blocksRaycasts = true;
            _splashCanvasGroup.interactable = true;
            _defeatSplash.SetActive(true);
       
        foreach(var button in _buttons)
        {   
            
            button.SetActive(true);
        }
        yield return null;
    }
    IEnumerator Victory()
    {        
        DisableUI();
        _splashCanvasGroup.DOFade(1,0.6f);
            
        yield return new WaitForSeconds(1f);
            _splashCanvasGroup.blocksRaycasts = true;
            _splashCanvasGroup.interactable = true;
            _victorySplash.SetActive(true);
             foreach(var button in _buttons)
        {   
            
            button.SetActive(true);
        }
        
        yield return null;
    }
    private void DisableUI()
    {
        foreach( var cv in _restOfUI)
        {
            cv.blocksRaycasts = false;
            cv.interactable = false;
            cv.DOFade(0,0.6f);
        }
       
    }
}
