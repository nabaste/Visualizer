using UnityEngine;
using Unity.Serialization.Json;
using Visualizer.Core;

namespace Visualizer.IO
{
    public class JsonLoader : MonoBehaviour
    {
        public static MeshData ParseJson(string json)
        {
            MeshData data = JsonSerialization.FromJson<MeshData>(json);
            return data;
        }
    }
}