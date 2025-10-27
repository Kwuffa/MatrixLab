namespace MatrixLab.Core
{
    public interface IMatrix
    {
        // Властивості
        int Rows { get; }
        int Cols { get; }
        // Індексатор
        double this[int row, int col] { get; set; }

        // Методи
        Matrix Add(Matrix other);
        Matrix Multiply(double scalar);
        Matrix Multiply(Matrix other);
        double GetDeterminant();
    }
}