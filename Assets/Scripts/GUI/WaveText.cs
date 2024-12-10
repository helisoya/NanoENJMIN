using TMPro;
using UnityEngine;

public class WaveText : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private bool canWave = true;
    [SerializeField] private int startLetter = 0;
    [SerializeField] private int endLetter = -1;

    [Header("Parameters Wave"), Space(6)]
    [SerializeField] private float speedWave = 2;
    [SerializeField] private float verticeDistorsion = 0.01f;
    [SerializeField] private float heigthWave = 5;

    private TMP_TextInfo _textInfo;
    private TMP_TextInfo _initialTextInfo;

    private void Awake()
    {
        if(endLetter < 0)
        {
            endLetter = textComponent.text.Length;
        }

        _textInfo = textComponent.textInfo;
        _initialTextInfo = textComponent.textInfo;
    }

    void FixedUpdate()
    {
        if(canWave)
            Waving();
    }

    public void SwitchCanWaveTrue()
    {
        canWave = true;
    }

    public void SwitchCanWaveFalse()
    {
        canWave = false;
        ApplyModifications(_initialTextInfo);
        textComponent.ForceMeshUpdate();
    }

    private void Waving()
    {
        textComponent.ForceMeshUpdate();

        // modification of the _textInfo
        for (int i = startLetter; i < startLetter + endLetter; ++i)
        {
            var charInfo = _textInfo.characterInfo[i];

            if (!charInfo.isVisible)
            {
                continue;
            }

            var verts = _textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            // Modification of the vertices (4 because it is a quad)
            for (int j = 0; j < 4; ++j)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * speedWave + orig.x * verticeDistorsion) * heigthWave, 0);
            }
        }

        // Apply modifications on the textMeshPro
        ApplyModifications(_textInfo);
    }

    private void ApplyModifications(TMP_TextInfo tmp_ti)
    {
        for (int i = 0; i < tmp_ti.meshInfo.Length; ++i)
        {
            var meshInfo = tmp_ti.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
