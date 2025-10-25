namespace MatrixLab.Core
{
    public interface IMatrix
    {
        // Властивості
        /// <summary>
        /// Кількість рядків матриці.
        /// </summary>
        int Rows { get; }

        /// <summary>
        /// Кількість стовпців матриці.
        /// </summary>
        int Cols { get; }

        // Індексатор
        /// <summary>
        /// Доступ до елемента матриці за індексами.
        /// </summary>
        double this[int row, int col] { get; set; }

        // Методи
        /// <summary>
        /// Додає дві матриці.
        /// </summary>
        Matrix Add(Matrix other);

        /// <summary>
        /// Множить матрицю на число.
        /// </summary>
        Matrix Multiply(double scalar);

        /// <summary>
        /// Множить дві матриці.
        /// </summary>
        Matrix Multiply(Matrix other);

        /// <summary>
        /// Обчислює визначник матриці.
        /// </summary>
        double GetDeterminant();
    }
}