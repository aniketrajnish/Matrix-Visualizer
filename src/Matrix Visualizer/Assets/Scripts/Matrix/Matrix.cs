/// <summary>
/// Hey Chris, I've written every class in one file right now. (This is not a good practice though)
/// I did it so that you can easily run and test the code in any online C# compiler without having to install C# locally.
/// You can test here: https://www.programiz.com/csharp-programming/online-compiler/
/// </summary>
using System;
using System.Text;

namespace Matrix
{
    /// <summary>
    /// I decided to put everything in one namespace to easily access the Matrix class and its helper functions.
    /// It also has a test program to test the Matrix class. (will be removed later) 
    /// </summary>
    internal class Matrix
    {
        /// <summary>
        /// This class represents a matrix.
        /// It has a constructor and overrrides for various operators to support matrix operations.        
        /// </summary>
        internal float[,] data;
        public Matrix(float[,] inputData)
        {
            /// <summary>
            /// Constructor for the Matrix class.
            /// It takes a 2D array of floats as input and stores it in the data field.
            /// It also checks if the input data is a valid matrix.
            /// </summary>
            if (!MatrixHelpers.IsValidMatrix(inputData))
                throw new ArgumentException("Invalid matrix data");

            this.data = inputData;
        }
        public static Matrix identity(int size)
        {
            /// <summary>
            /// Creates an identity matrix of the given size. (size x size)
            /// </summary>
            float[,] result = new float[size, size];

            for (int i = 0; i < size; i++)
                result[i, i] = 1; // all diagonal elements

            return new Matrix(result);
        }
        /// <summary>
        /// Creates a zero matrix of the given size. (rows x cols)
        /// </summary>
        public static Matrix zero(int rows, int cols) => new Matrix(new float[rows, cols]);
        public override string ToString()
        {
            /// <summary>
            /// Overides the string representation of the matrix to show the matrix when printed.
            /// </summary>
            StringBuilder builder = new StringBuilder();
            int maxElementWidth = MatrixHelpers.GetMaxElementWidth(data); // for padding based on the row with maximum width

            for (int i = 0; i < data.GetLength(0); i++)
            {
                builder.Append("| "); // start of the row

                for (int j = 0; j < data.GetLength(1); j++)
                    builder.Append(data[i, j].ToString().PadRight(maxElementWidth) + " ");

                builder.AppendLine("|"); // end of the row
            }

            return builder.ToString();
        }
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            /// <summary>
            /// Overloads the + operator to add two matrices.
            /// </summary>
            if (m1.data.GetLength(0) != m2.data.GetLength(0) || m1.data.GetLength(1) != m2.data.GetLength(1))
                throw new ArgumentException("Matrices must have the same dimensions to be added.");

            float[,] result = new float[m1.data.GetLength(0), m1.data.GetLength(1)];

            for (int i = 0; i < m1.data.GetLength(0); i++)
                for (int j = 0; j < m1.data.GetLength(1); j++)
                    result[i, j] = m1.data[i, j] + m2.data[i, j]; // add the corresponding elements

            return new Matrix(result);
        }
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            /// <summary>
            /// Overloads the - operator to subtract two matrices.
            /// </summary>
            if (m1.data.GetLength(0) != m2.data.GetLength(0) || m1.data.GetLength(1) != m2.data.GetLength(1))
                throw new ArgumentException("Matrices must have the same dimensions to be subtracted.");

            float[,] result = new float[m1.data.GetLength(0), m1.data.GetLength(1)];

            for (int i = 0; i < m1.data.GetLength(0); i++)
                for (int j = 0; j < m1.data.GetLength(1); j++) // subtract the corresponding elements of the second matrix from the first
                    result[i, j] = m1.data[i, j] - m2.data[i, j];

