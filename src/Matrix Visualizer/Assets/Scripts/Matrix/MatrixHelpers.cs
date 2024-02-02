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
            if (m.data == null || m.data.GetLength(1) != 1 || m.data.GetLength(0) == 0)
                throw new ArgumentException("Invalid vector.");


            float[,] result = new float[m.data.GetLength(0) + 1, m.data.GetLength(1)];

            for (int i = 0; i < m.data.GetLength(0); i++)
                result[i, 0] = m.data[i, 0];

            result[m.data.GetLength(0), 0] = 1; // set the last row to 1

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
        /// Creates a 3D rotation matrix for x , y and z axes.
        /// </summary>        
        public static Matrix rotation3D(float? angleX = null, float? angleY = null, float? angleZ = null) 
        => rotation3Dx(angleX ?? 0) * rotation3Dy(angleY ?? 0) * rotation3Dz(angleZ ?? 0);
        /// <summary>
        /// Method to concatenate matrices by multiplying them.
        /// </summary>
        public static Matrix Concatenation(Matrix[] ms) => ms.Aggregate((first, second) => first * second);
        public static Matrix WorldSpaceTransformationMatrix(float[] ts = null, float[] ss = null, float[] rs = null)
        {
            /// <summary>
            /// Returns the world space transformation matrix for the given translation, scaling and rotation vectors.
            /// WST = T * R * S * I = T * Rz * Ry * Rx * S * I
            /// </summary>

            if (ts != null && ts.Length != 3 || ss != null && ss.Length != 3 || rs != null && rs.Length != 3)
                throw new ArgumentException("The translation, scaling and rotation vectors must be 3D vectors.");

            Matrix t = (ts != null) ? MatrixHelpers.translationMatrix(ts) : Matrix.identity(4);
            Matrix s = (ss != null) ? MatrixHelpers.scalingMatrix(ss) : Matrix.identity(4);

            float angleX = (rs != null) ? rs[0] : 0f;
            float angleY = (rs != null) ? rs[1] : 0f;
            float angleZ = (rs != null) ? rs[2] : 0f;

            Matrix r = MatrixHelpers.rotation3Dx(angleX) * MatrixHelpers.rotation3Dy(angleY) * MatrixHelpers.rotation3Dz(angleZ);
            Matrix i = Matrix.identity(4); // why tho

            return MatrixHelpers.Concatenation(new Matrix[] {t, r, s, i});
        }
        public static Matrix ObjectSpaceTransformationMatrix(float[] ts = null, float[] ss = null, float[] rs = null)
        {
            /// <summary>
            /// Returns the object space transformation matrix for the given translation, scaling and rotation vectors.
            /// OST = S * R * T * I = S * Rx * Ry * Rz * T * i
            /// </summary>                   
            if (ts != null && ts.Length != 3 || ss != null && ss.Length != 3 || rs != null && rs.Length != 3)
                throw new ArgumentException("The translation, scaling and rotation vectors must be 3D vectors.");

            Matrix t = (ts != null) ? MatrixHelpers.translationMatrix(ts) : Matrix.identity(4);
            Matrix s = (ss != null) ? MatrixHelpers.scalingMatrix(ss) : Matrix.identity(4);

            float angleX = (rs != null) ? rs[0] : 0f;
            float angleY = (rs != null) ? rs[1] : 0f;
            float angleZ = (rs != null) ? rs[2] : 0f;

            Matrix r = MatrixHelpers.rotation3Dz(angleZ) * MatrixHelpers.rotation3Dy(angleY) * MatrixHelpers.rotation3Dx(angleX);
            Matrix i = Matrix.identity(4); // why tho

            return MatrixHelpers.Concatenation(new Matrix[] {s, r, t, i});
        }
        /// <summary>
        /// Returns the camera space matrix based on the camera's world space transformation matrix.
        /// CSM = (WST)⁻¹
        /// </summary>
        public static Matrix CameraViewSpaceMatrix(Matrix cameraWST) => ~cameraWST;
        public static Matrix PerspectiveProjectionMatrix(float fov, float aspect, float near, float far)
        {
            /// <summary>
            /// Returns the perspective projection matrix for the given field of view, aspect ratio, near and far planes.
            /// </summary>            
            float f = 1 / (float)Math.Tan(deg2Rad(fov) / 2);
            float z1 = -(far + near) / (far - near);
            float z2 = -2 * far * near / (far - near);

            return new Matrix(new float[,]
                           { { f / aspect, 0,      0,       0 },
                             { 0,          f,      0,       0 },
                             { 0,          0,     z1,      z2 },
                             { 0,          0,     -1,       0 } });
        }
        public static Matrix OrthographicProjectionMatrix(float left, float right, float bottom, float top, float near, float far)
        {
            /// <summary>
            /// Returns the orthographic projection matrix for the given left, right, bottom, top, near and far planes.
            /// </summary>            
            float m1 = 2 / (right - left);
            float m2 = 2 / (top - bottom);
            float m3 = -2 / (far - near);
            float n1 = -(right + left) / (right - left);
            float n2 = -(top + bottom) / (top - bottom);
            float n3 = -(far + near) / (far - near);

            return new Matrix(new float[,]
                           { { m1, 0, 0, n1 },
                             { 0, m2, 0, n2 },
                             { 0, 0, m3, n3 },
                             { 0, 0,  0,  1 } });
        }
        public static void Pivot(float[,] augmentedMatrix, int i)
        {
            /// <summary>
            /// Pivots the augmented matrix to make the diagonal element of the current row 1.
            /// </summary>
            int n = augmentedMatrix.GetLength(0);
            float diagonal = augmentedMatrix[i, i];

            for (int j = 0; j < 2 * n; j++)
                augmentedMatrix[i, j] /= diagonal; // divide the current row by the diagonal element

            for (int k = 0; k < n; k++)
            {
                if (k != i)
                {
                    float factor = augmentedMatrix[k, i];

                    for (int j = 0; j < 2 * n; j++)
                        augmentedMatrix[k, j] -= factor * augmentedMatrix[i, j]; // subtract the factor times the current row from the other rows
                }
            }
        }
    }   
}