using UnityEngine;
using TMPro;

[ExecuteInEditMode] // This lets you see the curve in the Scene view without hitting Play
public class TextCurveEffect : MonoBehaviour
{
    public TMP_Text textMesh;
    public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
    public float curveScale = 50f;

    void Update()
    {
        if (textMesh == null) return;

        textMesh.ForceMeshUpdate();
        TMP_TextInfo textInfo = textMesh.textInfo;
        int characterCount = textInfo.characterCount;

        if (characterCount == 0) return;

        for (int i = 0; i < characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            Vector3[] sourceVertices = textInfo.meshInfo[materialIndex].vertices;

            // Find the center point of the letter
            float x0 = (sourceVertices[vertexIndex + 0].x + sourceVertices[vertexIndex + 2].x) / 2;

            // Map the x position to a 0-1 range for the curve
            float minX = textMesh.bounds.min.x;
            float maxX = textMesh.bounds.max.x;
            float t = (x0 - minX) / (maxX - minX);

            // Calculate the offset based on your curve
            float yOffset = curve.Evaluate(t) * curveScale;

            // Apply the offset to all 4 vertices of the character
            Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
            destinationVertices[vertexIndex + 0].y += yOffset;
            destinationVertices[vertexIndex + 1].y += yOffset;
            destinationVertices[vertexIndex + 2].y += yOffset;
            destinationVertices[vertexIndex + 3].y += yOffset;
        }

        textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}