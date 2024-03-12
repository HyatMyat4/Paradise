using Godot;
using System;
using System.Collections.Generic;

public partial class world : Node2D
{

	public TileMap tileMap;



	public int ground_layer = 1;
	public int environment_layer = 2;
	public string can_place_seed = "can_place_seeds";

	public string can_place_dirt = "can_place_dirt";
	public Godot.Collections.Array<Godot.Vector2I> dirt_tiles = new Godot.Collections.Array<Godot.Vector2I>();

	public enum FarmingMode
	{
		Seeds,
		Dirt
	}

	public FarmingMode farming_status = FarmingMode.Dirt;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tileMap = GetNode<TileMap>("TileMap");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("toggle_dirt"))
		{
			farming_status = FarmingMode.Dirt;
			GD.Print("toggle_dirt");
		}
		else if (Input.IsActionJustPressed("toggle_seeds"))
		{
			farming_status = FarmingMode.Seeds;
			GD.Print("toggle_seeds");
		};

		if (Input.IsActionJustPressed("click"))
		{

			// Example Vector2 (47, 118)
			Vector2 mouse_position = GetGlobalMousePosition();
			// Chnage Vector2 to Vector2I
			Vector2I tile_mouse_position = tileMap.LocalToMap(mouse_position);


			if (farming_status == FarmingMode.Dirt && CheckCanPlace(ground_layer, tile_mouse_position, can_place_dirt))
			{
				dirt_tiles.Add(tile_mouse_position);
				tileMap.SetCellsTerrainConnect(ground_layer, dirt_tiles, 2, 0);
			}
			else if (farming_status == FarmingMode.Seeds && CheckCanPlace(ground_layer, tile_mouse_position, can_place_seed))
			{
				int leven = 0;
				int final_seed_leven = 3;
				Vector2I atlas_coordinates = new Vector2I(11, 1);
				HandleSeed(tile_mouse_position, atlas_coordinates, leven, final_seed_leven);
			};
		}

	}

	public bool CheckCanPlace(int layer, Vector2I mouse_position, string custom_data_layer)
	{
		TileData cell_tile_data = tileMap.GetCellTileData(layer, mouse_position);
		if (cell_tile_data != null)
		{
			bool can_place = (bool)cell_tile_data.GetCustomData(custom_data_layer);
			return can_place;
		}
		else
		{
			return false;
		};

	}

	public async void HandleSeed(Vector2I tile_mouse_position, Vector2I atlas_coordinates, int leven, int final_seed_leven)
	{
		int source_id = 0;
		tileMap.SetCell(environment_layer, tile_mouse_position, source_id, atlas_coordinates);

		SceneTreeTimer timer = GetTree().CreateTimer(3.0);

		await ToSignal(timer, "timeout");

		GD.Print("30 seconds delay completed.");

		if (leven == final_seed_leven)
		{
			return;
		}
		else
		{
			Vector2I new_atlas = new Vector2I(atlas_coordinates.X + 1, atlas_coordinates.Y);
			atlas_coordinates = new_atlas;
		}
		HandleSeed(tile_mouse_position, atlas_coordinates, leven + 1, final_seed_leven);
	}



}

