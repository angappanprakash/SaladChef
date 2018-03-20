using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

[System.Serializable]
public class PlayerSpawnMaterials
{
	public Material			_spawnMaterial;
	public PlayerColor	    _playerColor;
}

[System.Serializable]
public class PlayerMarkerTextures
{
    public Texture 		_MarkerTexture;
    public PlayerColor 	_playerColor;

}

[CreateAssetMenu(menuName = "GameAssets/CharacterCommonData")]
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

    public List<PlayerSpawnMaterials>  	_playerSpawnMaterials;
    public List<PlayerMarkerTextures> 	_playerMarkerTextures;

    public Material GetSpawnMaterial(PlayerColor playerColor)
    {
        PlayerSpawnMaterials spawnMaterialMap = _playerSpawnMaterials.Find(x => x._playerColor == playerColor);
        if (spawnMaterialMap != null)
            return spawnMaterialMap._spawnMaterial;

        return null;
    }

    public Texture GetPlayerMarkerTexture(PlayerColor playerColor)
    {
        PlayerMarkerTextures playerMarkerTextures = _playerMarkerTextures.Find(x => x._playerColor == playerColor);
        if (playerMarkerTextures != null)
        {
            return playerMarkerTextures._MarkerTexture;
        }

        return null;
    }

    public Color GetColorValue(PlayerColor playerColor)
    {
        ColorMap colorMap = _colorMaps.Find(x => x._playerColor == playerColor);
        if(colorMap != null)
            return colorMap._colorValue;

        return Color.white;
    }
}
