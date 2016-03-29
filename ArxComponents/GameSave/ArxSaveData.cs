using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.GameSave
{
    [Serializable]
    public class Vec3
    {
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public class ArxSaveData
    {
        private static ArxSaveData _current;

        public static ArxSaveData Current
        {
            get
            {
                return _current;
            }
            set
            {
                _current = value;
            }
        }

        private Vec3 playerPosition;

        public string LevelName { get; set; }
        public Vector3? PlayerPosition
        {
            get
            {
                if(playerPosition == null)
                {
                    return null;
                }
                return new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
            }
            set
            {
                if(value == null)
                {
                    playerPosition = null;
                }
                playerPosition = new Vec3()
                {
                    x = value.Value.x,
                    y = value.Value.y,
                    z = value.Value.z
                };
            }
        }

        public DreamingBeyondImaginationSaveData DreamingBeyondImagination { get; set; }

        public ArxSaveData()
        {
            DreamingBeyondImagination = new DreamingBeyondImaginationSaveData();
        }
    }
}
