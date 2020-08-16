using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UiEffect;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    [SerializeField] private Image _backGround;
    [SerializeField] private Image _img1;
    [SerializeField] private Image _img2;
    [SerializeField] private Image _img3;
    [SerializeField] private Image _title;

    private RectTransform _transform1;
    private RectTransform _transform2;
    private RectTransform _transform3;
    private RectTransform _transform4;
    private GradientAlpha _alpha3;
    private GradientAlpha _alpha1;
    private GradientAlpha _alpha2;

    // Start is called before the first frame update
    private void Start()
    {
        _backGround.color = Color.black;

        _transform1 = _img1.GetComponent<RectTransform>();
        _transform2 = _img2.GetComponent<RectTransform>();
        _transform3 = _img3.GetComponent<RectTransform>();
        _transform4 = _title.GetComponent<RectTransform>();
        _transform1.eulerAngles = new Vector3(0,0,30);
        _transform1.localPosition += new Vector3(0,150,0);
        _transform2.eulerAngles = new Vector3(0,0,30);
        _transform2.localPosition += new Vector3(0,150,0);
        _transform3.eulerAngles = new Vector3(0,0,30);
        _transform3.localPosition += new Vector3(56,150,0);
        _transform4.localPosition = new Vector3(0, -16, 0);

        _alpha2 = _img2.GetComponent<GradientAlpha>();
        _alpha1 = _img1.GetComponent<GradientAlpha>();
        _alpha3 = _img3.GetComponent<GradientAlpha>();
        
        _img1.gameObject.SetActive(false);
        _img2.gameObject.SetActive(false);
        _img3.gameObject.SetActive(false);
        _title.gameObject.SetActive(false);

        var seq = DOTween.Sequence();

        seq.AppendInterval(1)
            .AppendCallback(() => _img1.gameObject.SetActive(true))
            .Append(_transform1.DOLocalMoveY(-150, 0.25f).SetEase(Ease.InSine).SetRelative())
            .Join(DOVirtual.Float(0, 1, 0.25f, value =>
            {
                _alpha1.alphaTop = (value - 0.5f) * 2f;
                _alpha1.alphaRight = (value - 0.5f) * 2f;
                _alpha1.alphaBottom = value * 2f;
                _alpha1.alphaLeft = value * 2f;
            }).SetEase(Ease.Linear))
            .AppendCallback(() => _img2.gameObject.SetActive(true))
            .Append(_transform2.DOLocalMoveY(-150, 0.25f).SetEase(Ease.InSine).SetRelative())
            .Join(DOVirtual.Float(0, 1, 0.25f, value =>
            {
                _alpha2.alphaTop = (value - 0.5f) * 2f;
                _alpha2.alphaRight = (value - 0.5f) * 2f;
                _alpha2.alphaBottom = value * 2f;
                _alpha2.alphaLeft = value * 2f;
            }).SetEase(Ease.Linear))
            .Join(_transform1.DOLocalRotate(Vector3.zero, 0.25f / 3f).SetEase(Ease.Linear))
            .AppendCallback(() => _img3.gameObject.SetActive(true))
            .Append(_transform3.DOLocalMoveY(-150, 0.25f).SetEase(Ease.InSine).SetRelative())
            .Join(DOVirtual.Float(0, 1, 0.25f, value =>
            {
                _alpha3.alphaTop = (value - 0.5f) * 2f;
                _alpha3.alphaRight = (value - 0.5f) * 2f; 
                _alpha3.alphaBottom = value * 2f;
                _alpha3.alphaLeft = value * 2f;
            }).SetEase(Ease.Linear))
            .Join(_transform2.DOLocalRotate(Vector3.zero, 0.25f / 3f).SetEase(Ease.Linear))
            .Append(_transform3.DOLocalRotate(Vector3.zero, 0.25f / 3f).SetEase(Ease.Linear))
            .AppendInterval(2f / 3f)
            .Append(_transform3.DOLocalMoveX(16, 0.25f).SetEase(Ease.OutSine).SetRelative())
            .AppendCallback(() => _title.gameObject.SetActive(true))
            .Append(_transform3.DOLocalMoveX(-72, 1f / 3f).SetEase(Ease.InSine).SetRelative())
            .Join(_transform4.DOLocalMoveX(213, 1f / 3f).SetEase(Ease.InSine).SetRelative())
            .Join(_backGround.DOColor(Color.white, 1f / 3f).SetEase(Ease.Linear))
            .AppendInterval(1)
            .AppendCallback(() => StartCoroutine(LoadScene()));
    }

    private IEnumerator LoadScene()
    {
        yield return SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Additive);
        // yield return SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Additive);
        
        SplashEnd();
    }

    private void SplashEnd()
    {
        var seq = DOTween.Sequence();

        seq.Append(_title.DOFade(0, 0.333f))
            .AppendInterval(0.333f)
            .Append(_transform3.DOLocalMoveX(-16, 0.25f).SetEase(Ease.OutSine).SetRelative())
            .Append(_transform3.DOLocalMoveX(528, 5f / 12f).SetEase(Ease.InSine).SetRelative())
            .Join(DOVirtual.Float(1, 0, 5f / 12f, value =>
            {
                // alpha3.alphaTop = (value - 0.5f) * 2f;
                _alpha3.alphaRight = (value - 0.5f) * 2f;
                // alpha3.alphaBottom = value * 2f;
                _alpha3.alphaLeft = value;
            }).SetEase(Ease.Linear))
            .Join(_transform2.DOLocalMoveY(528, 5f / 12f).SetEase(Ease.InSine).SetRelative().SetDelay(0.18f))
            .Join(DOVirtual.Float(1, 0, 5f / 12f, value =>
            {
                _alpha2.alphaTop = (value - 0.5f) * 2f;
                // alpha2.alphaRight = (value - 0.5f) * 2f;
                _alpha2.alphaBottom = value;
                // alpha2.alphaLeft = value;
            }).SetEase(Ease.Linear))
            .Join(_transform1.DOLocalMoveX(-528, 5f / 12f).SetEase(Ease.InSine).SetRelative().SetDelay(0.18f))
            .Join(DOVirtual.Float(1, 0, 5f / 12f, value =>
            {
                // alpha1.alphaTop = (value - 0.5f) * 2f;
                _alpha1.alphaRight = value;
                // alpha1.alphaBottom = value * 2f;
                _alpha1.alphaLeft = (value - 0.5f) * 2f;
            }).SetEase(Ease.Linear))
            .Append(_backGround.DOFade(0, 0.25f))
            .AppendCallback(() => SceneManager.UnloadSceneAsync("SplashScene"));
    }
}
