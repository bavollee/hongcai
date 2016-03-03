// ----------------------------------------------------------------------------------
//
// FXMaker
// Modify by ismoon - 2012 - ismoonto@gmail.com
//
// reference source - http://wiki.unity3d.com/index.php/MeleeWeaponTrail
//
// ----------------------------------------------------------------------------------

// #define USE_INTERPOLATION
//
// By Anomalous Underdog, 2011
//
// Based on code made by Forest Johnson (Yoggy) and xyber
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
// [RequireComponent (typeof(MeshRenderer))]
 
public class NcTrailTexture : NcEffectBehaviour
{
	// Attribute ------------------------------------------------------------------------
	public		bool		m_bEmit					= true;
	public		bool		Emit { set{m_bEmit = value;} }

	public		float		m_fEmitTime				= 0.00f;
	public		Material	m_Material;
	public		float		m_fLifeTime				= 1.00f;
	public		Color[]		m_Colors;
	public		float[]		m_Sizes;
	public		float		m_fMinVertexDistance	= 0.10f;
	public		float		m_fMaxVertexDistance	= 10.00f;
	public		float		m_fMaxAngle				= 3.00f;
	public		bool		m_bAutoDestruct			= false;

	public		Transform	_base;
	public		Transform	_tip;

	protected	List<Point> m_Points				= new List<Point>();

	protected	GameObject	m_TrialObject;
	protected	Mesh		m_TrailMesh;
	protected	Vector3		m_LastPosition;
	protected	Vector3		m_LastCameraPosition1;
	protected	Vector3		m_LastCameraPosition2;
	protected	bool		m_bLastFrameEmit		= true;

#if USE_INTERPOLATION
	public		int			m_nSubdivisions			= 4;
	protected	List<Point>	m_SmoothedPoints		= new List<Point>();
#endif

 	public class Point
	{
		public	float		timeCreated		= 0.00f;
		public	Vector3		basePosition;
		public	Vector3		tipPosition;
		public	bool		lineBreak		= false;
	}

	// Property -------------------------------------------------------------------------
	// Event Function -------------------------------------------------------------------
	void Start()
	{
		if (renderer == null || renderer.material)
		{
			enabled = false;
			return;
		}

		_base	= transform.parent;
		_tip	= transform;

		m_LastPosition = transform.position;
		m_TrialObject = new GameObject("Trail");
		m_TrialObject.transform.position = Vector3.zero;
		m_TrialObject.transform.rotation = Quaternion.identity;
		m_TrialObject.transform.localScale = Vector3.one;
		m_TrialObject.AddComponent(typeof(MeshFilter));
		m_TrialObject.AddComponent(typeof(MeshRenderer));
// 		m_TrialObject.AddComponent<Nc>();
		m_TrialObject.renderer.material = renderer.material;
		m_TrailMesh = m_TrialObject.GetComponent<MeshFilter>().mesh;
		CreateEditorGameObject(m_TrialObject);
	}

