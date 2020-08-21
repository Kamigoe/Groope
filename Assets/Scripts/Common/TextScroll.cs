using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextScroll : BaseMeshEffect
{
    [SerializeField]
    private float _space;
    [SerializeField]
    private float _speed;
    
    private Graphic _cacheGraphic;
    private List<UIVertex> _uiVertexes = new List<UIVertex>();
    
    private float _offset;
    private float _rectWidth;
    private float _pivotX;
    protected override void Awake()
    {
        Init();
    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        Init();
        _cacheGraphic.SetVerticesDirty();
    }
#endif
    private void Init()
    {
        var text = GetComponent<Text>();
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        _cacheGraphic = GetComponent<Graphic>();
        _pivotX = (transform as RectTransform).pivot.x;
        _rectWidth = (transform as RectTransform).rect.width;
        Debug.Log(_rectWidth);
    }
    private void Update()
    {
        _offset += Time.deltaTime * _speed;
        _cacheGraphic.SetVerticesDirty();
    }
    public override void ModifyMesh(VertexHelper vertex)
    {
        _uiVertexes.Clear();
        vertex.GetUIVertexStream(_uiVertexes);
        var count = _uiVertexes.Count;
        if (count > 5)
        {
            var textWidth = _uiVertexes[count - 3].position.x - _uiVertexes[0].position.x;
            // テキスト数
            var textCount = _uiVertexes.Count / 6;
            // 一文字あたりのサイズ
            var charaWidth = textWidth / textCount;
            if (textWidth - charaWidth > _rectWidth)
            {
                var offset = _offset % (textWidth + _space);
                var leftValue = Mathf.Lerp(0, _rectWidth * -1, _pivotX);
                for (var i = 0; i < count; i += 6)
                {
                    var checkVert = _uiVertexes[i + 3];
                    checkVert.position.x -= offset;
                    var isAddValue = checkVert.position.x < leftValue;
                    if (isAddValue)
                        checkVert.position.x += textWidth + _space;
                
                    _uiVertexes[i + 3] = checkVert;
                    foreach (var index in new[]{0, 1, 2, 4, 5})
                    {
                        var vert = _uiVertexes[i + index];
                        vert.position.x -= offset;
                        if (isAddValue)
                            vert.position.x += textWidth + _space;
                        _uiVertexes[i + index] = vert;
                    }
                }
            }
        }
        
        vertex.Clear();
        vertex.AddUIVertexTriangleStream(_uiVertexes);
    }
}