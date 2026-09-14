using UnityEngine;
using TMPro;

public class BlockCounter : MonoBehaviour
{
    public int blocks = 0;
    public TMP_Text blockCounterText;

    void Start()
    {
        UpdateCounter();
    }

    public void SuccessfulBlock()
    {
        blocks++;
        UpdateCounter();

        Debug.Log("Successful Blocks: " + blocks);
    }

    void UpdateCounter()
    {
        if (blockCounterText != null)
        {
            blockCounterText.text = "Blocks: " + blocks;
        }
    }
}