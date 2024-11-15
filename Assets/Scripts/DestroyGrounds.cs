using UnityEngine;
using UnityEngine.Tilemaps;

public class DestroyGrounds : MonoBehaviour
{
    [SerializeField]
    Transform Boss;

	[SerializeField]
	GridLayout GridLayout;

	[SerializeField]
	Tilemap Tilemap;

	[SerializeField]
	ParticleSystem ParticleSystem;

	// Update is called once per frame
	void Update()
    {
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				Vector3Int position = GridLayout.WorldToCell(Boss.position);
				TileBase tile = Tilemap.GetTile(new Vector3Int(position.x + i, position.y + j, position.z));
				if (tile)
				{
					Instantiate(ParticleSystem, Boss.position, new Quaternion());
				}
				Tilemap.SetTile(GridLayout.WorldToCell(new Vector3(Boss.position.x + i, Boss.position.y + j, Boss.position.z)), null);
			}
		}
	}
}
