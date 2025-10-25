using MatrixLab.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MatrixLab.Tests
{
    [TestClass]
    public class MatrixTests
    {
        // Тести для Конструкторів та Властивостей
        // Ці тести мають пройти, бо реалізовані конструктори

        [TestMethod]
        public void Constructor_ValidDimensions_CreatesMatrix()
        // Сценарій: Створення матриці з коректними розмірами (3x4)
        // Очікування: Властивості Rows та Cols відповідають заданим
        {
            // Arrange
            int rows = 3;
            int cols = 4;

            // Act
            var matrix = new Matrix(rows, cols);

            // Assert
            Assert.AreEqual(rows, matrix.Rows);
            Assert.AreEqual(cols, matrix.Cols);
        }

        [TestMethod]
        public void Constructor_From2DArray_CopiesDataCorrectly()
        // Сценарій: Створення матриці з існуючого 2D-масиву
        // Очікування: Розміри та елементи матриці відповідають масиву
        {
            // Arrange
            double[,] data = { { 1.1, 2.2 }, { 3.3, 4.4 } };

            // Act
            var matrix = new Matrix(data);

            // Assert
            Assert.AreEqual(2, matrix.Rows);
            Assert.AreEqual(2, matrix.Cols);
            Assert.AreEqual(1.1, matrix[0, 0]);
            Assert.AreEqual(4.4, matrix[1, 1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_ZeroDimensions_ThrowsArgumentException()
        // Сценарій: Спроба створити матрицю з розміром 0x5
        // Очікування: Конструктор кидає ArgumentException
        {
            // Arrange, Act, Assert
            new Matrix(0, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_NegativeDimensions_ThrowsArgumentException()
        // Сценарій: Спроба створити матрицю з розміром 3x(-1)
        // Очікування: Конструктор кидає ArgumentException
        {
            // Arrange, Act, Assert
            new Matrix(3, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_FromNullArray_ThrowsArgumentNullException()
        // Сценарій: Спроба створити матрицю з null-масиву
        // Очікування: Конструктор кидає ArgumentNullException
        {
            // Arrange, Act, Assert
            new Matrix((double[,])null);
        }

        [TestMethod]
        public void CopyConstructor_CreatesIndependentCopy()
        // Сценарій: Створення копії матриці m1. Зміна копії m2
        // Очікування: Оригінальна матриця m1 не змінилася
        {
            // Arrange
            var m1 = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var m2 = new Matrix(m1);

            // Act
            m2[0, 0] = 99;

            // Assert
            Assert.AreEqual(1, m1[0, 0]); // Перевіряємо, що оригінал не змінився
            Assert.AreEqual(99, m2[0, 0]); // Перевіряємо, що копія змінилася
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyConstructor_NullOriginal_ThrowsArgumentNullException()
        // Сценарій: Спроба скопіювати null-матрицю
        // Очікування: Конструктор кидає ArgumentNullException
        {
            // Arrange, Act, Assert
            new Matrix((Matrix)null);
        }

        // --- 2. Тести для Індексатора ---
        // Ці тести мають пройти, бо реалізован індексатор

        [TestMethod]
        public void Indexer_SetAndGet_ValidIndices_WorksCorrectly()
        // Сценарій: Запис значення 5.0 в комірку [1, 1] та його читання
        // Очікування: Отримане значення дорівнює 5.0
        {
            // Arrange
            var m = new Matrix(3, 3);

            // Act
            m[1, 1] = 5.0;
            double value = m[1, 1];

            // Assert
            Assert.AreEqual(5.0, value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerGet_RowOutOfRange_ThrowsArgumentOutOfRangeException()
        // Сценарій: Спроба отримати значення з рядка 99 (для матриці 3x3)
        // Очікування: Кидає ArgumentOutOfRangeException
        {
            // Arrange
            var m = new Matrix(3, 3);
            // Act
            double value = m[99, 1];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerSet_ColOutOfRange_ThrowsArgumentOutOfRangeException()
        // Сценарій: Спроба записати значення у стовпець -1
        // Очікування: Кидає ArgumentOutOfRangeException
        {
            // Arrange
            var m = new Matrix(3, 3);
            // Act
            m[1, -1] = 10;
        }

        // Тести для Допоміжних Методів (Equals, ToString)
        // Ці тести мають пройти

        [TestMethod]
        public void Equals_TwoIdenticalMatrices_ReturnsTrue()
        // Сценарій: Порівняння двох матриць, створених з однакових масивів
        // Очікування: Equals повертає true
        {
            // Arrange
            var m1 = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var m2 = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            // Act
            bool result = m1.Equals(m2);
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_DifferentDimensions_ReturnsFalse()
        // Сценарій: Порівняння матриць 2x2 та 3x3
        // Очікування: Equals повертає false
        {
            // Arrange
            var m1 = new Matrix(2, 2);
            var m2 = new Matrix(3, 3);
            // Act
            bool result = m1.Equals(m2);
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_DifferentData_ReturnsFalse()
        // Сценарій: Порівняння матриць з різними елементами
        // Очікування: Equals повертає false
        {
            // Arrange
            var m1 = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var m2 = new Matrix(new double[,] { { 1, 2 }, { 3, 99 } });
            // Act
            bool result = m1.Equals(m2);
            // Assert
            Assert.IsFalse(result);
        }

        // =================================================================
        // Тести для TDD Методів (Мають Провалитися!)
        // Це тести для методів Add, Multiply, GetDeterminant
        // Вони будуть ЧЕРВОНИМИ, бо методи кидають NotImplementedException
        // =================================================================

        // Тести для Add

        [TestMethod]
        public void Add_Two2x2Matrices_ReturnsCorrectSum()
        // Сценарій: Додавання двох матриць 2x2
        // Очікування: Кожен елемент результуючої матриці є сумою відповідних елементів
        {
            // Arrange
            var m1 = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var m2 = new Matrix(new double[,] { { 5, 6 }, { 7, 8 } });
            var expected = new Matrix(new double[,] { { 6, 8 }, { 10, 12 } });

            // Act
            var result = m1.Add(m2);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_MismatchedDimensions_ThrowsArgumentException()
        // Сценарій: Спроба додати матриці 2x2 та 3x3
        // Очікування: Кидає ArgumentException
        {
            // Arrange
            var m1 = new Matrix(2, 2);
            var m2 = new Matrix(3, 3);

            // Act
            m1.Add(m2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_NullMatrix_ThrowsArgumentNullException()
        // Сценарій: Спроба додати "null" до існуючої матриці
        // Очікування: Кидає ArgumentNullException
        {
            // Arrange
            var m1 = new Matrix(2, 2);
            Matrix m2 = null; // Створюємо null-посилання

            // Act
            m1.Add(m2);
        }

        // Тести для Multiply (scalar)

        [TestMethod]
        public void Multiply_MatrixByScalar_ReturnsCorrectResult()
        // Сценарій: Множення матриці 2x2 на число 3
        // Очікування: Кожен елемент матриці помножено на 3
        {
            // Arrange
            var m = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var expected = new Matrix(new double[,] { { 3, 6 }, { 9, 12 } });

            // Act
            var result = m.Multiply(3);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Multiply_MatrixByZero_ReturnsZeroMatrix()
        // Сценарій: Множення матриці на скаляр 0
        // Очікування: Повертає матрицю того ж розміру, заповнену нулями
        {
            // Arrange
            var m = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var expected = new Matrix(2, 2);

            // Act
            var result = m.Multiply(0);

            // Assert
            Assert.AreEqual(expected, result);
        }

        // Тести для Multiply (matrix)

        [TestMethod]
        public void Multiply_Two2x2Matrices_ReturnsCorrectProduct()
        // Сценарій: Множення двох матриць 2x2
        // Очікування: Повертає коректний добуток
        {
            // Arrange
            var m1 = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var m2 = new Matrix(new double[,] { { 5, 6 }, { 7, 8 } });
            // (1*5 + 2*7) = 19 | (1*6 + 2*8) = 22
            // (3*5 + 4*7) = 43 | (3*6 + 4*8) = 50
            var expected = new Matrix(new double[,] { { 19, 22 }, { 43, 50 } });

            // Act
            var result = m1.Multiply(m2);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Multiply_RectangularMatrices_ReturnsCorrectProduct()
        // Сценарій: Множення матриці 2x3 на 3x2
        // Очікування: Повертає коректний добуток 2x2
        {
            // Arrange
            var m1 = new Matrix(new double[,] { { 1, 2, 3 },
                                                { 4, 5, 6 } }); // 2x3

            var m2 = new Matrix(new double[,] { { 7, 8 },
                                                { 9, 10 },
                                                { 11, 12 } }); // 3x2

            // Очікуваний результат 2x2:
            // (1*7 + 2*9 + 3*11) = 7 + 18 + 33 = 58  | (1*8 + 2*10 + 3*12) = 8 + 20 + 36 = 64
            // (4*7 + 5*9 + 6*11) = 28 + 45 + 66 = 139 | (4*8 + 5*10 + 6*12) = 32 + 50 + 72 = 154
            var expected = new Matrix(new double[,] { { 58, 64 }, { 139, 154 } });

            // Act
            var result = m1.Multiply(m2);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Multiply_InvalidDimensions_ThrowsArgumentException()
        // Сценарій: Спроба помножити матрицю 3x2 на 3x3
        // Очікування: Кидає ArgumentException
        {
            // Arrange
            var m1 = new Matrix(3, 2);
            var m2 = new Matrix(3, 3);

            // Act
            m1.Multiply(m2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Multiply_NullMatrix_ThrowsArgumentNullException()
        // Сценарій: Спроба помножити матрицю на "null"
        // Очікування: Кидає ArgumentNullException
        {
            // Arrange
            var m1 = new Matrix(2, 2);
            Matrix m2 = null;

            // Act
            m1.Multiply(m2);
        }

        // Тести для GetDeterminant

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetDeterminant_NonSquareMatrix_ThrowsInvalidOperationException()
        // Сценарій: Спроба знайти визначник для матриці 2x3
        // Очікування: Кидає InvalidOperationException
        {
            // Arrange
            var m = new Matrix(2, 3);

            // Act
            m.GetDeterminant();
        }

        [TestMethod]
        public void GetDeterminant_2x2Matrix_ReturnsCorrectValue()
        // Сценарій: Обчислення визначника для матриці 2x2
        // Очікування: Повертає (ad - bc)
        {
            // Arrange
            // det = (3 * 6) - (8 * 4) = 18 - 32 = -14
            var m = new Matrix(new double[,] { { 3, 8 }, { 4, 6 } });
            double expected = -14;

            // Act
            double result = m.GetDeterminant();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetDeterminant_3x3Matrix_ReturnsCorrectValue()
        // Сценарій: Обчислення визначника для матриці 3x3
        // Очікування: Повертає коректне значення
        {
            // Arrange
            //|1 2 3|
            //|4 5 6|
            //|7 8 9|
            //1 * |5 6| - 2 * |4 6| + 3 * |4 5|
            //    |8 9|       |7 9|       |7 8|
            // 1*(5*9 - 6*8) - 2*(4*9 - 6*7) + 3*(4*8 - 5*7)
            // 1*(-3) - 2*(-6) + 3*(-3)
            // -3 + 12 - 9 = 0
            var m = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
            double expected = 0;

            // Act
            double result = m.GetDeterminant();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetDeterminant_1x1Matrix_ReturnsElementValue()
        // Сценарій: Обчислення визначника для матриці 1x1
        // Очікування: Повертає єдиний елемент
        {
            // Arrange
            var m = new Matrix(new double[,] { { 15 } });
            double expected = 15;

            // Act
            double result = m.GetDeterminant();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}