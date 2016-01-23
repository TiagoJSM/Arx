using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D
{
	public class SpriteMeshInstance : MonoBehaviour
	{
		public SpriteMesh spriteMesh;
		[SortingLayer]
		public string sortingLayer = "Default";
		public int orderInLayer = 0;

		[HideInInspector]
		public List<Bone2D> bones = new List<Bone2D>();

		Renderer mCachedRenderer;
		public Renderer cachedRenderer {
			get {
				if(!mCachedRenderer)
				{
					mCachedRenderer = GetComponent<Renderer>();
				}

				return mCachedRenderer;
			}
		}

		SkinnedMeshRenderer mCachedSkinnedRenderer;
		public SkinnedMeshRenderer cachedSkinnedRenderer {
			get {
				if(!mCachedSkinnedRenderer)
				{
					mCachedSkinnedRenderer = GetComponent<SkinnedMeshRenderer>();
				}
				
				return mCachedSkinnedRenderer;
			}
		}

		void Update()
		{
			UpdateRenderer();
		}

		public void UpdateRenderer()
		{
			if(cachedRenderer)
			{
				cachedRenderer.sortingLayerName = sortingLayer;
				cachedRenderer.sortingOrder = orderInLayer;
			}
		}
	}
}
