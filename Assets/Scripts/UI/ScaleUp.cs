using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class ScaleUp : MonoBehaviour
{   
    [SerializeField] private Vector3 _scale;
    [SerializeField] private float _duration;
    [SerializeField] private Ease _ease;

    [SerializeField] private LoopData loop;
    [Serializable] private class LoopData
    {
        public bool _hasLoop;
        public int loops;
        public  LoopType _loopType;
    }
    void OnEnable()
    {
        if(loop._hasLoop)
            transform.DOScale(_scale,_duration).SetLoops(loop.loops,loop._loopType).SetEase(_ease);
        else
            transform.DOScale(_scale,_duration).SetEase(_ease);
    }
}
