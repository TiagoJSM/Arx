using UnityEngine;
using System;
using System.Collections.Generic;

namespace Anima2D 
{
	[Serializable]
	public class BoneWeight2 : ICloneable
	{
		public float weight0 = 0f;
		public float weight1 = 0f;
		public float weight2 = 0f;
		public float weight3 = 0f;
		public int boneIndex0 = 0;
		public int boneIndex1 = 0;
		public int boneIndex2 = 0;
		public int boneIndex3 = 0;

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		public float GetBoneWeight(int boneIndex)
		{
			if(boneIndex0 == boneIndex)
			{
				return weight0;
				
			}else if(boneIndex1 == boneIndex)
			{
				return weight1;
			}else if(boneIndex2 == boneIndex)
			{
				return weight2;
			}else if(boneIndex3 == boneIndex)
			{
				return weight3;
			}

			return 0f;
		}

		public void SetBoneIndexWeight(int boneIndex, float weight)
		{
			int weightIndex = -1;

			weight = Mathf.Clamp01(weight);

			if(boneIndex0 == boneIndex)
			{
				weightIndex = 0;
				weight0 = weight;

			}else if(boneIndex1 == boneIndex)
			{
				weightIndex = 1;
				weight1 = weight;
			}else if(boneIndex2 == boneIndex)
			{
				weightIndex = 2;
				weight2 = weight;
			}else if(boneIndex3 == boneIndex)
			{
				weightIndex = 3;
				weight3 = weight;
			}

			Normalize(weightIndex);
		}

		public void SetWeight(int weightIndex, int boneIndex, float weight)
		{
			weight = Mathf.Clamp01(weight);

			if(weightIndex == 0)
			{
				boneIndex0 = boneIndex;
				weight0 = weight;
				
			}else if(weightIndex == 1)
			{
				boneIndex1 = boneIndex;
				weight1 = weight;
				
			}else if(weightIndex == 2)
			{
				boneIndex2 = boneIndex;
				weight2 = weight;
				
			}else if(weightIndex == 3)
			{
				boneIndex3 = boneIndex;
				weight3 = weight;
			}

			Normalize(weightIndex);
		}

		void Normalize(int masterIndex)
		{
			if(masterIndex >= 0 && masterIndex < 4)
			{
				float sum = 0f;

				float[] weights = new float[4];

				weights[0] = weight0;
				weights[1] = weight1;
				weights[2] = weight2;
				weights[3] = weight3;

				for(int i = 0; i < 4; ++i)
				{
					if(i != masterIndex)
					{
						sum += weights[i];
					}
				}

				float targetSum = 1f- weights[masterIndex];

				for(int i = 0; i < 4; ++i)
				{
					if(i != masterIndex)
					{
						if(sum > 0f)
						{
							weights[i] = weights[i] * targetSum / sum;
						}else{
							weights[i] = targetSum / 3f;
						}
					}
				}

				weight0 = weights[0];
				weight1 = weights[1];
				weight2 = weights[2];
				weight3 = weights[3];
			}
		}
	}
}
