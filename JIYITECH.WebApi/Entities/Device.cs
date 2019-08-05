using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Entities
{
    public class Device
    {
        public class BucketWheelStack
        {
            private static readonly List<BucketWheelStack> instances = new List<BucketWheelStack>();
            public float Position { get; set; }
            public float Rotation { get; set; }
            public float Pitch { get; set; }
            private BucketWheelStack()
            {
            }
        }
    }
}
