using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperImage : Graphic
{

	public RectTransform hole;
	public Vector4 margin;

	protected override void OnPopulateMesh (VertexHelper vh)
	{
		if (hole == null) {
			base.OnPopulateMesh (vh);
			return;
		}

		var r0 = GetPixelAdjustedRect();
		var r1 = hole.rect;

		var v0 = new Vector4(r0.x, r0.y, r0.x + r0.width, r0.y + r0.height);
		var v1 = new Vector4(r1.x + margin.x, r1.y + margin.w, r1.x + r1.width - margin.z, r1.y + r1.height - margin.y);

		Color32 color32 = color;
		vh.Clear();

		vh.AddVert(new Vector3(v0.x, v0.y), color32, new Vector2(0f, 0f));
		vh.AddVert(new Vector3(v0.x, v0.w), color32, new Vector2(0f, 1f));
		vh.AddVert(new Vector3(v0.z, v0.w), color32, new Vector2(1f, 1f));
		vh.AddVert(new Vector3(v0.z, v0.y), color32, new Vector2(1f, 0f));

		vh.AddVert(new Vector3(v1.x, v1.y), color32, new Vector2(0f, 0f));
		vh.AddVert(new Vector3(v1.x, v1.w), color32, new Vector2(0f, 1f));
		vh.AddVert(new Vector3(v1.z, v1.w), color32, new Vector2(1f, 1f));
		vh.AddVert(new Vector3(v1.z, v1.y), color32, new Vector2(1f, 0f));

		vh.AddTriangle(0, 4, 7);
		vh.AddTriangle(7, 3, 0);

		vh.AddTriangle(7, 6, 2);
		vh.AddTriangle(2, 3, 7);

		vh.AddTriangle(5, 1, 2);
		vh.AddTriangle(2, 6, 5);

		vh.AddTriangle(0, 1, 5);
		vh.AddTriangle(5, 4, 0);

	}

	[ContextMenu("Update Vertices")]
	public void UpdateVertices ()
	{
		SetVerticesDirty ();
	}

	public void UpdateVertices (RectTransform hole, Vector4 margin)
	{
		this.hole = hole;
		this.margin = margin;
		SetVerticesDirty ();
	}
}
