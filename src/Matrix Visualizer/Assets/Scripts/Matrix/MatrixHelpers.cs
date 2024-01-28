/// <summary>
/// Hey Chris, I've written every class in one file right now. (This is not a good practice though)
/// I did it so that you can easily run and test the code in any online C# compiler without having to install C# locally.
/// You can test here: https://www.programiz.com/csharp-programming/online-compiler/
/// </summary>
using System;
using System.Linq;

namespace MatrixLibrary
{    
    internal class MatrixHelpers
    {
        /// <summary>
        /// Helper class that contains some static functions to help the Matrix class.
        /// I put things that were not directly related to the mathematics of Matrices in this class.
        /// </summary>
        public static int GetMaxElementWidth(float[,] data)
        {
            /// <return>
            /// The width of the largest element in the matrix.
            /// </return>
            int maxWidth = 0;
            foreach (int value in data)
            {
                int length = value.ToString().Length;
                if (length > maxWidth)
                    maxWidth = length;
            }
            return maxWidth;
        }
        /// <return>
        /// If the given 2D array of floats is a valid matrix.
        /// </return>
        public static bool IsValidMatrix(float[,] data) => !(data == null || data.GetLength(0) == 0 || data.GetLength(1) == 0);        
        public static Matrix HomogenizeVector(Matrix m)
        {
            /// <summary>
            /// Homogenizes a given matrix.
            /// </summary>
            if (m.data == null || m.data.GetLength(0) != 1 || m.data.GetLength(1) == 0)
                throw new ArgumentException("Invalid vector.");


            float[,] result = new float[m.data.GetLength(0), m.data.GetLength(1) + 1];

            for (int i = 0; i < m.data.GetLength(1); i++)
                result[0, i] = m.data[0, i];

            result[0, m.data.GetLength(1)] = 1; // set the last element to 1

            return new Matrix(result);
        }
        public static Matrix HomogenizeMatrix(Matrix m)
        {
            /// <summary>
            /// Homogenizes a given matrix by adding a column of ones.
            /// This is typically used to convert a 3D point for affine transformations.
            /// </summary>
            if (m.data == null || m.data.GetLength(0) == 0 || m.data.GetLength(1) == 0)
                throw new ArgumentException("Invalid matrix data for homogenization.");            
            
            float[,] result = new float[m.data.GetLength(0) + 1, m.data.GetLength(1) + 1];

            for (int i = 0; i < m.data.GetLength(0); i++)            
                for (int j = 0; j < m.data.GetLength(1); j++)                
                    result[i, j] = m.data[i, j];

            result[m.data.GetLength(0), m.data.GetLength(1)] = 1; // set the last element to 1

            return new Matrix(result);
        }
        public static Matrix translationMatrix(float[] t)
        {
            /// <summary>
            /// Creates a translation matrix from the given translation vector.
            /// Can be used to translate a point of any dimension.
            /// </summary>
            if (t == null || t.Length == 0)
                throw new ArgumentException("Invalid translation vector.");

            int dimension = t.Length;

            Matrix result = Matrix.identity(dimension + 1);

            for (int i = 0; i < dimension; i++)
                result.data[i, dimension] = t[i]; // set the last column to the translation vector

            return result;
        }
        public static Matrix scalingMatrix(float[] s)
        {
            /// <summary>
            /// Creates a scaling matrix from the given scaling vector.
            /// Can be used to scale a point of any dimension.
            /// </summary>
            if (s == null || s.Length == 0)
                throw new ArgumentException("Invalid scaling vector.");

            int dimension = s.Length;

            Matrix result = Matrix.identity(dimension + 1);

            for (int i = 0; i < dimension; i++)
                result.data[i, i] = s[i]; // set the diagonal elements to the scaling vector

            return result;
        }
        /// <summary>
        /// Methods to convert between degrees and radians.
        /// </summary>
        public static float deg2Rad(float angle) => (float)(Math.PI * angle / 180);
        public static float rad2Deg(float angle) => (float)(angle * 180 / Math.PI);
        /// <summary>
        /// Creates a 2D rotation matrix from the given angle.
        /// Rotates the point along the axis perpendicular to the plane.
        /// </summary>
        public static Matrix rotation2D(float angle) => new Matrix(new float[,]
        { { (float)Math.Cos(deg2Rad(angle)), -(float)Math.Sin(deg2Rad(angle)) },
        { (float)Math.Sin(deg2Rad(angle)), (float)Math.Cos(deg2Rad(angle)) } });
        /// <summary>
        /// The next three functions create 3D rotation matrices about the x, y and z axes respectively.
        /// </summary>
        public static Matrix rotation3Dx(float angle) => MatrixHelpers.HomogenizeMatrix(new Matrix(new float[,]
        { { 1, 0, 0 },
        { 0, (float)Math.Cos(deg2Rad(angle)), -(float)Math.Sin(deg2Rad(angle)) },
        { 0, (float)Math.Sin(deg2Rad(angle)), (float)Math.Cos(deg2Rad(angle)) } }));
        public static Matrix rotation3Dy(float angle) => MatrixHelpers.HomogenizeMatrix(new Matrix(new float[,]
        { { (float)Math.Cos(deg2Rad(angle)), 0, (float)Math.Sin(deg2Rad(angle)) },
        { 0, 1, 0 },
        { -(float)Math.Sin(deg2Rad(angle)), 0, (float)Math.Cos(deg2Rad(angle)) } }));
        public static Matrix rotation3Dz(float angle) => MatrixHelpers.HomogenizeMatrix(new Matrix(new float[,]
        { { (float)Math.Cos(deg2Rad(angle)), -(float)Math.Sin(deg2Rad(angle)), 0 },
        { (float)Math.Sin(deg2Rad(angle)), (float)Math.Cos(deg2Rad(angle)), 0 },
        { 0, 0, 1 } }));
        /// <summary>
        /// Method to concatenate matrices by multiplying them.
        /// </summary>
        public static Matrix Concatenation(Matrix m, Matrix[] ms) => ms.Aggregate(m, (acc, next) => acc * next);
    }   
}