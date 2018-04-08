using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OcclusionMeshFader : MonoBehaviour {
	public float minAlpha = 0.5f;
	float timeTillFullAlpha = 0.2f;
	List<Material> m_materials = new List<Material> ();
	List<Color> m_initialColors = new List<Color> ();

	void Awake () {
		var renderers = GetComponentsInChildren<Renderer> ();
		foreach (var renderer in renderers) {
			var materials = renderer.materials;
			m_materials.AddRange (materials);
			m_initialColors.AddRange (materials.Select (x => x.color));
		}
	}

	[EditorButton]
	void Debug_ON () {
		Overlapping ();
	}

	float lastTimeOverlapping = -100f;
	public void Overlapping () {
		lastTimeOverlapping = Time.time;
	}

	float m_prevT = 1;
	void Update () {
		var overlapping = Time.time - lastTimeOverlapping < 0.1f;
		float t = Mathf.Clamp01 (m_prevT + Time.deltaTime / timeTillFullAlpha * (overlapping ? -1f : 1f));
		if (t != m_prevT) {
			for (int i = 0; i < m_materials.Count; i++) {
				var alphaedColor = m_initialColors[i];
				alphaedColor.a = minAlpha;
				m_materials[i].color = Color.Lerp (alphaedColor, m_initialColors[i], t);
				if (t != 1f) {
					StandardShaderUtils.ChangeRenderMode (m_materials[i], StandardShaderUtils.BlendMode.Fade);
				} else {
					StandardShaderUtils.ChangeRenderMode (m_materials[i], StandardShaderUtils.BlendMode.Opaque);
				}
			}

			//			int j = 0;
			//			var renderers = GetComponentsInChildren<Renderer> ();
			//			foreach (var renderer in renderers) {
			//				renderer.material = m_materials [j];
			//			}
		}
		m_prevT = t;
	}
}