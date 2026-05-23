using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using Task1.Models;
using Task1.Models.Data;
using Task1.Models.Enums;

namespace Task1.Services
{
    public class ChessPainter
    {
        private const float PieceScale = 0.09f;
        private readonly Dictionary<ChessPiece, Shape> _pieceShapes = new();
        private readonly Shape _boardShape;
        private bool _initialized;
        private string _shapesDir = "";
        private string _texturesDir = "";

        public ChessBoard Board { get; }

        public ChessPainter(ChessMove[] moves)
        {
            Board = new ChessBoard();
            _boardShape = new Shape(0f, 0f, 0f, 4f);
            Initialize(moves);
        }

        public void Update(float deltaTime)
        {
            if (!_initialized) return;
            Board.Update(deltaTime);

            foreach (ChessPiece piece in Board.GetAllPieces())
            {
                if (_pieceShapes.TryGetValue(piece, out Shape? shape))
                {
                    shape.X = piece.Position.X;
                    shape.Y = piece.Position.Y;
                    shape.Z = piece.Position.Z;
                }
            }
        }

        public void Paint()
        {
            if (!_initialized) return;

            _boardShape.Paint();

            foreach (ChessPiece piece in Board.GetAllPieces())
            {
                if (_pieceShapes.TryGetValue(piece, out Shape? shape))
                {
                    shape.Paint();
                }
            }
        }

        public void Dispose()
        {
            _boardShape.Dispose();
            foreach (Shape shape in _pieceShapes.Values)
            {
                shape.Dispose();
            }
        }

        private void Initialize(ChessMove[] moves)
        {
            if (_initialized) return;

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _shapesDir = Path.Combine(baseDir, "shapes");
            _texturesDir = Path.Combine(baseDir, "textures");

            // Создаем директории если их нет
            if (!Directory.Exists(_shapesDir))
                Directory.CreateDirectory(_shapesDir);
            if (!Directory.Exists(_texturesDir))
                Directory.CreateDirectory(_texturesDir);

            Console.WriteLine($"Shapes directory: {_shapesDir}");
            Console.WriteLine($"Textures directory: {_texturesDir}");

            Board.Initialize();
            Board.SetupGameSequence(moves);

            string boardPath = Path.Combine(_shapesDir, "Board.3ds");
            if (File.Exists(boardPath))
            {
                _boardShape.LoadPicture(boardPath);
                Console.WriteLine("Board loaded successfully");
            }
            else
            {
                Console.WriteLine($"Board file not found: {boardPath}");
            }

            BuildPieceShapes();
            _initialized = true;
        }

        private void BuildPieceShapes()
        {
            foreach (ChessPiece piece in Board.GetAllPieces())
            {
                Shape shape = new Shape(piece.Position.X, piece.Position.Y, piece.Position.Z, PieceScale);
                string modelFile = piece.GetModelPath();
                string modelPath = Path.Combine(_shapesDir, Path.GetFileName(modelFile));

                if (File.Exists(modelPath))
                {
                    shape.LoadPicture(modelPath);
                    Console.WriteLine($"Loaded piece: {piece.Type} at {modelPath}");
                }
                else
                {
                    Console.WriteLine($"Piece model not found: {modelPath}");
                }

                shape.SetColor(piece.Color == PieceColor.White ? new float[] { 1f, 1f, 1f } : new float[] { 0.3f, 0.3f, 0.3f });
                _pieceShapes[piece] = shape;
            }
        }
    }
}