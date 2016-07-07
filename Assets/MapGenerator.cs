using UnityEngine;
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour {

	public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;

	Cell[,] map;

	void Start() {
		GenerateBaseMap();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			GenerateBaseMap();
			ConstructMaze();
		}
	}

	void GenerateBaseMap() {
		map = new Cell[width,height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				map [x, y] = new Cell (x, y);

				if (x == 0) {
					map [x, y].leftWallState = WallState.Border;
				}

				if (x == width - 1) {
					map [x, y].rightWallState = WallState.Border;
				}

				if (y == 0) {
					map [x, y].topWallState = WallState.Border;
				}

				if (x == height - 1) {
					map [x, y].bottomWallState = WallState.Border;
				}
			}
		}
	}

	void ConstructMaze() {
		int visited_cells = 1;
		int total_cells = width * height;

		Stack<Cell> cell_stack = new Stack<Cell> ();

		if (useRandomSeed) {
			seed = Time.time.ToString();
		}

		System.Random rng = new System.Random(seed.GetHashCode());

		Cell current_cell = map [rng.Next (0, width - 1), rng.Next (0, height - 1)];

		while (visited_cells < total_cells) {

			List<Cell> closed_neighbors = GetClosedNeighbors (current_cell);

			if (closed_neighbors.Count > 0) {
				Cell next_cell = closed_neighbors [rng.Next (0, closed_neighbors.Count - 1)];

				if (current_cell.x < next_cell.x) {
					current_cell.rightWallState = WallState.Open;
					next_cell.leftWallState = WallState.Open;
				}

				if (current_cell.x > next_cell.x) {
					current_cell.leftWallState = WallState.Open;
					next_cell.rightWallState = WallState.Open;
				}

				if (current_cell.y < next_cell.y) {
					current_cell.bottomWallState = WallState.Open;
					next_cell.topWallState = WallState.Open;
				}

				if (current_cell.y > next_cell.y) {
					current_cell.topWallState = WallState.Open;
					next_cell.bottomWallState = WallState.Open;
				}

				cell_stack.Push (current_cell);
				current_cell = next_cell;
				visited_cells++;
			} else {
				current_cell = cell_stack.Pop ();
			}
		}
	}

	private List<Cell> GetClosedNeighbors(Cell cell) {
		List<Cell> result = new List<Cell> ();

		if (cell.topWallState != WallState.Border) {
			if (CheckAllWallsClosed(map[cell.x, cell.y - 1])) {
				result.Add(map[cell.x, cell.y - 1]);
			}
		}

		if (cell.rightWallState != WallState.Border) {
			if (CheckAllWallsClosed(map[cell.x + 1, cell.y])) {
				result.Add(map[cell.x + 1, cell.y]);
			}
		}

		if (cell.bottomWallState != WallState.Border) {
			if (CheckAllWallsClosed(map[cell.x, cell.y + 1])) {
				result.Add(map[cell.x, cell.y + 1]);
			}
		}

		if (cell.leftWallState != WallState.Border) {
			if (CheckAllWallsClosed(map[cell.x - 1,cell.y])) {
				result.Add(map[cell.x - 1, cell.y]);
			}
		}

		return result;
	}

	private bool CheckAllWallsClosed(Cell cell){
		bool result = true;

		if (cell.topWallState == WallState.Open) {
			result = false;
		}

		if (cell.rightWallState == WallState.Open) {
			result = false;
		}

		if (cell.bottomWallState == WallState.Open) {
			result = false;
		}

		if (cell.leftWallState == WallState.Open) {
			result = false;
		}

		return result;
	}
}