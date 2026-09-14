using UnityEngine;
using TMPro;

public class ShieldBlock : MonoBehaviour
{
    [Header("Shield")]
    public Collider shieldCollider;
    public GameObject blockEffect;

    [Header("Block Key")]
    public KeyCode blockKey = KeyCode.F;

    [Header("Block Counter")]
    public int blockCount = 0;
    public TMP_Text blockCounterText;

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

        UpdateBlockCounter();
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

    // Call this when an enemy attack is successfully blocked
    public void SuccessfulBlock()
    {
        blockCount++;

        UpdateBlockCounter();

        Debug.Log("Successful Blocks: " + blockCount);
    }

    void UpdateBlockCounter()
    {
        if (blockCounterText != null)
        {
            blockCounterText.text = "Blocks: " + blockCount;
        }
    }
}