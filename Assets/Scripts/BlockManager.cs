using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] Color OutSideColor;
    private void Start()
    {
        int blockCount = transform.childCount;
        for (int x = 0; x < blockCount; x++)
        {
            transform.GetChild(x).gameObject.GetComponent<Renderer>().material.color = OutSideColor;
        }
    }

}
