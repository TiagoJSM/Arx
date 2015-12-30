using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Anima2D 
{
	public static class IkUtils
	{
		public static void InitializeIk2D(SerializedObject ikSO)
		{
			SerializedProperty targetProp = ikSO.FindProperty("m_Target");
			SerializedProperty numBonesProp = ikSO.FindProperty("m_NumBones");
			SerializedProperty solverProp = ikSO.FindProperty("m_Solver");
			SerializedProperty solverPosesProp = solverProp.FindPropertyRelative("m_SolverPoses");
			SerializedProperty rootBoneProp = solverProp.FindPropertyRelative("m_RootBone");

			Bone2D targetBone = targetProp.objectReferenceValue as Bone2D;
			Bone2D rootBone = null;

			if(targetBone)
			{
				rootBone = Bone2D.GetChainBoneByIndex(targetBone, numBonesProp.intValue-1);
			}

			for(int i = 0; i < solverPosesProp.arraySize; ++i)
			{
				SerializedProperty poseProp = solverPosesProp.GetArrayElementAtIndex(i);
				SerializedProperty poseBoneProp = poseProp.FindPropertyRelative("bone");

				Bone2D poseBone = poseBoneProp.objectReferenceValue as Bone2D;

				if(poseBone)
				{
					poseBone.attachedIK = null;
				}
			}

			rootBoneProp.objectReferenceValue = rootBone;
			solverPosesProp.arraySize = 0;

			if(rootBone)
			{
				solverPosesProp.arraySize = numBonesProp.intValue;

				Bone2D bone = rootBone;
				
				for(int i = 0; i < numBonesProp.intValue; ++i)
				{
					SerializedProperty poseProp = solverPosesProp.GetArrayElementAtIndex(i);
					SerializedProperty poseBoneProp = poseProp.FindPropertyRelative("bone");
					SerializedProperty localRotationProp = poseProp.FindPropertyRelative("defaultLocalRotation");
					SerializedProperty solverPositionProp = poseProp.FindPropertyRelative("solverPosition");
					SerializedProperty solverRotationProp = poseProp.FindPropertyRelative("solverRotation");

					if(bone)
					{
						poseBoneProp.objectReferenceValue = bone;
						localRotationProp.quaternionValue = bone.transform.localRotation;
						solverPositionProp.vector3Value = Vector3.zero;
						solverRotationProp.quaternionValue = Quaternion.identity;

						bone = bone.child;
					}
				}
			}
		}

		public static void UpdateIK(Ik2D ik2D, string undoName)
		{
			UpdateIK(ik2D.target,undoName);
		}

		public static void UpdateIK(Bone2D bone, string undoName)
		{
			List<Bone2D> boneList = new List<Bone2D>(25);
			List<Ik2D> ikList = new List<Ik2D>(25);

			BuildIkList(bone,boneList,ikList);

			for (int i = 0; i < ikList.Count; i++)
			{
				Ik2D l_ik2D = ikList[i];

				if(l_ik2D && l_ik2D.isActiveAndEnabled)
				{
					for (int j = 0; j < l_ik2D.solver.solverPoses.Count; j++)
					{
						IkSolver2D.SolverPose pose = l_ik2D.solver.solverPoses [j];
						if (pose.bone)
						{
							Undo.RecordObject(pose.bone.transform, undoName);
						}
					}

					l_ik2D.solver.RestoreDefaultPoses();
					l_ik2D.UpdateIK();
				}
			}
		}

		static void BuildIkList(Bone2D bone, List<Bone2D> boneList, List<Ik2D> ikList)
		{
			if(!bone) return;

			if(boneList.Contains(bone)) return;

			boneList.Add(bone);

			Ik2D ik2D = bone.attachedIK;

			List<Bone2D> childBones = new List<Bone2D>(25);

			if(ik2D)
			{
				if(!ikList.Contains(ik2D))
				{
					ikList.Add(ik2D);
				}

				for (int i = 0; i < ik2D.solver.solverPoses.Count; i++)
				{
					IkSolver2D.SolverPose pose = ik2D.solver.solverPoses [i];

					if(pose.bone)
					{
						pose.bone.GetComponentsInChildren<Bone2D>(childBones);

						for (int j = 0; j < childBones.Count; j++)
						{
							Bone2D l_bone = childBones[j];

							if(l_bone && !boneList.Contains(l_bone))
							{
								BuildIkList(l_bone,boneList,ikList);
							}
						}
					}
				}

			}else{

				bone.GetComponentsInChildren<Bone2D>(childBones);

				for (int j = 0; j < childBones.Count; j++)
				{
					Bone2D l_bone = childBones[j];
					
					if(l_bone && !boneList.Contains(l_bone))
					{
						BuildIkList(l_bone,boneList,ikList);
					}
				}
			}
		}
	}
}
