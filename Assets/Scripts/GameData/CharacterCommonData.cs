using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class CharacterCommonData : ScriptableObject
{
    [System.Serializable]
    public class ColorMap
    {
        public PlayerColor _playerColor;
        public Color       _colorValue;
    }

    public float _runSpeed;
    public float _minRunSpeed;
    public float _maxRunSpeed;
    public List<ColorMap> _colorMaps;

    public Color GetColorValue(PlayerColor playerColor)
    {
        ColorMap colorMap = _colorMaps.Find(x => x._playerColor == playerColor);
        if(colorMap != null)
            return colorMap._colorValue;

        return Color.white;
    }
}
