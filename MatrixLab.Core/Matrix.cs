using System;
using System.Text; // Потрібен для StringBuilder у ToString

namespace MatrixLab.Core
{
    public class Matrix
    {
        // Дані класу (приватні поля)

        private readonly int _rows;
        private readonly int _cols;
        private readonly double[,] _data; // Масив для зберігання даних

        // Властивості класу

        public int Rows => _rows; // Скорочений запис для { get { return _rows; } }
        public int Cols => _cols;

        // Індексатор що дозволяє звертатися до елементів матриці як до масиву: matrix[i, j]
        public double this[int row, int col]
        {
            get
            {
                // Спочатку перевіряємо індекси
                ValidateIndices(row, col);

                // Якщо все добре, повертаємо значення
                return _data[row, col];
            }
            set
            {
                ValidateIndices(row, col);
                _data[row, col] = value;
            }
        }

        // Конструктори

        /// <summary>
        /// Створює матрицю заданого розміру, заповнену нулями.
        /// </summary>
        public Matrix(int rows, int cols)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Розміри матриці мають бути додатними.");

            _rows = rows;
            _cols = cols;
            _data = new double[rows, cols]; // За замовчуванням заповнюється нулями
        }

        /// <summary>
        /// Створює матрицю на основі існуючого 2D-масиву.
        /// </summary>
        public Matrix(double[,] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            _rows = data.GetLength(0);
            _cols = data.GetLength(1);
            _data = new double[_rows, _cols];

            // Глибоке копіювання, щоб уникнути впливу зміни зовнішнього масиву користувача на нашу матрицю
            Array.Copy(data, _data, data.Length);
        }

        /// <summary>
        /// Конструктор копіювання.
        /// Створює глибоку, незалежну копію іншої матриці.
        /// </summary>
        public Matrix(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            _rows = other.Rows;
            _cols = other.Cols;
            _data = new double[_rows, _cols];

            // Виконуємо глибоке копіювання даних, щоб матриці були незалежними одна від одної
            Array.Copy(other._data, this._data, other._data.Length);
        }

        // Допоміжні методи (Equals, GetHashCode, ToString)

        public override bool Equals(object obj)
        {
            if (!(obj is Matrix other))
                return false;

            if (this.Rows != other.Rows || this.Cols != other.Cols)
                return false;

            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Cols; j++)
                {
                    if (this[i, j] != other[i, j])
                        return false;
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            unchecked // Дозволяємо арифметичне переповнення, це нормально для хеш-кодів
            {
                int hash = 17; // Початкове просте число

                // Додаємо поля, від яких залежить Equals
                hash = hash * 31 + _rows.GetHashCode();
                hash = hash * 31 + _cols.GetHashCode();

                // Додаємо всі елементи
                for (int i = 0; i < this.Rows; i++)
                {
                    for (int j = 0; j < this.Cols; j++)
                    {
                        hash = hash * 31 + this[i, j].GetHashCode();
                    }
                }
                return hash;
            }
        }

        public override string ToString()
        {
            // Створюємо порожній контейнер для майбутнього рядка
            var sb = new StringBuilder();

            // Починаємо цикл по кожному РЯДКУ матриці
            for (int i = 0; i < Rows; i++)
            {
                // На початку кожного рядка додаємо в кінець контейнера "[ "
                sb.Append("[ ");

                // Запускаємо цикл по кожному СТОВПЦЮ всередині поточного рядка
                for (int j = 0; j < Cols; j++)
                {
                    // Додаємо відформатоване число
                    sb.AppendFormat("{0,8:F2} ", this[i, j]);
                }

                // Коли рядок закінчився, додаємо "]" і символ "нового рядка", щоб наступний рядок почався з нової лінії
                sb.AppendLine("]");
            }

            // Коли всі рядки оброблені, перетворюємо наш контейнер StringBuilder на звичайний string
            return sb.ToString();
        }

        // Функції класу

