using UnityEngine;

public class GloveCore : MonoBehaviour
{
    [SerializeField] private Glove glove;

    private int _numOfBlocks;

    private GrassBlock _grassBlock;

    private void Start()
    {
        _numOfBlocks = 0;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("gloveZone"))
        {
            _numOfBlocks++;

            if (_numOfBlocks > 0) glove.HideGlove();

            if (_grassBlock != null)
            {
                _grassBlock.HideShovel();
            }

            _grassBlock = col.transform.parent.GetComponent<GrassBlock>();
            _grassBlock.ShowShovel();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("gloveZone"))
        {
            _numOfBlocks--;

            if (_numOfBlocks <= 0)
            {
                if (other.transform.parent.GetComponent<GrassBlock>() != null)
                {
                    other.transform.parent.GetComponent<GrassBlock>().HideShovel();
                }
                glove.ShowGlove();
            }
        }
    }
}