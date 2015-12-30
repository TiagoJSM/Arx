using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D
{
	[Serializable]
	public abstract class IkSolver2D
	{
		[Serializable]
		public class SolverPose 
		{
			public Bone2D bone;
			public Vector3 solverPosition = Vector3.zero;
			public Quaternion solverRotation = Quaternion.identity;
			public Quaternion defaultLocalRotation = Quaternion.identity;

			public void StoreDefaultPose()
			{
				defaultLocalRotation = bone.transform.localRotation;
			}

			public void RestoreDefaultPose()
			{
				if(bone)
				{
					bone.transform.localRotation = defaultLocalRotation;
				}
			}
		}

		[SerializeField] Bone2D m_RootBone;
		[SerializeField] List<SolverPose> m_SolverPoses = new List<SolverPose>();
		[SerializeField] float m_Weight = 1f;

		public Bone2D rootBone { get { return m_RootBone; } private set { m_RootBone = value; } }
		public List<SolverPose> solverPoses { get { return m_SolverPoses; } }
		public float weight { 
			get { return m_Weight; } 
			set { 
				m_Weight = Mathf.Clamp01(value);
			}
		}

		public Vector3 targetPosition;

		public void Initialize(Bone2D _rootBone, int numChilds)
		{
			rootBone = _rootBone;

			Bone2D bone = rootBone;
			solverPoses.Clear();

			for(int i = 0; i < numChilds; ++i)
			{
				if(bone)
				{
					SolverPose solverPose = new SolverPose();
					solverPose.bone = bone;
					solverPoses.Add(solverPose);
					bone = bone.child;
				}
			}

			StoreDefaultPoses();
		}

		public void Update()
		{
			if(weight > 0f)
			{
				DoSolverUpdate();
				UpdateBones();
			}
		}

		public void StoreDefaultPoses()
		{
			for (int i = 0; i < solverPoses.Count; i++)
			{
				SolverPose pose = solverPoses [i];
				
				if(pose != null)
				{
					pose.StoreDefaultPose();
				}
			}
		}

		public void RestoreDefaultPoses()
		{
			for (int i = 0; i < solverPoses.Count; i++)
			{
				SolverPose pose = solverPoses [i];
				
				if(pose != null)
				{
					pose.RestoreDefaultPose();
				}
			}
		}

		void UpdateBones()
		{
			for(int i = 0; i < solverPoses.Count; ++i)
			{
				SolverPose solverPose = solverPoses[i];
				
				if(solverPose != null && solverPose.bone)
				{
					if(weight == 1f)
					{
						solverPose.bone.transform.localRotation = solverPose.solverRotation;
					}else{
						solverPose.bone.transform.localRotation = Quaternion.Slerp(solverPose.bone.transform.localRotation,
						                                                           solverPose.solverRotation,
						                                                           weight);
					}
				}
			}
		}

		protected abstract void DoSolverUpdate();
	}
}
