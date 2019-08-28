using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation
{
    /// <summary>
    /// Represents a face detected by an AR device.
    /// </summary>
    /// <remarks>
    /// Generated by the <see cref="ARFaceManager"/> when an AR device detects
    /// a face in the environment.
    /// </remarks>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(ARUpdateOrder.k_Face)]
    [HelpURL("https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@1.5/api/UnityEngine.XR.ARFoundation.ARFace.html")]
    public sealed class ARFace : ARTrackable<XRFace, ARFace>
    {
        /// <summary>
        /// Invoked when the face is updated. If face meshes are supported, there will be
        /// updated <see cref="vertices"/>, <see cref="normals"/>, <see cref="indices"/>, and
        /// <see cref="uvs"/>.
        /// </summary>
        public event Action<ARFaceUpdatedEventArgs> updated;

        /// <summary>
        /// The vertices representing the face mesh. Check for existence with <c>vertices.IsCreated</c>.
        /// This array is parallel to <see cref="normals"/> and <see cref="uvs"/>. Vertices are
        /// provided in face space, that is, relative to this <see cref="ARFace"/>'s local
        /// position and rotation.
        /// </summary>
        public unsafe NativeArray<Vector3> vertices
        {
            get
            {
                return GetUndisposable(m_FaceMesh.vertices);
            }
        }

        /// <summary>
        /// The normals representing the face mesh. Check for existence with <c>normals.IsCreated</c>.
        /// This array is parallel to <see cref="vertices"/> and <see cref="uvs"/>.
        /// </summary>
        public unsafe NativeArray<Vector3> normals
        {
            get
            {
                return GetUndisposable(m_FaceMesh.normals);
            }
        }

        /// <summary>
        /// The indices defining the triangles of the face mesh. Check for existence with <c>indices.IsCreated</c>.
        /// The are three times as many indices as triangles, so this will always be a multiple of 3.
        /// </summary>
        public NativeArray<int> indices
        {
            get
            {
                return GetUndisposable(m_FaceMesh.indices);
            }
        }

        /// <summary>
        /// The texture coordinates representing the face mesh. Check for existence with <c>uvs.IsCreated</c>.
        /// This array is parallel to <see cref="vertices"/> and <see cref="normals"/>.
        /// </summary>
        public NativeArray<Vector2> uvs
        {
            get
            {
                return GetUndisposable(m_FaceMesh.uvs);
            }
        }

        void Update()
        {
            if (m_Updated && updated != null)
            {
                updated(new ARFaceUpdatedEventArgs(this));
                m_Updated = false;
            }
        }

        void OnDestroy()
        {
            m_FaceMesh.Dispose();
        }

        // Creates an alias to the same array, but the caller cannot Dispose it.
        unsafe NativeArray<T> GetUndisposable<T>(NativeArray<T> disposable) where T : struct
        {
            if (!disposable.IsCreated)
                return default(NativeArray<T>);

            return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(
                disposable.GetUnsafePtr(),
                disposable.Length,
                Allocator.None);
        }

        internal void UpdateMesh(XRFaceSubsystem subsystem)
        {
            subsystem.GetFaceMesh(sessionRelativeData.trackableId, Allocator.Persistent, ref m_FaceMesh);
            m_Updated = true;
        }

        XRFaceMesh m_FaceMesh;

        bool m_Updated;
    }
}
