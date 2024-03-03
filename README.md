# First time setup

1. Pull repository
2. Setup [VSCode for Unity](https://code.visualstudio.com/docs/other/unity) and use **game.code-workspace** with VSCode for auto-format / auto-complete
2. Setup ReactUnity (see below)
3. Try running game and should be able to move around with WASD

# ReactUnity Setup

1. Install [node](https://nodejs.org/en/download) for windows
2. Open terminal and navigate to **/react**
3. Install node dependencies with **npm install**
4. Open Unity Project
5. Open **React/Quick Start** window inside Unity, and install dependencies
6. **When developing:** Run **npm start** inside **/react** folder
7. Test UI by going into play mode and on the **Services/ReactUnity** game object, change the **Debug Route** on the **UI Router** script.
8. **When building:** Run **npm run build** inside **/react** folder and copy built assets to **Resources/react**

Documentation: https://reactunity.github.io/

Repo: https://github.com/ReactUnity/core

# Helpful Links

- Unity Input System: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/index.html 
- Unity Post Processing: https://docs.unity3d.com/Packages/com.unity.postprocessing@3.2/manual/Quick-start.html 
- "Skyplane" Shader: https://aras-p.info/blog/2019/02/01/Infinite-sky-shader-for-Unity/ 
- Rendering Grids: https://gamedev.stackexchange.com/questions/182263/infinite-world-floor-grid-shader 
- Computing Vertex Normals from Heightmap: https://forum.unity.com/threads/calculate-vertex-normals-in-shader-from-heightmap.169871/ 
- HDR Bloom: https://gamedev.stackexchange.com/questions/137659/unity-how-to-apply-bloom-to-only-some-objects-in-the-scene 
- sRGB vs Linear: https://forum.unity.com/threads/understanding-srgb-and-gamma-corrected-values-in-the-render-pipeline.783224/ 

### Fonts

- Can input some text and see different fonts render them: https://fonts.google.com/?preview.text=Handler&preview.text_type=custom
- Vaporwave Text Generator: https://lingojam.com/VaporwaveTextGenerator 
- Show unicode characters in text: https://qaz.wtf/u/show.cgi?show=Handler&type=string 
- Unicode Characters for Japanese: https://stackoverflow.com/questions/19899554/unicode-range-for-japanese 

# References

https://www.pinterest.com/pin/783626403888415262/ 

