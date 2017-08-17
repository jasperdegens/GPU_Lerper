using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* GPU_Lerper
 * 
 * Lerps between float(s) buffers and returns results.
 * 
 * Needs to be attached to gameobject, but will perform better than STATIC because resource lookup is only once
 * 
 *
*/



namespace jasper.Tools
{
    public class GPU_Lerper : MonoBehaviour
    {
        [SerializeField]
        private ComputeShader lerper;

        private readonly static string BufferKernelName = "BufferLerp";
        private readonly static string BufferVertAnimKernelName = "BufferVertAnimLerp";
        private readonly static string VertAnimKernelName = "VertAnimLerp";

        private readonly static string BufferAPropName = "_BufferA";
        private readonly static string BufferBPropName = "_BufferB";
        private readonly static string BufferOutputPropName = "_BufferOutput";
        private readonly static string LerpAmountPropName = "_LerpAmount";
        private readonly static string CountPropName = "_Count";

        private static int NUM_THREADS = 8;

        // Lerps float buffers. Floats can be of type float, flaot2/Vec2, float3/Vec3, or float4/Vec4
        public void LerpFloatBuffers(ComputeBuffer a, ComputeBuffer b, ComputeBuffer output, float lerpAmount)
        {

            if (a == null || b == null || output == null) return;

            int floatType = DetermineFloatType(a.stride); // determine float type to decide whether to lerp between float, float2, etc
            string floatLengthString = floatType.ToString(); // should be 1, 2, 3, or 4

            if(lerper == null) lerper = Resources.Load("GPU_Lerper") as ComputeShader;

            int count = Mathf.Min(new int[] { a.count, b.count, output.count });
            int kernel = lerper.FindKernel(BufferKernelName + floatLengthString);


            lerper.SetBuffer(kernel, BufferAPropName + floatLengthString, a);
            lerper.SetBuffer(kernel, BufferBPropName + floatLengthString, b);
            lerper.SetBuffer(kernel, BufferOutputPropName + floatLengthString, output);
            lerper.SetInt(CountPropName, count);
            lerper.SetFloat(LerpAmountPropName, Mathf.Clamp01(lerpAmount));

            lerper.Dispatch(kernel, Mathf.CeilToInt(count / (float)NUM_THREADS), 1, 1);
        }

        private int DetermineFloatType(int stride)
        {
            int type = stride / sizeof(float);
            return type;
        }

    }


}