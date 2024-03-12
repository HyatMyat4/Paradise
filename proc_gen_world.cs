using Godot;
using System;

public partial class proc_gen_world : Node2D
{
	[Export]
	public NoiseTexture2D noise_height_texture;
	public TileMap tileMap;
	const int width = 400;
	const int height = 400;
	const int scource_id = 0;
	Vector2I water_atlas = new Vector2I(0, 1);
	Vector2I land_alts = new Vector2I(0, 0);

	public Noise Noise;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tileMap = GetNode<TileMap>("TileMap");
		Noise = noise_height_texture.Noise;
		generate_world();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void generate_world()
	{
		GD.Print("IN Case");
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				float noise_vale = Noise.GetNoise2D(x, y);
				if (noise_vale > 0.0)
				{
					tileMap.SetCell(0, new Vector2I(x, y), scource_id, water_atlas);
				}
				else if (noise_vale < 0.0)
				{
					tileMap.SetCell(0, new Vector2I(x, y), scource_id, land_alts);
				}
			};
		};
	}
}