	void Update()
	{
		if (renderer == null || renderer.material)
		{
			enabled = false;
			return;
		}

		if (m_bEmit && m_fEmitTime != 0)
		{
			m_fEmitTime -= Time.deltaTime;
			if (m_fEmitTime <= 0) m_bEmit = false;
		}
 
		if (!m_bEmit && m_Points.Count == 0 && m_bAutoDestruct)
		{
			Destroy(m_TrialObject);
			Destroy(gameObject);
		}
 
		// early out if there is no camera
		if (!Camera.main) return;
 
		// if we have moved enough, create a new vertex and make sure we rebuild the mesh
		float theDistance = (m_LastPosition - transform.position).magnitude;
		if (m_bEmit)
		{
			if (theDistance > m_fMinVertexDistance)
			{
				bool make = false;
				if (m_Points.Count < 3)
				{
					make = true;
				}
				else
				{
					//Vector3 l1 = m_Points[m_Points.Count - 2].basePosition - m_Points[m_Points.Count - 3].basePosition;
					//Vector3 l2 = m_Points[m_Points.Count - 1].basePosition - m_Points[m_Points.Count - 2].basePosition;
					Vector3 l1 = m_Points[m_Points.Count - 2].tipPosition - m_Points[m_Points.Count - 3].tipPosition;
					Vector3 l2 = m_Points[m_Points.Count - 1].tipPosition - m_Points[m_Points.Count - 2].tipPosition;
					if (Vector3.Angle(l1, l2) > m_fMaxAngle || theDistance > m_fMaxVertexDistance) make = true;
				}
 
				if (make)
				{
					Point p = new Point();
					p.basePosition = _base.position;
					p.tipPosition = _tip.position;
					p.timeCreated = Time.time;
					m_Points.Add(p);
					m_LastPosition = transform.position;
 
 
#if USE_INTERPOLATION
					if (m_Points.Count == 1)
					{
						m_SmoothedPoints.Add(p);
					}
					else if (m_Points.Count > 1)
					{
						// add 1+m_nSubdivisions for every possible pair in the m_Points
						for (int n = 0; n < 1+m_nSubdivisions; ++n)
							m_SmoothedPoints.Add(p);
					}
 
					// we use 4 control points for the smoothing
					if (m_Points.Count >= 4)
					{
						Vector3[] tipPoints = new Vector3[4];
						tipPoints[0] = m_Points[m_Points.Count - 4].tipPosition;
						tipPoints[1] = m_Points[m_Points.Count - 3].tipPosition;
						tipPoints[2] = m_Points[m_Points.Count - 2].tipPosition;
						tipPoints[3] = m_Points[m_Points.Count - 1].tipPosition;
 
						//IEnumerable<Vector3> smoothTip = Interpolate.NewBezier(Interpolate.Ease(Interpolate.EaseType.Linear), tipPoints, m_nSubdivisions);
						IEnumerable<Vector3> smoothTip = NcInterpolate.NewCatmullRom(tipPoints, m_nSubdivisions, false);
 
						Vector3[] basePoints = new Vector3[4];
						basePoints[0] = m_Points[m_Points.Count - 4].basePosition;
						basePoints[1] = m_Points[m_Points.Count - 3].basePosition;
						basePoints[2] = m_Points[m_Points.Count - 2].basePosition;
						basePoints[3] = m_Points[m_Points.Count - 1].basePosition;
 
						//IEnumerable<Vector3> smoothBase = Interpolate.NewBezier(Interpolate.Ease(Interpolate.EaseType.Linear), basePoints, m_nSubdivisions);
						IEnumerable<Vector3> smoothBase = NcInterpolate.NewCatmullRom(basePoints, m_nSubdivisions, false);
 
						List<Vector3> smoothTipList = new List<Vector3>(smoothTip);
						List<Vector3> smoothBaseList = new List<Vector3>(smoothBase);
 
						float firstTime = m_Points[m_Points.Count - 4].timeCreated;
						float secondTime = m_Points[m_Points.Count - 1].timeCreated;
 
						//Debug.Log(" smoothTipList.Count: " + smoothTipList.Count);
 
						for (int n = 0; n < smoothTipList.Count; ++n)
						{
 
							int idx = m_SmoothedPoints.Count - (smoothTipList.Count-n);
							// there are moments when the m_SmoothedPoints are lesser
							// than what is required, when elements from it are removed
							if (idx > -1 && idx < m_SmoothedPoints.Count)
							{
								Point sp = new Point();
								sp.basePosition = smoothBaseList[n];
								sp.tipPosition = smoothTipList[n];
								sp.timeCreated = Mathf.Lerp(firstTime, secondTime, (float)n/smoothTipList.Count);
								m_SmoothedPoints[idx] = sp;
							}
							//else
							//{
							//	Debug.LogError(idx + "/" + m_SmoothedPoints.Count);
							//}
						}
					}
#endif
				}
				else
				{
					m_Points[m_Points.Count - 1].basePosition = _base.position;
					m_Points[m_Points.Count - 1].tipPosition = _tip.position;
					//m_Points[m_Points.Count - 1].timeCreated = Time.time;
 
#if USE_INTERPOLATION
					m_SmoothedPoints[m_SmoothedPoints.Count - 1].basePosition = _base.position;
					m_SmoothedPoints[m_SmoothedPoints.Count - 1].tipPosition = _tip.position;
#endif
				}
			}
			else
			{
				if (m_Points.Count > 0)
				{
					m_Points[m_Points.Count - 1].basePosition = _base.position;
					m_Points[m_Points.Count - 1].tipPosition = _tip.position;
					//m_Points[m_Points.Count - 1].timeCreated = Time.time;
				}
 
#if USE_INTERPOLATION
				if (m_SmoothedPoints.Count > 0)
				{
					m_SmoothedPoints[m_SmoothedPoints.Count - 1].basePosition = _base.position;
					m_SmoothedPoints[m_SmoothedPoints.Count - 1].tipPosition = _tip.position;
				}
#endif
			}
		}
 
		if (!m_bEmit && m_bLastFrameEmit && m_Points.Count > 0)
			m_Points[m_Points.Count - 1].lineBreak = true;
 
		m_bLastFrameEmit = m_bEmit;
 
 
 
 
		List<Point> remove = new List<Point>();
		foreach (Point p in m_Points)
		{
			// cull old points first
			if (Time.time - p.timeCreated > m_fLifeTime)
			{
				remove.Add(p);
			}
		}
		foreach (Point p in remove)
		{
			m_Points.Remove(p);
		}
 
#if USE_INTERPOLATION
		remove = new List<Point>();
		foreach (Point p in m_SmoothedPoints)
		{
			// cull old points first
			if (Time.time - p.timeCreated > m_fLifeTime)
			{
				remove.Add(p);
			}
		}
		foreach (Point p in remove)
		{
			m_SmoothedPoints.Remove(p);
		}
#endif
 
 
#if USE_INTERPOLATION
		List<Point> pointsToUse = m_SmoothedPoints;
#else
		List<Point> pointsToUse = m_Points;
#endif
 
		if (pointsToUse.Count > 1)
		{
			Vector3[] newVertices = new Vector3[pointsToUse.Count * 2];
			Vector2[] newUV = new Vector2[pointsToUse.Count * 2];
			int[] newTriangles = new int[(pointsToUse.Count - 1) * 6];
			Color[] newColors = new Color[pointsToUse.Count * 2];
 
			for (int n = 0; n < pointsToUse.Count; ++n)
			{
				Point p = pointsToUse[n];
				float time = (Time.time - p.timeCreated) / m_fLifeTime;
 
				Color color = Color.Lerp(Color.white, Color.clear, time);
				if (m_Colors != null && m_Colors.Length > 0)
				{
					float colorTime = time * (m_Colors.Length - 1);
					float min = Mathf.Floor(colorTime);
					float max = Mathf.Clamp(Mathf.Ceil(colorTime), 1, m_Colors.Length - 1);
					float lerp = Mathf.InverseLerp(min, max, colorTime);
					if (min >= m_Colors.Length) min = m_Colors.Length - 1; if (min < 0) min = 0;
					if (max >= m_Colors.Length) max = m_Colors.Length - 1; if (max < 0) max = 0;
					color = Color.Lerp(m_Colors[(int)min], m_Colors[(int)max], lerp);
				}
 
				float size = 0f;
				if (m_Sizes != null && m_Sizes.Length > 0)
				{
					float sizeTime = time * (m_Sizes.Length - 1);
					float min = Mathf.Floor(sizeTime);
					float max = Mathf.Clamp(Mathf.Ceil(sizeTime), 1, m_Sizes.Length - 1);
					float lerp = Mathf.InverseLerp(min, max, sizeTime);
					if (min >= m_Sizes.Length) min = m_Sizes.Length - 1; if (min < 0) min = 0;
					if (max >= m_Sizes.Length) max = m_Sizes.Length - 1; if (max < 0) max = 0;
					size = Mathf.Lerp(m_Sizes[(int)min], m_Sizes[(int)max], lerp);
				}
 
				Vector3 lineDirection = p.tipPosition - p.basePosition;
 
				newVertices[n * 2] = p.basePosition - (lineDirection * (size * 0.5f));
				newVertices[(n * 2) + 1] = p.tipPosition + (lineDirection * (size * 0.5f));
 
				newColors[n * 2] = newColors[(n * 2) + 1] = color;
 
				float uvRatio = (float)n/pointsToUse.Count;
				newUV[n * 2] = new Vector2(uvRatio, 0);
				newUV[(n * 2) + 1] = new Vector2(uvRatio, 1);
 
				if (n > 0 /*&& !pointsToUse[n - 1].lineBreak*/)
				{
					newTriangles[(n - 1) * 6] = (n * 2) - 2;
					newTriangles[((n - 1) * 6) + 1] = (n * 2) - 1;
					newTriangles[((n - 1) * 6) + 2] = n * 2;
 
					newTriangles[((n - 1) * 6) + 3] = (n * 2) + 1;
					newTriangles[((n - 1) * 6) + 4] = n * 2;
					newTriangles[((n - 1) * 6) + 5] = (n * 2) - 1;
				}
			}
 
			m_TrailMesh.Clear();
			m_TrailMesh.vertices = newVertices;
			m_TrailMesh.colors = newColors;
			m_TrailMesh.uv = newUV;
			m_TrailMesh.triangles = newTriangles;
 
		}else{
                    m_TrailMesh.Clear();
 
		}
	}
}
