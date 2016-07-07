using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

	public WallState leftWallState { get; set; }
	public WallState rightWallState { get; set; }
	public WallState topWallState { get; set; }
	public WallState bottomWallState { get; set; }
	public bool cellVisited { get; set; }
	public int x { get; set; }
	public int y { get; set; }

	public Cell(int pos_x, int pos_y, WallState left_wall_state, WallState right_wall_state, WallState top_wall_state, WallState bottom_wall_state){
		x = pos_x;
		y = pos_y;
		leftWallState = left_wall_state;
		rightWallState = right_wall_state;
		topWallState = top_wall_state;
		bottomWallState = bottom_wall_state;
		cellVisited = false;
	}

	public Cell(int pos_x, int pos_y){
		x = pos_x;
		y = pos_y;
		leftWallState = WallState.Closed;
		rightWallState = WallState.Closed;
		topWallState = WallState.Closed;
		bottomWallState = WallState.Closed;
		cellVisited = false;
	}
}
