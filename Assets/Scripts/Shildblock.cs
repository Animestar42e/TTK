using UnityEngine;

public class ShieldBlock : MonoBehaviour
{
    public Collider shieldCollider;
    public GameObject blockEffect;

    public KeyCode blockKey = KeyCode.F;

    private bool isBlocking;

    void Start()
    {
        if (shieldCollider != null)
        {
            shieldCollider.enabled = false;
        }

        if (blockEffect != null)
        {
            blockEffect.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKey(blockKey))
        {
            isBlocking = true;

            if (shieldCollider != null)
            {
                shieldCollider.enabled = true;
            }

            if (blockEffect != null)
            {
                blockEffect.SetActive(true);
            }
        }
        else
        {
            isBlocking = false;

            if (shieldCollider != null)
            {
                shieldCollider.enabled = false;
            }

            if (blockEffect != null)
            {
                blockEffect.SetActive(false);
            }
        }
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }
}