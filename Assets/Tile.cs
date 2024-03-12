using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color colorAlive;
    public Color colorDead;

    public SpriteRenderer spriteRenderer;

    public void Init(bool isAlive)
    {
        spriteRenderer.color = isAlive ? colorAlive:colorDead;
    }
}