        public Matrix Add(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (this.Rows != other.Rows || this.Cols != other.Cols)
                throw new ArgumentException("Матриці мають бути однакового розміру для додавання.");

            Matrix result = new Matrix(this.Rows, this.Cols);
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Cols; j++)
                {
                    result[i, j] = this[i, j] + other[i, j];
                }
            }

            return result;
        }

        public Matrix Multiply(double scalar)
        {
            var result = new Matrix(this.Rows, this.Cols);

            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Cols; j++)
                {
                    result[i, j] = this[i, j] * scalar;
                }
            }

            return result;
        }

        public Matrix Multiply(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            // Для множення (A * B), кількість стовпців A має дорівнювати кількості рядків B
            if (this.Cols != other.Rows)
                throw new ArgumentException("Кількість стовпців першої матриці " +
                    "має дорівнювати кількості рядків другої матриці для множення.");

            // Розмір результату буде (this.Rows x other.Cols)
            var result = new Matrix(this.Rows, other.Cols);

            // Алгоритм множення
            // i - для проходу по РЯДКАХ першої матриці (і рядках результату)
            for (int i = 0; i < result.Rows; i++)
            {
                // j - для проходу по СТОВПЦЯХ другої матриці (і стовпцях результату)
                for (int j = 0; j < result.Cols; j++)
                {
                    // k - для "внутрішнього" циклу (прохід по стовпцях 'this' і рядках 'other')
                    double sum = 0;
                    for (int k = 0; k < this.Cols; k++)
                    {
                        sum += this[i, k] * other[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return result;
        }
        public double GetDeterminant()
        {
            // Перевірка на квадратність
            if (this.Rows != this.Cols)
                throw new InvalidOperationException("Визначник можна знайти лише для квадратної матриці.");

            // Базовий випадок: Матриця 1x1
            if (this.Rows == 1)
                return this[0, 0];

            // Базовий випадок: Матриця 2x2
            if (this.Rows == 2)
            {
                // Формула: (a*d - b*c)
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            }

            // Рекурсивний випадок: Матриця 3x3 або більше
            // Використовуємо розклад по першому рядку (i = 0)

            double determinant = 0;

            for (int j = 0; j < this.Cols; j++)
            {
                // Отримуємо мінор (матриця без 0-го рядка та j-го стовпця)
                Matrix minor = CreateMinor(0, j);

                // Визначаємо знак (алгебраїчне доповнення)
                double sign = (j % 2 == 0) ? 1 : -1;

                // Додаємо до суми: Знак * Елемент * Визначник_Мінора
                determinant += sign * this[0, j] * minor.GetDeterminant();
            }

            return determinant;
        }

        // Приватні допоміжні методи
        /// <summary>
        /// Приватний метод для перевірки, чи індекси не виходять за межі матриці.
        /// </summary>
        private void ValidateIndices(int row, int col)
        {
            if (row < 0 || row >= _rows)
            {
                throw new ArgumentOutOfRangeException(nameof(row),
                    $"Індекс рядка ({row}) виходить за межі. Допустимий діапазон: [0..{_rows - 1}].");
            }

            if (col < 0 || col >= _cols)
            {
                throw new ArgumentOutOfRangeException(nameof(col),
                    $"Індекс стовпця ({col}) виходить за межі. Допустимий діапазон: [0..{_cols - 1}].");
            }
        }

        /// <summary>
        /// Створює мінор - матрицю N-1 x N-1, видаливши вказаний рядок та стовпець.
        /// </summary>
        private Matrix CreateMinor(int rowToRemove, int colToRemove)
        {
            var minor = new Matrix(this.Rows - 1, this.Cols - 1);
            int minorRow = 0;

            for (int i = 0; i < this.Rows; i++)
            {
                // Пропускаємо рядок, який треба видалити
                if (i == rowToRemove)
                    continue;

                int minorCol = 0;
                for (int j = 0; j < this.Cols; j++)
                {
                    // Пропускаємо стовпець, який треба видалити
                    if (j == colToRemove)
                        continue;

                    // Копіюємо значення в нову матрицю
                    minor[minorRow, minorCol] = this[i, j];
                    minorCol++;
                }
                minorRow++;
            }
            return minor;
        }
    }
}