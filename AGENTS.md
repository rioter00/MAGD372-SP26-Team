# AGENTS.md

## Project Snapshot
- Unity project pinned to `6000.2.1f1` (`ProjectSettings/ProjectVersion.txt`).
- URP rendering stack via `com.unity.render-pipelines.universal` (`Packages/manifest.json`).
- Build Settings currently include only `Assets/Scenes/SUNMOONALPHA.unity` (`ProjectSettings/EditorBuildSettings.asset`).

## High-Signal Code Areas
- Runner gameplay core: `Assets/Michael's Stuff/Scripts/`.
- Menu/start-state toggles: `Assets/Michael's Stuff/Scripts/RunnerGameFlow.cs`, `Assets/MainMenu Scripts/MainMenuUI.cs`.
- Lighting/fog integration: `Assets/DayNightCycle.cs`, `Assets/URPFogDriver.cs`, `Assets/CarHeadlightController.cs`, `Assets/CloseFogRenderFeature.cs`.

## Runtime Architecture (How Systems Connect)
- `RunnerGameFlow` owns menu/start transitions and delayed run start; it calls into a `RunnerSpeedSource`.
- `RunnerSpeedSource` is the speed abstraction (`CurrentSpeed`, `BeginRun`, `ResetToMenuState`), with `BasicStartupSpeedSource` as a concrete implementation.
- `MovingEndlessRoad` reads `speedSource.CurrentSpeed` in `FixedUpdate`, moves active `RoadTile` instances backward, recycles passed tiles, then spawns ahead.
- `RoadTile` defines end-of-tile semantics (`exitPoint` fallback + kinematic rigidbody movement), which drives spawn/recycle behavior.
- `LaneMover_PlayerOnly` handles lane changes with `Input.GetKeyDown(KeyCode.A/D)` and is currently independent from Input System actions assets.

## Rendering/Data Flow You Must Preserve
- `DayNightCycle` is the source of truth for sun/moon intensity, ambient light, fog color, and skybox switching.
- `URPFogDriver` disables built-in fog each frame and pushes `RenderSettings.fogColor` plus fog distance/strength into a fog material.
- `CarHeadlightController` toggles headlight `Light`s from sun direction and writes beam vectors/strength (`_HeadlightPosA/B`, `_HeadlightDirA/B`, `_FogClearStrength`) to the fog material.
- `CloseFogRenderFeature` runs a URP blit pass that consumes camera color/depth; shader/material property names must stay in sync with script writes.

## Repo-Specific Conventions
- Treat vendor/sample directories as mostly read-only unless explicitly requested: `Assets/BOXOPHIC/`, `Assets/TextMesh Pro/`, and imported asset-pack folders.
- Prefer gameplay edits in first-party scripts under `Assets/Michael's Stuff/Scripts/` and root-level first-party scripts in `Assets/`.
- Scripts rely heavily on public inspector fields; preserve serialized field names/types to avoid breaking scene references.
- Local style favors straightforward MonoBehaviour orchestration over framework-heavy patterns; match nearby code style.

## Developer Workflows (macOS zsh)
- Open in matching Unity version:
  ```bash
  "/Applications/Unity/Hub/Editor/6000.2.1f1/Unity.app/Contents/MacOS/Unity" -projectPath "/Users/nickhwang/Documents/GitHub/MAGD372-SP26-Team"
  ```
- Run EditMode tests (test framework is installed):
  ```bash
  "/Applications/Unity/Hub/Editor/6000.2.1f1/Unity.app/Contents/MacOS/Unity" -batchmode -quit -projectPath "/Users/nickhwang/Documents/GitHub/MAGD372-SP26-Team" -runTests -testPlatform EditMode -testResults "Logs/editmode-tests.xml"
  ```
- Run PlayMode tests:
  ```bash
  "/Applications/Unity/Hub/Editor/6000.2.1f1/Unity.app/Contents/MacOS/Unity" -batchmode -quit -projectPath "/Users/nickhwang/Documents/GitHub/MAGD372-SP26-Team" -runTests -testPlatform PlayMode -testResults "Logs/playmode-tests.xml"
  ```
- No custom scripted build entry point was found; use Unity Build Settings or add a dedicated static build method before CI automation.

## Existing AI Conventions Scan
- One glob scan of `**/{.github/copilot-instructions.md,AGENT.md,AGENTS.md,CLAUDE.md,.cursorrules,.windsurfrules,.clinerules,.cursor/rules/**,.windsurf/rules/**,.clinerules/**,README.md}` returned no matching files at generation time.
