using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Task1
{
    public class IcosaDodecahedron
    {
        private Vector3[] vertices;
        private List<(int, int)> edges = new();
        private List<int[]> triangles = new();
        private List<int[]> pentagons = new();

        private List<int>[] adjacency;

        private const float Scale = 1.5f;

        public IcosaDodecahedron()
        {
            BuildVertices();
            BuildEdges();
            BuildAdjacency();
            BuildFaces();
        }

        private void BuildVertices()
        {
            var raw = new List<Vector3>();

            float phi = (1f + (float)Math.Sqrt(5)) / 2f;
            float phi2 = phi * phi;
            float a = 2f * phi;

            raw.AddRange(new[]
            {
                new Vector3(0,0,a), new Vector3(0,0,-a),
                new Vector3(0,a,0), new Vector3(0,-a,0),
                new Vector3(a,0,0), new Vector3(-a,0,0)
            });

            int[] s = { 1, -1 };

            foreach (int x in s)
            {
                foreach (int y in s)
                {
                    foreach (int z in s)
                    {
                        raw.Add(new Vector3(x * 1, y * phi, z * phi2));
                        raw.Add(new Vector3(x * phi2, y * 1, z * phi));
                        raw.Add(new Vector3(x * phi, y * phi2, z * 1));
                    }
                }
            }

            vertices = raw.Select(Vector3.Normalize).ToArray();
        }

        private void BuildEdges()
        {
            float minDist = float.MaxValue;

            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = i + 1; j < vertices.Length; j++)
                {
                    float d = (vertices[i] - vertices[j]).Length;
                    if (d > 0.01f && d < minDist)
                    {
                        minDist = d;
                    }
                }
            }

            float eps = 0.05f;

            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = i + 1; j < vertices.Length; j++)
                {
                    float d = (vertices[i] - vertices[j]).Length;
                    if (Math.Abs(d - minDist) < eps)
                    {
                        edges.Add((i, j));
                    }
                }
            }
        }

        private void BuildAdjacency()
        {
            adjacency = new List<int>[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                adjacency[i] = new List<int>();
            }

            foreach (var (a, b) in edges)
            {
                adjacency[a].Add(b);
                adjacency[b].Add(a);
            }
        }

        private bool IsTriangleUnique(List<int[]> triList, int[] triangle)
        {
            foreach (var existing in triList)
            {
                if (existing[0] == triangle[0] &&
                    existing[1] == triangle[1] &&
                    existing[2] == triangle[2])
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsPentagonUnique(List<int[]> pentList, int[] pentagon)
        {
            foreach (var existing in pentList)
            {
                bool match = true;
                for (int idx = 0; idx < 5; idx++)
                {
                    if (existing[idx] != pentagon[idx])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    return false;
                }
            }
            return true;
        }

        private List<int[]> FindTriangles()
        {
            var triList = new List<int[]>();

            for (int i = 0; i < vertices.Length; i++)
            {
                for (int jIdx = 0; jIdx < adjacency[i].Count; jIdx++)
                {
                    int j = adjacency[i][jIdx];
                    for (int kIdx = 0; kIdx < adjacency[j].Count; kIdx++)
                    {
                        int k = adjacency[j][kIdx];
                        if (k != i && adjacency[i].Contains(k))
                        {
                            var t = new[] { i, j, k };
                            Array.Sort(t);

                            if (IsTriangleUnique(triList, t))
                            {
                                triList.Add(t);
                            }
                        }
                    }
                }
            }

            return triList;
        }

        private List<int[]> FindPentagons()
        {
            var pentList = new List<int[]>();

            for (int a = 0; a < vertices.Length; a++)
            {
                for (int bIdx = 0; bIdx < adjacency[a].Count; bIdx++)
                {
                    int b = adjacency[a][bIdx];
                    for (int cIdx = 0; cIdx < adjacency[b].Count; cIdx++)
                    {
                        int c = adjacency[b][cIdx];
                        for (int dIdx = 0; dIdx < adjacency[c].Count; dIdx++)
                        {
                            int d = adjacency[c][dIdx];
                            for (int eIdx = 0; eIdx < adjacency[d].Count; eIdx++)
                            {
                                int e = adjacency[d][eIdx];
                                if (e != a && adjacency[e].Contains(a))
                                {
                                    var p = new[] { a, b, c, d, e };
                                    Array.Sort(p);

                                    if (IsPentagonUnique(pentList, p))
                                    {
                                        pentList.Add(p);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return pentList;
        }

        private void BuildFaces()
        {
            triangles = FindTriangles();
            pentagons = FindPentagons();
        }

        private void DrawTriangles()
        {
            GL.Color4(Color4.Orange);
            GL.Begin(PrimitiveType.Triangles);

            foreach (var t in triangles)
            {
                foreach (int i in t)
                {
                    GL.Vertex3(vertices[i] * Scale);
                }
            }

            GL.End();
        }

        private void DrawPentagons()
        {
            GL.Color4(Color4.CornflowerBlue);

            foreach (var p in pentagons)
            {
                GL.Begin(PrimitiveType.TriangleFan);
                foreach (int i in p)
                {
                    GL.Vertex3(vertices[i] * Scale);
                }
                GL.End();
            }
        }

        private void DrawEdges()
        {
            GL.Color4(Color4.Black);
            GL.LineWidth(1.5f);

            GL.Begin(PrimitiveType.Lines);
            foreach (var (a, b) in edges)
            {
                GL.Vertex3(vertices[a] * Scale);
                GL.Vertex3(vertices[b] * Scale);
            }
            GL.End();
        }

        public void Draw()
        {
            GL.Enable(EnableCap.DepthTest);

            DrawTriangles();
            DrawPentagons();

            GL.Disable(EnableCap.CullFace);

            DrawEdges();
        }
    }
}