/*using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class WallMesh : MonoBehaviour {

	WTF WTF WTF... is this doing here, i cant even remember making this, W.... T.... F....

	public Material[] materials;
	public Material material;
	public bool render;
	public float wall_height;
	public Type type;
	public Windows windows;

	private List<Vector3> vertices = new List<Vector3>();
	private List<List<int>> triangles = new List<List<int>>();
	private List<Vector2> uvs = new List<Vector2>();

	private Mesh mesh;
	private MeshCollider col;
	private Vector3 rot;
	private Vector3 zLocal, xLocal;
	public bool diaganol;
	public bool fix, update_edges;

	private Vector3 pos;

	public enum Type {
		CleanWall,
		DoorWay,
		Windowd
	}

	public enum Windows {
		MediumSquare,
		LargeSquare,
		MediumRectangle,
		LargeRectangle

	}

	void Start () 
	{
		zLocal = transform.forward;
		xLocal = transform.right;
		mesh = GetComponent<MeshFilter>().mesh;
		col = GetComponent<MeshCollider>();

		rot = transform.TransformDirection(transform.localEulerAngles);
		if (rot.y == 0 || rot.y == 90 || rot.y == 180 || rot.y == 270 || rot.y == 360 || rot.y == -90 || rot.y == -180 || rot.y == -270 || rot.y == -360)
		{
			diaganol = false;
		}else// if (rot.y == 45 || rot.y == 135 || rot.y == 225 || rot.y == 315 || rot.y == -45 || rot.y == -135 || rot.y == -225 || rot.y == -315)
		{
			diaganol = true;
		}

		vertices.Clear();
		triangles.Clear();
		triangles.Add(new List<int>());
		triangles.Add(new List<int>());
		triangles.Add(new List<int>());
		uvs.Clear();
		mesh.Clear();
		MakeVertices(pos.x, pos.y, pos.z);
		mesh.vertices = vertices.ToArray();
		//mesh.triangles = triangles.ToArray();
		mesh.subMeshCount = 3;
		for (int i = 0; i < triangles.Count; i++) {
			mesh.SetTriangles(triangles[i], i);
		}
		mesh.uv = uvs.ToArray();
		col.sharedMesh = mesh;
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		mesh.RecalculateBounds();
		GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
		GetComponent<MeshRenderer>().materials = materials;
	}

	void Update ()
	{
	//print ();
		if (fix)
		{
			MakeMesh();
			fix = false;
		}
	}

	public void MakeMesh ()
	{
		rot = transform.localEulerAngles;
		if (rot.y == 0 || rot.y == 90 || rot.y == 180 || rot.y == 270 || rot.y == 360 || rot.y == -90 || rot.y == -180 || rot.y == -270 || rot.y == -360)
		{
			diaganol = false;
		}else// if (rot.y == 45 || rot.y == 135 || rot.y == 225 || rot.y == 315 || rot.y == -45 || rot.y == -135 || rot.y == -225 || rot.y == -315)
		{
			diaganol = true;
		}

		vertices.Clear();
		triangles.Clear();
		triangles.Add(new List<int>());
		triangles.Add(new List<int>());
		triangles.Add(new List<int>());
		uvs.Clear();
		mesh.Clear();
		MakeVertices(pos.x, pos.y, pos.z);
		mesh.vertices = vertices.ToArray();
		//mesh.triangles = triangles.ToArray();
		mesh.subMeshCount = 3;
		for (int i = 0; i < triangles.Count; i++) {
			mesh.SetTriangles(triangles[i], i);
		}
		mesh.uv = uvs.ToArray();
		col.sharedMesh = mesh;
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		mesh.RecalculateBounds();
		GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
		GetComponent<MeshRenderer>().materials = materials;
	}

	public void Clear () 
	{
		vertices.Clear();
		triangles.Clear();
		mesh.Clear();
		//mesh.vertices = vertices.ToArray();
		//mesh.triangles = triangles.ToArray();
		//col.sharedMesh = mesh;
	}

	void MakeVertices (float x, float y, float z)
	{
		float width = 0.08f;
		float lenght = (float)Math.Sqrt(2) / 2;
		float height = wall_height;
		float doorAndWdindowHeight = 2.2f;
		float diaganolSharpBlend = 0.193137f;
		float diaganolStompBlend = 0.05657f / 2;
		float mesh_blend_left_1 = 0;
		float mesh_blend_middle_1 = 0;
		float mesh_blend_right_1 = 0;
		float mesh_blend_left_2 = 0;
		float mesh_blend_middle_2 = 0;
		float mesh_blend_right_2 = 0;
		Vector3 pos = transform.position;
		if (diaganol)
		{
			/*if (AllWallObjects.walls.ContainsKey(pos + transform.TransformDirection(new Vector3(lenght / 2, 0, lenght / 2))) && AllWallObjects.walls.ContainsKey(pos + transform.TransformDirection(new Vector3(lenght / 2, 0, lenght + (lenght / 2)))) && 
				AllWallObjects.walls.ContainsKey(pos + transform.TransformDirection(new Vector3(-lenght / 2, 0, lenght + (lenght / 2)))) && AllWallObjects.walls.ContainsKey(pos + transform.TransformDirection(new Vector3(-lenght / 2, 0, lenght / 2))))
			{
				if (AllWallObjects.walls.ContainsKey(pos + transform.TransformDirection(new Vector3(lenght, 0, lenght))) && AllWallObjects.walls.ContainsKey(pos + transform.TransformDirection(new Vector3(-lenght, 0, lenght))) &&
					AllWallObjects.walls.ContainsKey(pos + transform.TransformDirection(new Vector3(0, 0, lenght + (lenght / 2)))))
				{
					print ("working");
					mesh_blend_left_1 = -diaganolSharpBlend; mesh_blend_middle_1 = 0; mesh_blend_right_1 = -diaganolSharpBlend;
					if (!update_edges)
					{
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(lenght / 2, 0, lenght / 2))].GetComponent<WallMesh>().update_edges = true;
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(lenght / 2, 0, lenght + (lenght / 2)))].GetComponent<WallMesh>().update_edges = true;
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(-lenght / 2, 0, lenght + (lenght / 2)))].GetComponent<WallMesh>().update_edges = true;
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(-lenght / 2, 0, lenght / 2))].GetComponent<WallMesh>().update_edges = true;
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(lenght, 0, lenght))].GetComponent<WallMesh>().update_edges = true;
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(-lenght, 0, lenght))].GetComponent<WallMesh>().update_edges = true;
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(0, 0, lenght + (lenght / 2)))].GetComponent<WallMesh>().update_edges = true;	
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(lenght / 2, 0, lenght / 2))].GetComponent<WallMesh>().fix = true;
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(lenght / 2, 0, lenght + (lenght / 2)))].GetComponent<WallMesh>().fix = true;
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(-lenght / 2, 0, lenght + (lenght / 2)))].GetComponent<WallMesh>().fix = true;
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(-lenght / 2, 0, lenght / 2))].GetComponent<WallMesh>().fix = true;
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(lenght, 0, lenght))].GetComponent<WallMesh>().fix = true;
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(-lenght, 0, lenght))].GetComponent<WallMesh>().fix = true;
						AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(0, 0, lenght + (lenght / 2)))].GetComponent<WallMesh>().fix = true;
					}
				}
				print ("working");
			}
			if (AllWallObjects.walls.ContainsKey(pos + transform.TransformDirection(new Vector3(lenght / 2, 0, lenght / 2))))
			{
				mesh_blend_left_1 = diaganolSharpBlend; mesh_blend_middle_1 = 0; mesh_blend_right_1 = -diaganolSharpBlend;
				if (!update_edges) {
					AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(lenght / 2, 0, lenght / 2))].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + transform.TransformDirection(new Vector3(lenght / 2, 0, lenght / 2))].GetComponent<WallMesh>().fix = true;
				}
			}
		}else {
			/*if (AllWallObjects.walls.ContainsKey(pos + (transform.right / 2) + -(transform.forward / 2)) && AllWallObjects.walls.ContainsKey(pos + -(transform.right / 2) + -(transform.forward / 2)) &&
				AllWallObjects.walls.ContainsKey(pos + -transform.forward) && )
			{
				
			}elseif (AllWallObjects.walls.ContainsKey(pos + (transform.right / 2) + -(transform.forward / 2)) && AllWallObjects.walls.ContainsKey(pos + -(transform.right / 2) + -(transform.forward / 2)) &&
				AllWallObjects.walls.ContainsKey(pos + -transform.forward))
			{
				mesh_blend_left_1 = width; mesh_blend_middle_1 = 0; mesh_blend_right_1 = width;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + (transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + -(transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + -transform.forward].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + (transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().fix = true;
					AllWallObjects.walls[pos + -(transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().fix = true;
					AllWallObjects.walls[pos + -transform.forward].GetComponent<WallMesh>().fix = true;
				}
			}else if (AllWallObjects.walls.ContainsKey(pos + (transform.right / 2) + -(transform.forward / 2)) && AllWallObjects.walls.ContainsKey(pos + -transform.forward))
			{
				mesh_blend_left_1 = 0; mesh_blend_middle_1 = width / 2; mesh_blend_right_1 = width;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + (transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + -transform.forward].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + (transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().fix = true;
					AllWallObjects.walls[pos + -transform.forward].GetComponent<WallMesh>().fix = true;
				}
			}else if (AllWallObjects.walls.ContainsKey(pos + -(transform.right / 2) + -(transform.forward / 2)) && AllWallObjects.walls.ContainsKey(pos + -transform.forward))
			{
				mesh_blend_left_1 = width; mesh_blend_middle_1 = width / 2; mesh_blend_right_1 = 0;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + -(transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + -transform.forward].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + -(transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().fix = true;
					AllWallObjects.walls[pos + -transform.forward].GetComponent<WallMesh>().fix = true;
				}
			}else if (AllWallObjects.walls.ContainsKey(pos + (transform.right / 2) + -(transform.forward / 2)) && AllWallObjects.walls.ContainsKey(pos + -(transform.right / 2) + -(transform.forward / 2)))
			{
				mesh_blend_left_1 = width; mesh_blend_middle_1 = -width; mesh_blend_right_1 = width;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + (transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + -(transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + (transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().fix = true;
					AllWallObjects.walls[pos + -(transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().fix = true;
				}
			}else if (AllWallObjects.walls.ContainsKey(pos + (transform.right / 2) + -(transform.forward / 2)))
			{
				mesh_blend_left_1 = -width; mesh_blend_middle_1 = 0; mesh_blend_right_1 = width;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + (transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + (transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().fix = true;
				}
			}else if (AllWallObjects.walls.ContainsKey(pos + -(transform.right / 2) + -(transform.forward / 2)))
			{
				mesh_blend_left_1 = width; mesh_blend_middle_1 = 0; mesh_blend_right_1 = -width;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + -(transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + -(transform.right / 2) + -(transform.forward / 2)].GetComponent<WallMesh>().fix = true;
				}
			}else if (AllWallObjects.walls.ContainsKey(pos + (-transform.right / 2)))
			{
				if (AllWallObjects.walls[pos + (-transform.right / 2)].transform.rotation.y == -45 && 
					AllWallObjects.walls[pos + (-transform.right / 2)].transform.rotation.y == 135)
				{
					mesh_blend_left_1 = diaganolSharpBlend; mesh_blend_middle_1 = 0; mesh_blend_right_1 = -diaganolSharpBlend;
					if (!update_edges)
					{
						AllWallObjects.walls[pos + (-transform.right / 2)].GetComponent<WallMesh>().update_edges = true;
						AllWallObjects.walls[pos + (-transform.right / 2)].GetComponent<WallMesh>().fix = true;
					}
				}
			}else if (AllWallObjects.walls.ContainsKey(pos + -transform.forward))
			{
				mesh_blend_left_1 = 0; mesh_blend_middle_1 = 0; mesh_blend_right_1 = 0;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + -transform.forward].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + -transform.forward].GetComponent<WallMesh>().fix = true;
				}
			}
			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if (AllWallObjects.walls.ContainsKey(pos + (transform.right / 2) + (transform.forward / 2)) && AllWallObjects.walls.ContainsKey(pos + -(transform.right / 2) + (transform.forward / 2)) &&
				AllWallObjects.walls.ContainsKey(pos + transform.forward))
			{
				mesh_blend_left_2 = -width; mesh_blend_middle_2 = 0; mesh_blend_right_2 = -width;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + (transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + -(transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + transform.forward].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + (transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().fix = true;
					AllWallObjects.walls[pos + -(transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().fix = true;
					AllWallObjects.walls[pos + transform.forward].GetComponent<WallMesh>().fix = true;
				}
			}else if (AllWallObjects.walls.ContainsKey(pos + (transform.right / 2) + (transform.forward / 2)) && AllWallObjects.walls.ContainsKey(pos + transform.forward))
			{
				mesh_blend_left_2 = 0; mesh_blend_middle_2 = -width / 2; mesh_blend_right_2 = -width;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + (transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + transform.forward].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + (transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().fix = true;
					AllWallObjects.walls[pos + transform.forward].GetComponent<WallMesh>().fix = true;
				}
			}else if (AllWallObjects.walls.ContainsKey(pos + -(transform.right / 2) + (transform.forward / 2)) && AllWallObjects.walls.ContainsKey(pos + transform.forward))
			{
				mesh_blend_left_2 = -width; mesh_blend_middle_2 = -width / 2; mesh_blend_right_2 = 0;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + -(transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + transform.forward].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + -(transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().fix = true;
					AllWallObjects.walls[pos + transform.forward].GetComponent<WallMesh>().fix = true;
				}
			}else if (AllWallObjects.walls.ContainsKey(pos + (transform.right / 2) + (transform.forward / 2)) && AllWallObjects.walls.ContainsKey(pos + -(transform.right / 2) + (transform.forward / 2)))
			{
				mesh_blend_left_2 = -width; mesh_blend_middle_2 = width; mesh_blend_right_2 = -width;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + (transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + -(transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + (transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().fix = true;
					AllWallObjects.walls[pos + -(transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().fix = true;
				}
			}else if (AllWallObjects.walls.ContainsKey(pos + (transform.right / 2) + (transform.forward / 2)))
			{
				mesh_blend_left_2 = width; mesh_blend_middle_2 = 0; mesh_blend_right_2 = -width;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + (transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + (transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().fix = true;
				}
			}else if (AllWallObjects.walls.ContainsKey(pos + -(transform.right / 2) + (transform.forward / 2)))
			{
				mesh_blend_left_2 = -width; mesh_blend_middle_2 = 0; mesh_blend_right_2 = width;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + -(transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + -(transform.right / 2) + (transform.forward / 2)].GetComponent<WallMesh>().fix = true;
				}
			}else if (AllWallObjects.walls.ContainsKey(pos + transform.forward))
			{
				mesh_blend_left_2 = 0; mesh_blend_middle_2 = 0; mesh_blend_right_2 = 0;
				if (!update_edges)
				{
					AllWallObjects.walls[pos + transform.forward].GetComponent<WallMesh>().update_edges = true;
					AllWallObjects.walls[pos + transform.forward].GetComponent<WallMesh>().fix = true;
				}
			}
		}
		switch (type) {
		case Type.CleanWall:
			if (diaganol)
			{
				vertices.Add(new Vector3(x - width, y + height, z - lenght + mesh_blend_left_1 ));
				vertices.Add(new Vector3(x + width, y + height, z - lenght + mesh_blend_right_1));
				vertices.Add(new Vector3(x + width, y,          z - lenght + mesh_blend_right_1));
				vertices.Add(new Vector3(x - width, y,          z - lenght + mesh_blend_left_1 ));
				MakeTriangles (2, true);//back

				vertices.Add(new Vector3(x + width, y + height, z + lenght + mesh_blend_right_2));
				vertices.Add(new Vector3(x - width, y + height, z + lenght + mesh_blend_left_2 ));
				vertices.Add(new Vector3(x - width, y,          z + lenght + mesh_blend_left_2 ));
				vertices.Add(new Vector3(x + width, y,          z + lenght + mesh_blend_right_2));
				MakeTriangles (1, true);//front

				vertices.Add(new Vector3(x - width, y + height, z + lenght + mesh_blend_left_2 ));
				vertices.Add(new Vector3(x - width, y + height, z - lenght + mesh_blend_left_1 ));
				vertices.Add(new Vector3(x - width, y,          z - lenght + mesh_blend_left_1 ));
				vertices.Add(new Vector3(x - width, y,          z + lenght + mesh_blend_left_2 ));
				MakeTriangles (1, false);//left

				vertices.Add(new Vector3(x + width, y + height, z - lenght + mesh_blend_right_1));
				vertices.Add(new Vector3(x + width, y + height, z + lenght + mesh_blend_right_2));
				vertices.Add(new Vector3(x + width, y,          z + lenght + mesh_blend_right_2));
				vertices.Add(new Vector3(x + width, y,          z - lenght + mesh_blend_right_1));
				MakeTriangles (2, false);//right

				vertices.Add(new Vector3(x - width, y + height, z + lenght + mesh_blend_left_2  ));
				vertices.Add(new Vector3(x, y + height,         z + lenght + mesh_blend_middle_2));
				vertices.Add(new Vector3(x, y + height,         z - lenght + mesh_blend_middle_1));
				vertices.Add(new Vector3(x - width, y + height, z - lenght + mesh_blend_left_1  ));
				MakeTriangles (0, true);//top1

				vertices.Add(new Vector3(x, y + height,         z + lenght + mesh_blend_middle_2));
				vertices.Add(new Vector3(x + width, y + height, z + lenght + mesh_blend_right_2 ));
				vertices.Add(new Vector3(x + width, y + height, z - lenght + mesh_blend_right_1 ));
				vertices.Add(new Vector3(x, y + height,         z - lenght + mesh_blend_middle_1));
				MakeTriangles (0, true);//top2
			}else {

				vertices.Add(new Vector3(x - width, y + height, z - 0.5f + mesh_blend_left_1 ));
				vertices.Add(new Vector3(x + width, y + height, z - 0.5f + mesh_blend_right_1));
				vertices.Add(new Vector3(x + width, y,          z - 0.5f + mesh_blend_right_1));
				vertices.Add(new Vector3(x - width, y,          z - 0.5f + mesh_blend_left_1 ));
				MakeTriangles (2, true);//back

				vertices.Add(new Vector3(x + width, y + height, z + 0.5f + mesh_blend_right_2));
				vertices.Add(new Vector3(x - width, y + height, z + 0.5f + mesh_blend_left_2 ));
				vertices.Add(new Vector3(x - width, y,          z + 0.5f + mesh_blend_left_2 ));
				vertices.Add(new Vector3(x + width, y,          z + 0.5f + mesh_blend_right_2));
				MakeTriangles (1, true);//front

				vertices.Add(new Vector3(x - width, y + height, z + 0.5f + mesh_blend_left_2 ));
				vertices.Add(new Vector3(x - width, y + height, z - 0.5f + mesh_blend_left_1 ));
				vertices.Add(new Vector3(x - width, y,          z - 0.5f + mesh_blend_left_1 ));
				vertices.Add(new Vector3(x - width, y,          z + 0.5f + mesh_blend_left_2 ));
				MakeTriangles (1, false);//left

				vertices.Add(new Vector3(x + width, y + height, z - 0.5f + mesh_blend_right_1));
				vertices.Add(new Vector3(x + width, y + height, z + 0.5f + mesh_blend_right_2));
				vertices.Add(new Vector3(x + width, y,          z + 0.5f + mesh_blend_right_2));
				vertices.Add(new Vector3(x + width, y,          z - 0.5f + mesh_blend_right_1));
				MakeTriangles (2, false);//right

				vertices.Add(new Vector3(x - width, y + height, z + 0.5f + mesh_blend_left_2  ));
				vertices.Add(new Vector3(x, y + height,         z + 0.5f + mesh_blend_middle_2));
				vertices.Add(new Vector3(x, y + height,         z - 0.5f + mesh_blend_middle_1));
				vertices.Add(new Vector3(x - width, y + height, z - 0.5f + mesh_blend_left_1  ));
				MakeTriangles (0, true);//top 1

				vertices.Add(new Vector3(x, y + height,         z + 0.5f + mesh_blend_middle_2));
				vertices.Add(new Vector3(x + width, y + height, z + 0.5f + mesh_blend_right_2 ));
				vertices.Add(new Vector3(x + width, y + height, z - 0.5f + mesh_blend_right_1 ));
				vertices.Add(new Vector3(x, y + height,         z - 0.5f + mesh_blend_middle_1));
				MakeTriangles (0, true);//top 2
			}
			break;
		case Type.DoorWay:
			if (diaganol)
			{
				vertices.Add(new Vector3(x - width, y + height, z - lenght));
				vertices.Add(new Vector3(x + width, y + height, z - lenght));
				vertices.Add(new Vector3(x + width, y, z - lenght));
				vertices.Add(new Vector3(x - width, y, z - lenght));
				MakeTriangles (2, true);//back

				vertices.Add(new Vector3(x + width, y + height, z + lenght));
				vertices.Add(new Vector3(x - width, y + height, z + lenght));
				vertices.Add(new Vector3(x - width, y, z + lenght));
				vertices.Add(new Vector3(x + width, y, z + lenght));
				MakeTriangles (1, true);//front

				vertices.Add(new Vector3(x - width, y + height, z - 0.4f));
				vertices.Add(new Vector3(x - width, y + height, z - lenght));
				vertices.Add(new Vector3(x - width, y, z - lenght));
				vertices.Add(new Vector3(x - width, y, z - 0.4f));
				MakeTriangles (1, false);//left 1

				vertices.Add(new Vector3(x - width, y + height, z + lenght));
				vertices.Add(new Vector3(x - width, y + height, z + 0.4f));
				vertices.Add(new Vector3(x - width, y, z + 0.4f));
				vertices.Add(new Vector3(x - width, y, z + lenght));
				MakeTriangles (1, false);//left 2

				vertices.Add(new Vector3(x - width, y + height, z + 0.4f));
				vertices.Add(new Vector3(x - width, y + height, z - 0.4f));
				vertices.Add(new Vector3(x - width, y + 2f, z - 0.4f));
				vertices.Add(new Vector3(x - width, y + 2f, z + 0.4f));
				MakeTriangles (1, false);//left 3

				vertices.Add(new Vector3(x + width, y + height, z - lenght));
				vertices.Add(new Vector3(x + width, y + height, z - 0.4f));
				vertices.Add(new Vector3(x + width, y, z - 0.4f));
				vertices.Add(new Vector3(x + width, y, z - lenght));
				MakeTriangles (2, false);//right 1

				vertices.Add(new Vector3(x + width, y + height, z + 0.4f));
				vertices.Add(new Vector3(x + width, y + height, z + lenght));
				vertices.Add(new Vector3(x + width, y, z + lenght));
				vertices.Add(new Vector3(x + width, y, z + 0.4f));
				MakeTriangles (2, false);//right 2

				vertices.Add(new Vector3(x + width, y + height, z - 0.4f));
				vertices.Add(new Vector3(x + width, y + height, z + 0.4f));
				vertices.Add(new Vector3(x + width, y + doorAndWdindowHeight, z + 0.4f));
				vertices.Add(new Vector3(x + width, y + doorAndWdindowHeight, z - 0.4f));
				MakeTriangles (2, false);//right 3

				vertices.Add(new Vector3(x - width, y + height, z + lenght));
				vertices.Add(new Vector3(x + width, y + height, z + lenght));
				vertices.Add(new Vector3(x + width, y + height, z - lenght));
				vertices.Add(new Vector3(x - width, y + height, z - lenght));
				MakeTriangles (0, true);//top
			}else {
				vertices.Add(new Vector3(x - width, y + height, z - 0.5f));
				vertices.Add(new Vector3(x + width, y + height, z - 0.5f));
				vertices.Add(new Vector3(x + width, y, z - 0.5f));
				vertices.Add(new Vector3(x - width, y, z - 0.5f));
				MakeTriangles (2, true);//back

				vertices.Add(new Vector3(x + width, y + height, z + 0.5f));
				vertices.Add(new Vector3(x - width, y + height, z + 0.5f));
				vertices.Add(new Vector3(x - width, y, z + 0.5f));
				vertices.Add(new Vector3(x + width, y, z + 0.5f));
				MakeTriangles (1, true);//front

				vertices.Add(new Vector3(x - width, y + height, z - 0.4f));
				vertices.Add(new Vector3(x - width, y + height, z - 0.5f));
				vertices.Add(new Vector3(x - width, y, z - 0.5f));
				vertices.Add(new Vector3(x - width, y, z - 0.4f));
				MakeTriangles (1, false);//left 1

				vertices.Add(new Vector3(x - width, y + height, z + 0.5f));
				vertices.Add(new Vector3(x - width, y + height, z + 0.4f));
				vertices.Add(new Vector3(x - width, y, z + 0.4f));
				vertices.Add(new Vector3(x - width, y, z + 0.5f));
				MakeTriangles (1, false);//left 2

				vertices.Add(new Vector3(x - width, y + height, z + 0.4f));
				vertices.Add(new Vector3(x - width, y + height, z - 0.4f));
				vertices.Add(new Vector3(x - width, y + 2f, z - 0.4f));
				vertices.Add(new Vector3(x - width, y + 2f, z + 0.4f));
				MakeTriangles (1, false);//left 3

				vertices.Add(new Vector3(x + width, y + height, z - 0.5f));
				vertices.Add(new Vector3(x + width, y + height, z - 0.4f));
				vertices.Add(new Vector3(x + width, y, z - 0.4f));
				vertices.Add(new Vector3(x + width, y, z - 0.5f));
				MakeTriangles (2, false);//right 1

				vertices.Add(new Vector3(x + width, y + height, z + 0.4f));
				vertices.Add(new Vector3(x + width, y + height, z + 0.5f));
				vertices.Add(new Vector3(x + width, y, z + 0.5f));
				vertices.Add(new Vector3(x + width, y, z + 0.4f));
				MakeTriangles (2, false);//right 2

				vertices.Add(new Vector3(x + width, y + height, z - 0.4f));
				vertices.Add(new Vector3(x + width, y + height, z + 0.4f));
				vertices.Add(new Vector3(x + width, y + doorAndWdindowHeight, z + 0.4f));
				vertices.Add(new Vector3(x + width, y + doorAndWdindowHeight, z - 0.4f));
				MakeTriangles (2, false);//right 3

				vertices.Add(new Vector3(x - width, y + height, z + 0.5f));
				vertices.Add(new Vector3(x + width, y + height, z + 0.5f));
				vertices.Add(new Vector3(x + width, y + height, z - 0.5f));
				vertices.Add(new Vector3(x - width, y + height, z - 0.5f));
				MakeTriangles (0, true);//top
			}
			break;
		}
		update_edges = false;
	}

	void MakeTriangles (int subMeshCount, bool topInside)
	{
		float uvLength = (subMeshCount == 0 && !topInside || subMeshCount != 0 && topInside)?0.16f:(subMeshCount == 0 && topInside)?0.08f:1;
		float uvHeight = (subMeshCount == 0 && !topInside || subMeshCount != 0)?wall_height:(subMeshCount == 0 && topInside)?1:1;
		triangles[subMeshCount].Add(vertices.Count - 4);
		triangles[subMeshCount].Add(vertices.Count - 3);
		triangles[subMeshCount].Add(vertices.Count - 1);
		triangles[subMeshCount].Add(vertices.Count - 3);
		triangles[subMeshCount].Add(vertices.Count - 2);
		triangles[subMeshCount].Add(vertices.Count - 1);

		uvs.Add(new Vector2(0, uvHeight));
		uvs.Add(new Vector2(uvLength, uvHeight));
		uvs.Add(new Vector2(uvLength, 0));
		uvs.Add(new Vector2(0, 0));

		return;
		switch (type) {
		case Type.CleanWall:
			if (subMeshCount == 0)
			{
				uvs.Add(new Vector2(0, wall_height));
				uvs.Add(new Vector2(0.16f, wall_height));
				uvs.Add(new Vector2(0.16f, 0));
				uvs.Add(new Vector2(0, 0));
			}else {
				uvs.Add(new Vector2(0, wall_height));
				uvs.Add(new Vector2(1, wall_height));
				uvs.Add(new Vector2(1, 0));
				uvs.Add(new Vector2(0, 0));
			}
			break;
		case Type.DoorWay:
			if (subMeshCount == 0)
			{
				uvs.Add(new Vector2(0, wall_height));
				uvs.Add(new Vector2(0.16f, wall_height));
				uvs.Add(new Vector2(0.16f, 0));
				uvs.Add(new Vector2(0, 0));
			}else {
				uvs.Add(new Vector2(0, wall_height));
				uvs.Add(new Vector2(1, wall_height));
				uvs.Add(new Vector2(1, 0));
				uvs.Add(new Vector2(0, 0));
			}
			break;
		case Type.Windowd:
			break;
		}
	}

	void OnDrawGizmos ()
	{
		//Gizmos.DrawSphere(transform.position + transform.TransformDirection(new Vector3((float)Math.Sqrt(2) / 4, 0, (float)Math.Sqrt(2) / 4)), 0.05f);
	}
}
 */