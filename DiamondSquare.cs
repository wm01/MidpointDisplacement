using UnityEngine;
using System;

public class DiamondSquare {
    private float[] _ValueMap;
    private int _Width;
    private int _Height;
    private int _Size;
    private float _AverageValue;
 
    //Width must be (2^n) + 1
    public DiamondSquare(int width)
    {
        if ((((width - 1) & ((width - 2))) == 0) && (width != 0))
        {
            _Width = width;
            _Height = width;
            _Size = width * width;
            _ValueMap = new float[_Size];
        }
        else
        {
            throw new ArgumentOutOfRangeException("Width not a power of 2 + 1 ");
        }
    }

    public void CreateFractal(float startingCornerValues, float rangeValue)
    {

        _ValueMap[0] = startingCornerValues;
        _ValueMap[_Width - 1] = startingCornerValues;
        _ValueMap[_Width * (_Height - 1)] = startingCornerValues;
        _ValueMap[(_Width * _Height) - 1] = startingCornerValues;

        DiamondSquareSteps(rangeValue, true);
    }

    public void CreateFractal(float startingCornerValues, float rangeValue, bool useRandomValues)
    {

        _ValueMap[0] = startingCornerValues;
        _ValueMap[_Width - 1] = startingCornerValues;
        _ValueMap[_Width * (_Height - 1)] = startingCornerValues;
        _ValueMap[(_Width * _Height) - 1] = startingCornerValues;

        DiamondSquareSteps(rangeValue, useRandomValues);
    }

    public void CreateFractal(float startingTopLeftCornerValue, float startingTopRightCornerValue, float startingBottomLeftCornerValue, float startingBottomRightCornerValue, float rangeValue)
    {
        _ValueMap[0] = startingTopLeftCornerValue;
        _ValueMap[_Width - 1] = startingTopRightCornerValue;
        _ValueMap[_Width * (_Height - 1)] = startingBottomLeftCornerValue;
        _ValueMap[(_Width * _Height) - 1] = startingBottomRightCornerValue;

        DiamondSquareSteps(rangeValue, true);
    }

    private void DiamondSquareSteps(float rangeValue, bool useRandomValues)
    {
        //initial number of squares
        int numOfSquares = 1;
        int squareLength = _Width - 1;
        int numOfSquareRows = 1;
        int numOfSquaresInRow = 1;
        int numOfDiamondRows = 2;
        int numOfDiamondsInRows = 1;
        float count = 1.0f;
        //Begin Diamond Square algorithm (Midpoint Displacement Algo)
        while (squareLength > 1)
        {
            //Diamond Step
            DiamondStep(numOfSquareRows, numOfSquaresInRow, squareLength, count, rangeValue, useRandomValues);

            numOfSquares *= 4;
            numOfSquareRows *= 2;
            numOfSquaresInRow *= 2;

            //Square Step
            SquareStep(numOfDiamondRows, numOfDiamondsInRows, squareLength, rangeValue, useRandomValues);

            rangeValue /= 2;
            numOfDiamondRows *= 2;
            numOfDiamondsInRows *= 2;
            squareLength /= 2;
            count++;
        }
    }

    private void DiamondStep(int numOfSquareRows, int numOfSquaresInRow, int squareLength, float count, float rangeValue, bool useRandomValues)
    {
        for (int i = 0; i < numOfSquareRows; i++)
        {
            for (int j = 0; j < numOfSquaresInRow; j++)
            {
                int rowOffset = (_Width * squareLength * i);
                int squareOffset = (squareLength * j);
                int center = rowOffset + squareOffset + (int)((_Size - 1) / Mathf.Pow(2.0f, count));
                //Top left
                _ValueMap[center] += _ValueMap[rowOffset + squareOffset];
                //Top right
                _ValueMap[center] += _ValueMap[rowOffset + squareOffset + squareLength];
                //Bottom left
                _ValueMap[center] += _ValueMap[rowOffset + squareOffset + (_Width * squareLength)];
                //Bottom right
                _ValueMap[center] += _ValueMap[rowOffset + squareOffset + (_Width * squareLength) + squareLength];

                _ValueMap[center] /= 4;
 
                if (useRandomValues)
                    _ValueMap[center] += UnityEngine.Random.Range(-rangeValue, rangeValue);
                else
                    _ValueMap[center] += rangeValue;
            }
        }
    }

