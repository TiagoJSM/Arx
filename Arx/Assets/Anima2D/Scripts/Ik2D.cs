using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D 
{
	public abstract class Ik2D : MonoBehaviour
	{
		[SerializeField] Bone2D m_Target;
		[SerializeField] int m_NumBones = 0;
		[SerializeField] float m_Weight = 1f;

		public IkSolver2D solver { get { return GetSolver(); } }

		public Bone2D target
		{
			get { return m_Target; }
			set {
				m_Target = value;
				InitializeSolver();
			}
		}
		
		public int numBones
		{
			get { return ValidateNumBones(m_NumBones); }
			set {
				int l_numBones = ValidateNumBones(value);

				if(l_numBones != m_NumBones)
				{
					m_NumBones = l_numBones;
					InitializeSolver();
				}
			}
		}
		
		public float weight
		{
			get { return m_Weight; }
			set { m_Weight = value; }
		}

		void OnDrawGizmos()
		{
			Gizmos.matrix = transform.localToWorldMatrix;

			if(enabled && target && numBones > 0)
			{
				Gizmos.DrawIcon(transform.position,"ikGoal");
			}else{
				Gizmos.DrawIcon(transform.position,"ikGoalDisabled");
			}
		}

		void OnValidate()
		{
			Validate();
		}

		void Start()
		{
			OnStart();
		}

		void Update()
		{
			SetAttachedIK(this);

			OnUpdate();
		}

		void LateUpdate()
		{
			OnLateUpdate();

			if(Application.isPlaying)
			{
				UpdateIK();
			}
		}

		void SetAttachedIK(Ik2D ik2D)
		{
			for (int i = 0; i < solver.solverPoses.Count; i++)
			{
				IkSolver2D.SolverPose pose = solver.solverPoses[i];
				
				if(pose.bone)
				{
					pose.bone.attachedIK = ik2D;
				}
			}
		}

		public void UpdateIK()
		{
			OnIkUpdate();

			solver.Update();
		}

		protected virtual void OnIkUpdate()
		{
			solver.weight = weight;
			solver.targetPosition = transform.position;
		}

		void InitializeSolver()
		{
			Bone2D rootBone = Bone2D.GetChainBoneByIndex(target, numBones-1);

			SetAttachedIK(null);

			solver.Initialize(rootBone,numBones);
		}

		protected virtual int ValidateNumBones(int numBones) { return numBones; }
		protected virtual void Validate() {}
		protected virtual void OnStart() {}
		protected virtual void OnUpdate() {}
		protected virtual void OnLateUpdate() {}

		protected abstract IkSolver2D GetSolver();
	}
}
