using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockHealthColors", menuName = "ScriptableObjects/BlockHealthColors")]
public class BlockHealthColors : ScriptableObject
{
    public List<BlockColor> blockColors;

    public Color GetColorByHealth(int health)
    {
        foreach (BlockColor blockColor in blockColors)
        {
            if(health == blockColor.lives)
                return blockColor.color;
        }
        
        return Color.white;
    }
}

[Serializable]
public struct BlockColor
{
    public int lives;  
    public Color color;
}
