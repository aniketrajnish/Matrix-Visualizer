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
        /// <summary>
        /// Methods to convert between degrees and radians.
        /// </summary>
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
        public static Matrix rotation3Dx(float angle) => new Matrix(new float[,]
        { { 1, 0, 0 },
        { 0, (float)Math.Cos(deg2Rad(angle)), -(float)Math.Sin(deg2Rad(angle)) },
        { 0, (float)Math.Sin(deg2Rad(angle)), (float)Math.Cos(deg2Rad(angle)) } });
        public static Matrix rotation3Dy(float angle) => new Matrix(new float[,]
        { { (float)Math.Cos(deg2Rad(angle)), 0, (float)Math.Sin(deg2Rad(angle)) },
        { 0, 1, 0 },
        { -(float)Math.Sin(deg2Rad(angle)), 0, (float)Math.Cos(deg2Rad(angle)) } });
        public static Matrix rotation3Dz(float angle) => new Matrix(new float[,]
        { { (float)Math.Cos(deg2Rad(angle)), -(float)Math.Sin(deg2Rad(angle)), 0 },
        { (float)Math.Sin(deg2Rad(angle)), (float)Math.Cos(deg2Rad(angle)), 0 },
        { 0, 0, 1 } });
    }
    class Program
    {
        /// <summary>
        /// Class that contains the Main method to test the Matrix class.
        /// This class will be removed later.
        /// </summary>        
        static void Main(string[] args)
        {
            /// <summary>
            /// The Main method tests the various functions of the Matrix class and displays the results.
            /// </summary>
            Matrix myMatrix1 = new Matrix(new float[,] { { 1, 2, 5, 7 }, { 3, 4, 6, 8 }, { 10, 11, 12, 15 }, { -1, -2, -5, -6 } }); // initalize and display a matrix
            Console.WriteLine("A = \n" + myMatrix1);

            Matrix myMatrix2 = new Matrix(new float[,] { { 13, 14, 15, 16 }, { 3, 4, 6, 7 }, { 7, 8, 9, 10 }, { -2, -4, -6, -8 } });
            Console.WriteLine("B = \n" + myMatrix2);

            Console.WriteLine("A == B = " + (myMatrix1 == myMatrix2)); // test == and != operators (checking if matrices are equal)
            Console.WriteLine("A != B = " + (myMatrix1 != myMatrix2) + "\n");

            Matrix myMatrix3 = myMatrix1 + myMatrix2; // test + and - operators (addition and subtraction)           
            Matrix myMatrix4 = myMatrix1 - myMatrix2;
            Console.WriteLine("A + B = \n" + myMatrix3);
            Console.WriteLine("A - B = \n" + myMatrix4);

            Matrix myMatrix5 = myMatrix1 * 2; // test * operator with scalar both ways (scalar multiplication)
            Matrix myMatrix6 = 0.5f * myMatrix5;
            Console.WriteLine("A * 2 = \n" + myMatrix5);
            Console.WriteLine("0.5f * A * 2 = \n" + myMatrix6);
            Console.WriteLine("A == 0.5f * A * 2 = " + (myMatrix1 == myMatrix6)); // checking again as this time matrices are equal
            Console.WriteLine("A != 0.5f * A * 2 = " + (myMatrix1 != myMatrix6) + "\n");

            Matrix myMatrix7 = myMatrix1 * myMatrix2; // test * operator with matrix (matrix multiplication)
            Console.WriteLine("A * B = \n" + myMatrix7);

            Matrix myMatrix8 = ~myMatrix1; // test ~ operator (transpose)
            Console.WriteLine("A' = \n" + myMatrix8);

            Matrix myMatrix9 = Matrix.identity(4); // test identity and zero matrix            
            Matrix myMatrix10 = Matrix.zero(4, 4);

            Console.WriteLine("Identity matrix = \n" + myMatrix9);
            Console.WriteLine("Zero matrix = \n" + myMatrix10);

            Matrix myMatrix11 = MatrixHelpers.translationMatrix(new float[] { 1, 2, 3 }); // test translation matrix
            Console.WriteLine("Translation matrix = \n" + myMatrix11);

            Matrix myMatrix12 = new Matrix(new float[,] { { 1, 2, 3 } }); // test translation
            Console.WriteLine("C = \n" + myMatrix12);
            Matrix myMatrix13 = myMatrix12.TranslateMatrix(new float[] { 1, 2, 3 });            
            Console.WriteLine("Translation of C = \n" + myMatrix13);

            Matrix myMatrix14 = MatrixHelpers.scalingMatrix(new float[] { 1, 2, 3 }); // test scaling matrix
            Console.WriteLine("Scaling matrix = \n" + myMatrix14);
            Matrix myMatrix15 = myMatrix12.ScaleMatrix(new float[] { 1, 2, 3 });
            Console.WriteLine("Scaling of C = \n" + myMatrix15);

            Matrix myMatrix16 = MatrixHelpers.rotation2D(45); // test 2D rotation matrix
            Console.WriteLine("2D rotation matrix = \n" + myMatrix16);

            Matrix myMatrix17 = new Matrix(new float[,] { { 3, 7 } }); // test 2D rotation
            Console.WriteLine("D = \n" + myMatrix17);
            Matrix myMatrix18 = myMatrix17.Rotate2D(45);
            Console.WriteLine("2D rotation of D = \n" + myMatrix18);

            Matrix myMatrix19 = MatrixHelpers.rotation3Dx(45); // test 3D rotation matrix
            Console.WriteLine("3D rotation matrix about x axis = \n" + myMatrix19);
            Matrix myMatrix20 = MatrixHelpers.rotation3Dy(45);
            Console.WriteLine("3D rotation matrix about y axis = \n" + myMatrix20);
            Matrix myMatrix21 = MatrixHelpers.rotation3Dz(45);
            Console.WriteLine("3D rotation matrix about z axis = \n" + myMatrix21);

            Matrix myMatrix22 = new Matrix(new float[,] { { 3, 7, 9 } }); // test 3D rotation
            Console.WriteLine("E = \n" + myMatrix22);
            Matrix myMatrix23 = myMatrix22.Rotate3D(rz:45);
            Console.WriteLine("3D rotation of E = \n" + myMatrix23);
        }
    }
}