    private void SquareStep(int numOfDiamondRows, int numOfDiamondsInRows, int squareLength, float rangeValue, bool useRandomValues)
    {
        for (int i = 0; i < numOfDiamondRows; i++)
        {
            for (int j = 0; j < numOfDiamondsInRows; j++)
            {
                //Since diamond centers alternate based on row, mod is used to adjust the diamond center offset
                int center = ((squareLength / 2) * (((i + 1) % 2) + (i * _Width))) + (squareLength * j);
                //Leftmost diamond corner
                if (j == 0 && (i % 2) != 0)
                {
                    _ValueMap[center] += _ValueMap[center + ((_Width - 1) - (squareLength / 2))];
                }
                else
                {
                    _ValueMap[center] += _ValueMap[center - (squareLength / 2)];
                }
                //Topmost diamond corner
                if (i == 0)
                {
                    //In this case the topmost corner is the same as the bottommost 
                    _ValueMap[center] += _ValueMap[center + ((squareLength / 2) * _Width)];
                }
                else
                {
                    _ValueMap[center] += _ValueMap[center - ((squareLength / 2) * _Width)];
                }
                //Rightmost diamond corner
                _ValueMap[center] += _ValueMap[center + (squareLength / 2)];
                //Bottommost diamond corner
                _ValueMap[center] += _ValueMap[center + ((squareLength / 2) * _Width)];
                _ValueMap[center] /= 4.0f;
 
                if (useRandomValues)
                    _ValueMap[center] += UnityEngine.Random.Range(-rangeValue, rangeValue);
                else
                    _ValueMap[center] += rangeValue;

                if (i == 0)
                {
                    //wrap around center
                    //This fills the values of the centers at the bottom of the array
                    int adjustedCenter = center + _Width * (_Height - 1);
                    //Leftmost diamond corner
                    _ValueMap[adjustedCenter] += _ValueMap[adjustedCenter - (squareLength / 2)];
                    //Topmost diamond corner
                    _ValueMap[adjustedCenter] += _ValueMap[adjustedCenter - ((squareLength / 2) * _Width)];
                    //Rightmost diamond corner
                    _ValueMap[adjustedCenter] += _ValueMap[adjustedCenter + (squareLength / 2)];
                    //Bottommost diamond corner
                    //In this case the bottommost corner is the same as the topmost 
                    _ValueMap[adjustedCenter] += _ValueMap[adjustedCenter - ((squareLength / 2) * _Width)];
                    
                    _ValueMap[adjustedCenter] /= 4.0f;
                    if (useRandomValues)
                        _ValueMap[adjustedCenter] += UnityEngine.Random.Range(-rangeValue, rangeValue);
                    else
                        _ValueMap[adjustedCenter] += rangeValue;
                }
                else if (j == 0 && (i % 2) != 0)
                {
                    //wrap around center
                    _ValueMap[center + (_Width - 1)] = _ValueMap[center];
                }
            }
        }
    }

    public void ShiftFractalToAverage()
    {
        ShiftAllValues(-GetAverageValue());
    }

    private float GetAverageValue()
    {
        for (int i = 0; i < _Size; i++)
        {
            _AverageValue += _ValueMap[i];
        }
        _AverageValue /= _Size;
        return _AverageValue;
    }

    private void ShiftAllValues(float value)
    {
        for (int i = 0; i < _Size; i++)
        {
            _ValueMap[i] += value;
        }
    }



    public float[] GetFractal()
    {
        return _ValueMap;
    }
    
    //Returns a slightly smaller fractal since all the corners of the fractal are duplicates and the world wraps around
    public float[] GetReducedFractal()
    {
            float[] reducedFractal = new float[(_Width - 1) * (_Width - 1)];
            for (int i = 0; i < _Width - 1; i++)
            {
                for (int j = 0; j < _Width - 1; j++)
                {
                    reducedFractal[i + j * (_Width - 1)] = _ValueMap[i + j * (_Width)];
                }
            }
            return reducedFractal;
    }

    public float[][] Get2DReducedFractal()
    {
        float[][] reducedFractal = new float[(_Width - 1)][];

        for (int i = 0; i < _Width - 1; i++)
            reducedFractal[i] = new float[_Width - 1];

        for (int i = 0; i < _Width - 1; i++)
        {
            for (int j = 0; j < _Width - 1; j++)
            {
                reducedFractal[i][j] = _ValueMap[i + j * (_Width)];
            }
        }
        return reducedFractal;
    }

}
