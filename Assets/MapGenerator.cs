﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour {

	public int num_cells_x;
	public int num_cells_y;
	public int cell_width;

	public string seed;
	public bool useRandomSeed;

	Cell[,] map;

	void Start() {
		GenerateBaseMap();
		ConstructMaze();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			GenerateBaseMap();
			ConstructMaze();
		}
	}

	void GenerateBaseMap() {
		map = new Cell[num_cells_x,num_cells_y];

		for (int x = 0; x < num_cells_x; x++) {
			for (int y = 0; y < num_cells_y; y++) {
				map [x, y] = new Cell (x, y);

				if (x == 0) {
					map [x, y].leftWallState = WallState.Border;
				}

				if (x == num_cells_x - 1) {
					map [x, y].rightWallState = WallState.Border;
				}

				if (y == 0) {
					map [x, y].topWallState = WallState.Border;
				}

				if (y == num_cells_y - 1) {
					map [x, y].bottomWallState = WallState.Border;
				}
			}
		}
	}

	void ConstructMaze() {
		int visited_cells = 1;
		int total_cells = num_cells_x * num_cells_y;

		Stack<Cell> cell_stack = new Stack<Cell> ();

		if (useRandomSeed) {
			seed = Time.time.ToString();
		}

		System.Random rng = new System.Random(seed.GetHashCode());

		Cell current_cell = map [rng.Next (num_cells_x), rng.Next (num_cells_y)];

		while (visited_cells < total_cells) {

			List<Cell> closed_neighbors = GetClosedNeighbors (current_cell);

			if (closed_neighbors.Count > 0) {
				Cell next_cell = closed_neighbors [rng.Next (closed_neighbors.Count)];

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

	void OnDrawGizmos() {
		if (map != null){
			int total_width = cell_width * num_cells_x;
			int total_height = cell_width * num_cells_y;

			for (int x = 0; x < num_cells_x; x++) {
				for (int y = 0; y < num_cells_y; y++) {

					Gizmos.color = Color.blue;
					if (map [x, y].topWallState == WallState.Closed || map [x, y].topWallState == WallState.Border) {
						if (map [x, y].topWallState == WallState.Border) {
							Gizmos.color = Color.red;
						}

						Vector2 start_pt = new Vector2((x*cell_width)-(total_width/2),(y*cell_width)-(total_height/2));
						Vector2 end_pt = new Vector2(((x+1)*cell_width)-(total_width/2), (y*cell_width)-(total_height/2));
						Gizmos.DrawLine (start_pt, end_pt);
					}

					Gizmos.color = Color.blue;
					if (map [x, y].bottomWallState == WallState.Closed || map [x, y].bottomWallState == WallState.Border) {
						if (map [x, y].bottomWallState == WallState.Border) {
							Gizmos.color = Color.red;
						}

						Vector2 start_pt = new Vector2((x*cell_width)-(total_width/2),((y+1)*cell_width)-(total_height/2));
						Vector2 end_pt = new Vector2(((x+1)*cell_width)-(total_width/2), ((y+1)*cell_width)-(total_height/2));
						Gizmos.DrawLine (start_pt, end_pt);
					}

					Gizmos.color = Color.blue;
					if (map [x, y].leftWallState == WallState.Closed || map [x, y].leftWallState == WallState.Border) {
						if (map [x, y].leftWallState == WallState.Border) {
							Gizmos.color = Color.red;
						}

						Vector2 start_pt = new Vector2((x*cell_width)-(total_width/2),(y*cell_width)-(total_height/2));
						Vector2 end_pt = new Vector2((x*cell_width)-(total_width/2), ((y+1)*cell_width)-(total_height/2));
						Gizmos.DrawLine (start_pt, end_pt);
					}

					Gizmos.color = Color.blue;
					if (map [x, y].rightWallState == WallState.Closed || map [x, y].rightWallState == WallState.Border) {
						if (map [x, y].rightWallState == WallState.Border) {
							Gizmos.color = Color.red;
						}

						Vector2 start_pt = new Vector2(((x+1)*cell_width)-(total_width/2),(y*cell_width)-(total_height/2));
						Vector2 end_pt = new Vector2(((x+1)*cell_width)-(total_width/2), ((y+1)*cell_width)-(total_height/2));
						Gizmos.DrawLine (start_pt, end_pt);
					}
				}
			}
		}
	}
}