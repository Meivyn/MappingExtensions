﻿using HarmonyLib;
using UnityEngine;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapObjectSpawnMovementData), nameof(BeatmapObjectSpawnMovementData.GetNoteOffset))]
    internal class BeatmapObjectSpawnMovementDataGetNoteOffset
    {
        private static void Postfix(int noteLineIndex, NoteLineLayer noteLineLayer, ref Vector3 __result, int ____noteLinesCount, Vector3 ____rightVec)
        {
            if (!Plugin.active) return;
            if (noteLineIndex is >= 1000 or <= -1000)
            {
                if (noteLineIndex <= -1000)
                    noteLineIndex += 2000;
                float num = -(____noteLinesCount - 1f) * 0.5f;
                num += noteLineIndex * (StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance / 1000);
                __result = ____rightVec * num + new Vector3(0f, StaticBeatmapObjectSpawnMovementData.LineYPosForLineLayer(noteLineLayer), 0f);
            }
        }
    }

    [HarmonyPatch(typeof(BeatmapObjectSpawnMovementData), nameof(BeatmapObjectSpawnMovementData.Get2DNoteOffset))]
    internal class BeatmapObjectSpawnMovementDataGet2DNoteOffset
    {
        private static void Postfix(int noteLineIndex, NoteLineLayer noteLineLayer, ref Vector2 __result, int ____noteLinesCount)
        {
            if (!Plugin.active) return;
            if (noteLineIndex is >= 1000 or <= -1000)
            {
                if (noteLineIndex <= -1000)
                    noteLineIndex += 2000;
                float num = -(____noteLinesCount - 1f) * 0.5f;
                float x = num + noteLineIndex * (StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance / 1000);
                float y = StaticBeatmapObjectSpawnMovementData.LineYPosForLineLayer(noteLineLayer);
                __result = new Vector2(x, y);
            }
        }
    }

    [HarmonyPatch(typeof(BeatmapObjectSpawnMovementData), nameof(BeatmapObjectSpawnMovementData.GetObstacleOffset))]
    internal class BeatmapObjectSpawnControllerGetObstacleOffset
    {
        private static void Postfix(int noteLineIndex, NoteLineLayer noteLineLayer, ref Vector3 __result,  int ____noteLinesCount, Vector3 ____rightVec)
        {
            if (!Plugin.active) return;
            if (noteLineIndex >= 1000 || noteLineIndex <= -1000)
            {
                if (noteLineIndex <= -1000)
                    noteLineIndex += 2000;
                float num = -(____noteLinesCount - 1f) * 0.5f;
                num += noteLineIndex * (StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance / 1000);
                __result = ____rightVec * num + new Vector3(0f, StaticBeatmapObjectSpawnMovementData.LineYPosForLineLayer(noteLineLayer) + StaticBeatmapObjectSpawnMovementData.kObstacleVerticalOffset, 0f);
            }
        }
    }

    [HarmonyPatch(typeof(BeatmapObjectSpawnMovementData), nameof(BeatmapObjectSpawnMovementData.HighestJumpPosYForLineLayer))]
    internal class BeatmapObjectSpawnMovementDataHighestJumpPosYForLineLayer
    {
        private static void Postfix(NoteLineLayer lineLayer, ref float __result, float ____upperLinesHighestJumpPosY, float ____topLinesHighestJumpPosY, IJumpOffsetYProvider ____jumpOffsetYProvider)
        {
            if (!Plugin.active) return;
            float delta = ____topLinesHighestJumpPosY - ____upperLinesHighestJumpPosY;
            switch ((int)lineLayer)
            {
                case >= 1000 or <= -1000:
                    __result = ____upperLinesHighestJumpPosY - delta - delta + ____jumpOffsetYProvider.jumpOffsetY + (int)lineLayer * (delta / 1000);
                    break;
                case > 2 or < 0:
                    __result = ____upperLinesHighestJumpPosY - delta + ____jumpOffsetYProvider.jumpOffsetY + (int)lineLayer * delta;
                    break;
            }
        }
    }
}
