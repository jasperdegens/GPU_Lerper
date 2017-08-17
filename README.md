GPU_Lerper
==========

This Unityhelper lerps between float, float2/Vector2, float3/Vector3, float4/Vector4 compute buffers.

How to Use 
----------

### STATIC_GPU_Lerper -> static class
From STATIC_GPU_Lerper static class, call: 
```c#
public static void LerpFloatBuffers(ComputeBuffer a, ComputeBuffer b, ComputeBuffer output, float lerpAmount)
```


### GPU_Lerper
Attach to game object, and then call:
```c#
public void LerpFloatBuffers(ComputeBuffer a, ComputeBuffer b, ComputeBuffer output, float lerpAmount)
```

### Issues/Concerns
STATIC_GPU_Lerper is more conveninet because it is a static class, but needs to load the ComputeShader from Resources each time it is called. If executed many times, GPU_Lerper class is recommended.