using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveCore : MonoBehaviour
{
    [SerializeField] private Glove glove;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<GrassBlockCollection>())
        {
            glove.HideGlove();
        }
        
        if (col.GetComponent<GrassBlock>())
        {
            col.GetComponent<GrassBlock>().ShowShovel();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<GrassBlockCollection>())
        {
            glove.ShowGlove();
        }
        
        if (other.GetComponent<GrassBlock>())
        {
            other.GetComponent<GrassBlock>().HideShovel();
        }
    }
}
