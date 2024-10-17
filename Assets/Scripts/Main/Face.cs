using Loop;
using System;
using System.Collections.Generic;

namespace Main {
    public class Face
    {
        private const int QuantityFaceOfCube = 4;
        private const int IndexActiveFaceCub = 0;
        private const int IndexRightFaceCub = 1;
        private const int IndexRearFaceCub = 2;
        private const int IndexLeftFaceCub = 3;

        private readonly int _�ellEdge = 4;
        private readonly TurnFace _turnFace = new TurnFace();

        private Cell[,] _cells;

        public int CellEdge => _�ellEdge;

        public Face()
        {
            _cells = new Cell[CellEdge, CellEdge];
        }

        public void Init()
        {
            FillEmptyCell();
        }

        public void Init(Face faceLeft)
        {
            FillCellLeft(faceLeft);
            Init();
        }

        public void Init(Face faceLeft, Face faceRight)
        {
            FillCellRight(faceRight);
            Init(faceLeft);
        }

        public void InitUp(IReadOnlyList<Face> faces)
        {
            if (faces.Count != QuantityFaceOfCube)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = 0; i < CellEdge; i++)
            {
                _cells[i, 0] = faces[IndexLeftFaceCub].GetCell(0, i);
                _cells[i, CellEdge - 1] = faces[IndexRightFaceCub].GetCell(0, CellEdge - i - 1);
                _cells[0, i] = faces[IndexRearFaceCub].GetCell(0, CellEdge - i - 1);
                _cells[CellEdge - 1, i] = faces[IndexActiveFaceCub].GetCell(0, i);
            }
            
            FillEmptyCell();
        }

        public void InitDown(IReadOnlyList<Face> faces)
        {
            if (faces.Count != QuantityFaceOfCube)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = 0; i < CellEdge; i++)
            {
                _cells[i, 0] = faces[IndexLeftFaceCub].GetCell(CellEdge - 1, CellEdge - i - 1);
                _cells[i, CellEdge - 1] = faces[IndexRightFaceCub].GetCell(CellEdge - 1, i);
                _cells[0, i] = faces[IndexActiveFaceCub].GetCell(CellEdge - 1, i);
                _cells[CellEdge - 1, i] = faces[IndexRearFaceCub].GetCell(CellEdge - 1, CellEdge - i - 1);
            }

            FillEmptyCell();
        }

        public Cell GetCell(int i, int j)
        {
            return _cells[i, j];
        }

        public void ThrowStatusBlock()
        {
            foreach (ValueIJ valueIJ in DoubleLoop.GetValues(new SettingsLoop(CellEdge - 1)))
            {
                _cells[valueIJ.I, valueIJ.J].ThrowStatusBlock();
            }
        }

        public void TurnRight()
        {
            _cells = _turnFace.TurnRight(_cells, CellEdge);
        }

        public void TurnLeft()
        {
            _cells = _turnFace.TurnLeft(_cells, CellEdge);
        }

        public void Invert()
        {
            _cells = _turnFace.Invert(_cells, CellEdge);
        }

        private void FillEmptyCell()
        {

            foreach (ValueIJ valueIJ in DoubleLoop.GetValues(new SettingsLoop(CellEdge - 1)))
            {
                if (_cells[valueIJ.I, valueIJ.J] == null)
                {
                    _cells[valueIJ.I, valueIJ.J] = new Cell();
                }
            }
        }

        private void FillCellLeft(Face faceLeft)
        {
            for (int i = 0; i < CellEdge; i++)
            {
                _cells[i, 0] = faceLeft.GetCell(i, CellEdge - 1);
            }
        }

        private void FillCellRight(Face faceRight)
        {
            for (int i = 0; i < CellEdge; i++)
            {
                _cells[i, CellEdge - 1] = faceRight.GetCell(i, 0);
            }
        }
    }
}
