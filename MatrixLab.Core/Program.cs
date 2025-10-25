using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLab.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Створюємо матрицю
            Matrix a = new Matrix(2, 3);

            // 2. Виводимо результат в консоль
            // Console.WriteLine() АВТОМАТИЧНО викликає .ToString() для будь-якого об'єкта
            Console.WriteLine("Наша матриця 'a':");
            Console.WriteLine(a); // Цього достатньо

        }
    }
}