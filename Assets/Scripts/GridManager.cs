using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject BG_Tile;
    public Transform Parent;
    private void Start()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                GameObject go = Instantiate(BG_Tile, Parent);
                go.transform.localPosition = new Vector2(x * 2.56f, y * 2.56f);
            }
        }
    }
}