            return new Matrix(result);
        }
        public static Matrix operator *(Matrix m, float scalar)
        {
            /// <summary>
            /// Overloads the * operator to multiply a matrix with a scalar.
            /// </summary>
            int rows = m.data.GetLength(0);
            int cols = m.data.GetLength(1);
            float[,] result = new float[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = m.data[i, j] * scalar; // multiply each element with the scalar

            return new Matrix(result);
        }
        /// <summary>
        /// If the order of the operands is reversed.
        /// </summary>
        public static Matrix operator *(float scalar, Matrix m) => m * scalar;
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            /// <summary>
            /// Overloads the * operator to multiply two matrices.
            /// </summary>
            if (m1.data.GetLength(1) != m2.data.GetLength(0)) // no. of cols of m1 should be equal to rows of m2 for multiplication
                throw new ArgumentException("The number of columns in the first matrix must be equal to the number of rows in the second matrix.");

            int rows = m1.data.GetLength(0);
            int cols = m2.data.GetLength(1);
            float[,] result = new float[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    for (int k = 0; k < m1.data.GetLength(1); k++)
                        result[i, j] += m1.data[i, k] * m2.data[k, j]; // dot product of the row of m1 and the column of m2

            return new Matrix(result);
        }
        public static Matrix operator ~(Matrix m)
        {
            /// <summary>
            /// Overloads the ~ operator to transpose a matrix.
            /// </summary>
            int rows = m.data.GetLength(0);
            int cols = m.data.GetLength(1);
            float[,] transposedData = new float[cols, rows];

            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                    transposedData[i, j] = m.data[j, i]; // interchange the rows and columns of the matrix

            return new Matrix(transposedData);
        }
        public Matrix TranslateMatrix(float[] t)
        {
            /// <summary>
            /// Translates the given matrix by the given translation vector.
            /// </summary>                                                            
            int dimension = t.Length;

            if (data.GetLength(0) != 1 || data.GetLength(1) != dimension)
                throw new ArgumentException("The matrix must be a vector of same dimension as the translation vector.");

            Matrix translationMatrix = MatrixHelpers.translationMatrix(t);

            return MatrixHelpers.HomogenizeVector(this) * ~translationMatrix;
        }
        public Matrix ScaleMatrix(float[] s)
        {
            /// <summary>
            /// Scales the given matrix by the given scaling vector.
            /// </summary>                                                            
            int dimension = s.Length;

            if (data.GetLength(0) != 1 || data.GetLength(1) != dimension)
                throw new ArgumentException("The matrix must be a vector of same dimension as the scaling vector.");

            Matrix scalingMatrix = MatrixHelpers.scalingMatrix(s);

            return MatrixHelpers.HomogenizeVector(this) * scalingMatrix;
        }
        public Matrix Rotate2D(float angle)
        {
            /// <summary>
            /// Rotates the given matrix by the given angle.
            /// </summary>                                                            
            if (data.GetLength(0) != 1 || data.GetLength(1) != 2)
                throw new ArgumentException("The matrix must be a 2D vector for 2D rotation");

            Matrix rotationMatrix = MatrixHelpers.rotation2D(angle);

            return this * rotationMatrix;
        }
        public Matrix Rotate3D(float? rx = null, float? ry = null, float? rz = null)
        {
            /// <summary>
            /// Rotates the given matrix by the given angles about the x, y and z axes respectively.
            /// </summary>
            if (data.GetLength(0) != 1 || data.GetLength(1) != 3)
                throw new ArgumentException("The matrix must be a 3D vector for 3D rotation.");

            Matrix rotationMatrix = MatrixHelpers.rotation3Dx(rx ?? 0) * MatrixHelpers.rotation3Dy(ry ?? 0) * MatrixHelpers.rotation3Dz(rz ?? 0); // the total rotation matrix is basically the product of the three rotation matrices

            return this * rotationMatrix;
        }
        public static bool operator ==(Matrix m1, Matrix m2)
        {
            /// <summary>
            /// Overloads the == operator to check if two matrices are equal.
            /// </summary>
            if (m1.data.GetLength(0) != m2.data.GetLength(0) || m1.data.GetLength(1) != m2.data.GetLength(1))
                return false;

            for (int i = 0; i < m1.data.GetLength(0); i++)
                for (int j = 0; j < m1.data.GetLength(1); j++)
                    if (m1.data[i, j] != m2.data[i, j]) // if any corresponding elements are not equal
                        return false;

            return true; // if all corresponding elements are equal
        }
        /// <summary>
        /// Overloads the != operator to check if two matrices are not equal.
        /// </summary>
        public static bool operator !=(Matrix m1, Matrix m2) => !(m1 == m2);
        public override bool Equals(object obj)
        {
            /// <summary>
            /// C# somehow wanted me to override the Equals method if I overrode the == operator.
            /// Its basically hase the same functionality as the == operator.
            /// But it also does some type checking to make sure that the object is a matrix.
            /// </summary>
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            Matrix m = (Matrix)obj;
            if (data.GetLength(0) != m.data.GetLength(0) || data.GetLength(1) != m.data.GetLength(1))
                return false;

            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                    if (data[i, j] != m.data[i, j])
                        return false;

            return true;
        }
        public override int GetHashCode()
        {
            /// <summary>
            /// I had to override this method because I overrode the Equals method (again according to C#).
            /// Idk what this method does, ChatGPT did it :]
            /// </summary>
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                foreach (int value in data)
                {
                    hash = hash * 31 + value;
                }
                return hash;
            }
        }
    }    
}