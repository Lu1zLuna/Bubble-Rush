using System.Collections.Generic;
using UnityEngine;

public class RoadRenderer : MonoBehaviour
{
    [Header("Rendering")]
    public Material drawMaterial;
    public int screenWidth = 1024;
    public int screenHeight = 600;
    public float roadWidth = 2000f;
    public float camZ = 0.85f;
    public int segmentLength = 200;

    [Header("Colors")]
    public Color grassDark = new Color(0f, 0.51f, 0.03f);
    public Color grassLight = new Color(0.01f, 0.67f, 0.05f);
    public Color borderDark = Color.red;
    public Color borderLight = Color.white;
    public Color roadDark = new Color(0.42f, 0.42f, 0.42f);
    public Color roadLight = new Color(0.4f, 0.4f, 0.4f);
    public Color stripLight = Color.white;
    public Color stripDark = new Color(0, 0, 0, 0);

    private List<LineSegment> lines = new List<LineSegment>();
    private float position = 0;
    private int numLines;
    private float direction = 0;

    void Start()
    {
        GenerateRoad();
    }

    void Update()
    {
        direction = Input.GetAxis("Vertical"); // W/S ou setas
        position += segmentLength * direction;
    }

    void OnPostRender()
    {
        Debug.Log("OnPostRender chamado!");


        if (!drawMaterial) return;
        drawMaterial.SetPass(0);

        GL.PushMatrix();
        GL.LoadOrtho();

        DrawRoad();

        GL.PopMatrix();
    }

    void GenerateRoad()
    {
        int roadLength = 2000;
        for (int i = 0; i < roadLength; i++)
        {
            LineSegment line = new LineSegment();
            line.z = i * segmentLength + 0.00000001f;

            if (i > 300 && i < 700) line.curve = 0.5f;
            if (i > 800 && i < 1200) line.curve = -0.7f;
            if (i < 755) line.y = Mathf.Sin(i / 30.0f) * 1500;
            if (i > 1200) line.curve = 0.2f;

            lines.Add(line);
        }
        numLines = lines.Count;
    }

    void DrawQuad(Color color, float x1, float y1, float w1, float x2, float y2, float w2)
    {
        float sw = screenWidth;
        float sh = screenHeight;

        float a_x = (x1 - w1) / sw;
        float a_y = y1 / sh;
        float b_x = (x2 - w2) / sw;
        float b_y = y2 / sh;
        float c_x = (x2 + w2) / sw;
        float c_y = y2 / sh;
        float d_x = (x1 + w1) / sw;
        float d_y = y1 / sh;

        GL.Begin(GL.QUADS);
        GL.Color(color);
        GL.Vertex3(a_x, a_y, 0);
        GL.Vertex3(b_x, b_y, 0);
        GL.Vertex3(c_x, c_y, 0);
        GL.Vertex3(d_x, d_y, 0);
        GL.End();
    }

    LineSegment Project(LineSegment line, float camX, float camY, float camZ)
    {
        float scale = camZ / (line.z - camZ);
        line.X = (1 + scale * (line.x - camX)) * screenWidth / 2;
        line.Y = (1 - scale * (line.y - camY)) * screenHeight / 2;
        line.W = scale * roadWidth * (screenWidth / 2);
        return line;
    }

    void DrawRoad()
    {
        if (position >= numLines * segmentLength) position -= numLines * segmentLength;
        if (position < 0) position += numLines * segmentLength;

        int start = Mathf.FloorToInt(position / segmentLength);
        float camH = 1500 + lines[start].y;

        float x = 0;
        float dx = 0;
        float cutoff = screenHeight;

        for (int n = start; n < start + 300; n++)
        {
            int index = n % numLines;
            LineSegment current = Project(lines[index], -x, camH, position);
            LineSegment prev = Project(lines[(index - 1 + numLines) % numLines], -x - dx, camH, position - segmentLength);

            x += dx;
            dx += current.curve;

            if (current.Y >= cutoff) continue;
            cutoff = current.Y;

            bool alt = (n / 4) % 2 == 0;
            bool grassAlt = (n / 8) % 2 == 0;

            Color border = alt ? borderLight : borderDark;
            Color road = alt ? roadLight : roadDark;
            Color grass = grassAlt ? grassLight : grassDark;
            Color strip = grassAlt ? stripLight : stripDark;

            DrawQuad(grass, 0, prev.Y, screenWidth, 0, current.Y, screenWidth);
            DrawQuad(border, prev.X, prev.Y, prev.W * 1.2f, current.X, current.Y, current.W * 1.2f);
            DrawQuad(road, prev.X, prev.Y, prev.W, current.X, current.Y, current.W);
            DrawQuad(strip, prev.X, prev.Y, prev.W * 0.01f, current.X, current.Y, current.W * 0.01f);
        }
    }

    class LineSegment
    {
        public float x = 0, y = 0, z = 0;
        public float X = 0, Y = 0, W = 0;
        public float curve = 0;
    }
}
