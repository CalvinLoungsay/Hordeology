using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScript : MonoBehaviour
{
    public Sprite[] spriteArray;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void ChangeSprite(int index)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteArray[index];
    }

    public int GetSpriteArrayLength() {
        return spriteArray.Length;
    }

    public void ChangeSpriteArray(Sprite[] array) {
        spriteArray = array;
    }
}
