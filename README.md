# 🧭 Terrain Visualizer

A compact Unity-based application for loading, visualizing, and interacting with 3D terrain meshes from JSON files. Inspired by AEC tools like Rhino or AutoCAD, it offers a simplified multi-camera viewport, interactive UI, and shading modes for exploring mesh geometry and metadata such as slope.



## ✨ Features

- 🧾 **Mesh Import**: Load `.json` terrain meshes with vertex data, normals, and slope angles.
- 🔲 **AEC-style Viewports**: Four-view layout (Perspective, Top, Front, Right) with maximization and focus switching.
- 🎨 **Shading Modes**:
  - Default shaded view
  - Wireframe overlays
  - Slope-based heatmap
  - Face selection mode
- 🖱 **Face Interaction**: Click to highlight individual mesh faces using vertex color overlays.
- 🧠 **Slope Visualization**: Vertex slope data linearly mapped to color gradients.
- 🖼 **Responsive UI**: Top toolbar with dropdowns, buttons, and labels indicating view focus.
- 💡 **Custom Shaders**: Includes simplified HLSL shaders compatible with URP for real-time slope rendering and selection.



## 🗂 Folder Structure
```bash
Assets/Scripts/
├── Core/ # App entry point, viewport and mesh lifecycle management
├── IO/ # JSON parsing, mesh construction with vertex welding
├── UI/ # Topbar buttons, dropdowns, and view label hooks
├── Cameras/ # Input and control per camera, focus logic
├── Visualization/ # Shader management and slope mapping
└── Interaction/ # Raycasting-based face selection and highlighting
```



## 🛠 Getting Started

### Platform

- Created wit Unity **6000.0.23f1**
- **Universal Render Pipeline (URP)**
- Windows build target

### Setup

1. Clone this repo:
   ```bash
   git clone https://github.com/nabaste/Visualizer.git
   ```
2. Open the project in Unity.
3. Play the scene.

## 📁 JSON Format
The application expects a JSON mesh file with the following structure:

```bash
{
  "Vertices": [[x, y, z], ...],
  "Faces": [[i1, i2, i3], ...],
  "Vertex_Normals": [[nx, ny, nz], ...],
  "Slope_Angles": [float, float, ...]
}
```
