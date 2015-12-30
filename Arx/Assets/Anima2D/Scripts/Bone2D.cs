using UnityEngine;
using System;
using System.Collections;

namespace Anima2D 
{
	public class Bone2D : MonoBehaviour
	{
		public Color color = Color.white;

		public Ik2D attachedIK { get; set; }

		[SerializeField][HideInInspector] Bone2D mChild;
		public Bone2D child
		{
			get {
				if(mChild &&
				   (mChild == this || mChild.transform.parent != transform))
				{
					return null;
				}

				return mChild;
			}

			set {
				mChild = value;
				mChild = child;
			}
		}

		public Vector3 localEndPosition
		{
			get {
				return Vector3.right*localLength;
			}
		}

		public Vector3 endPosition
		{
			get {
				return transform.TransformPoint(localEndPosition);
			}
		}

		[HideInInspector][SerializeField]float mLength = 1f;
		public float localLength {
			get {
				if(child)
				{
					Vector3 childPosition = transform.InverseTransformPoint(child.transform.position);
					mLength = Mathf.Clamp(childPosition.x,0f,childPosition.x);
				}

				return mLength;
			}
			set {
				if(!child)
				{
					mLength = value;
				}
			}
		}

		public float length {
			get {
				return transform.lossyScale.x * localLength;
			}
		}

		Bone2D mParentBone = null;
		public Bone2D parentBone
		{
			get {
				Transform parentTransform = transform.parent;

				if(!mParentBone)
				{
					if(parentTransform)
					{
						mParentBone = parentTransform.GetComponent<Bone2D>();
					}
				}else if(parentTransform != mParentBone.transform)
				{
					if(parentTransform)
					{
						mParentBone = parentTransform.GetComponent<Bone2D>();
					}else{
						mParentBone = null;
					}
				}
				
				return mParentBone;
			}
		}

		public Bone2D linkedParentBone
		{
			get {
				if(parentBone && parentBone.child == this)
				{
					return parentBone;
				}
				
				return null;
			}
		}
		
		public Bone2D root
		{
			get {
				Bone2D rootBone = this;
				
				while(rootBone.parentBone)
				{
					rootBone = rootBone.parentBone;
				}
				
				return rootBone;
			}
		}

		public Bone2D chainRoot
		{
			get {
				Bone2D chainRoot = this;
				
				while(chainRoot.parentBone && chainRoot.parentBone.child == chainRoot)
				{
					chainRoot = chainRoot.parentBone;
				}
				
				return chainRoot;
			}
		}

		public int chainLength
		{
			get {
				Bone2D chainRoot = this;

				int length = 1;

				while(chainRoot.parentBone && chainRoot.parentBone.child == chainRoot)
				{
					++length;
					chainRoot = chainRoot.parentBone;
				}
				
				return length;
			}
		}

		public static Bone2D GetChainBoneByIndex(Bone2D chainTip, int index)
		{
			if(!chainTip) return null;
			
			Bone2D bone = chainTip;
			
			int chainLength = bone.chainLength;
			
			for(int i = 0; i < chainLength && bone; ++i)
			{
				if(i == index)
				{
					return bone;
				}
				
				if(bone.linkedParentBone)
				{
					bone = bone.parentBone;
				}else{
					return null;
				}
			}
			
			return null;
		}
	}
}
