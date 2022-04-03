using UnityEngine;

public class Seed : Item
{

    public override void Activate()
    {
        Launch(0f, 800f);
    }

    private void Update()
    {
        if(transform.position.y < -8) Destroy(gameObject);

        if (c2D.IsTouching(FilterGrassBlock) && !IsLanded)
        {
            IsLanded = true;
            var temp = new Collider2D[1];
            c2D.GetContacts(FilterGrassBlock, temp);

            if (temp[0].GetComponent<GrassBlockCollection>() != null)
            {
                Debug.Log("oops");
            }
            
            if (temp[0].GetComponent<GrassBlock>().GetIsFertile())
            {
                Debug.Log("ground is fertile");
                //todo - have the ground close
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("ground is not fertile");
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
                Destroy(gameObject, 0.5f);
            }
        }
    }
}
