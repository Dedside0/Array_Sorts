using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApp1
{
    internal class Program
    {
        delegate int[] Rule(int lenght, int min, int max);
        delegate void Sort(int[] arr);

        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Rule[] types = { Increase, Decreasing, Half, Random, NinetyPercent };
            Sort[] sorts = { BubbleSort, InputSort, SelectionSort, ShakeSort };
            int[] arr;

            Console.WriteLine("Введите длинну массива");
            int len = int.Parse(Console.ReadLine());

            Stopwatch timer = Stopwatch.StartNew();

            //Таблица значений
            string[,] excel = new string[types.Length + 1, sorts.Length + 2];
            excel[0, 0] = string.Format("Тип");
            excel[0, 1] = string.Format("Создавался");
            for (int i = 2; i < excel.GetLength(1); i++)
            {
                excel[0, i] = string.Format(sorts[i - 2].Method.Name);
            }

            for (int i = 1; i < excel.GetLength(0); i++)
            {
                excel[i, 0] = string.Format(types[i - 1].Method.Name);
            }

            int row = 1, column = 1;
            PrintInfo(excel, len);
            foreach (var rule in types)
            {
                timer.Start();
                arr = GenerateMassive(rule, len);
                timer.Stop();

                excel[row, column] = string.Format("{0,15}", timer.ElapsedMilliseconds);
                column++;

                PrintInfo(excel, len);

                timer.Reset();
                foreach (var sort in sorts)
                {
                    timer.Start();
                    sort(arr);
                    timer.Stop();

                    excel[row, column] = string.Format("{0,15}", timer.ElapsedMilliseconds);
                    column++;

                    timer.Restart();

                    PrintInfo(excel, len);
                }
                column = 1;
                row++;
            }

        }

        static void PrintInfo(string[,] matrica, int len)
        {
            Console.Clear();
            Console.WriteLine("Все вычисления в миллисекундах.  Длинна массива: " + len);
            PrintTable(matrica);  
        }

        static void PrintTable(string[,] matrica)
        {
            for (int i = 0; i < matrica.GetLength(0); i++)
            {
                for (int j = 0; j < matrica.GetLength(1); j++)
                {
                    Console.Write("|{0,-15}", matrica[i, j]);
                }
                Console.WriteLine();
            }
        }

        //Сортировка Пузырьком
        static void BubbleSort(int[] arr)
        {
            int len = arr.Length; //длинна массива
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        //меняем местами
                        int temp = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
        }
        /// <summary>
        /// Шейкерная сортировка
        /// </summary>
        /// <param name="arr">Массив чисел который нужно отсортировать</param>
        static void ShakeSort(int[] arr)
        {
            //Если в массиве ничего не поменяется, то присвоим true и выйдем из цикла
            bool isEnd = false;
            //левая и права граница массива
            int left = 0;
            int right = arr.Length - 1;
            while (!isEnd)
            {
                isEnd = true;
                //Проходимся слева направо
                for (int i = left; i < right; i++)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        isEnd = false;
                        int temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                    }
                }
                right--;
                if (isEnd) break;

                //Проходимся справа налево
                for (int i = right; i > left; i--)
                {
                    if (arr[i] < arr[i - 1])
                    {
                        isEnd = false;
                        int temp = arr[i];
                        arr[i] = arr[i - 1];
                        arr[i - 1] = temp;
                    }
                }
                left++;
            }
        }

        //Сортировка Вставками
        static void InputSort(int[] arr)
        {
            int len = arr.Length;
            for (int i = 1; i < len; i++)
            {
                //Первое значение из неотсорированной части массива
                int key = arr[i];
                int j = i;
                //сравниваем key с предыдущим и меняем местами если key меньше
                while (j >= 0 && arr[j-1] > key)
                {
                    arr[j] = arr[j-1];
                    j--;
                }
                arr[j ] = key;
            }
        }
        //Сортировка Выбором
        static void SelectionSort(int[] arr)
        {
            int minIndex = 0;
            int len = arr.Length;
            for (int i = 0; i < len; i++)
            {
                minIndex = i;
                for (int j = i; j < len; j++)
                {
                    if (arr[j] < arr[minIndex])
                    {
                        minIndex = j;
                    }
                }
                int temp = arr[0];
                arr[0] = arr[minIndex];
                arr[minIndex] = temp;
            }
        }


        //Рандомный массив
        static int[] Random(int length, int min = 0, int max = 0)
        {
            int[] array = new int[length];
            max = length;
            
            for (int i = 0; i < length; i++)
            {
                array[i] = rnd.Next(min, max + 1);
            }
            return array;
        }

        //По возрастанию
        static int[] Increase(int length, int min, int max)
        {
            int[] arr = new int[length];
            Random rnd = new Random();

            arr[0] = rnd.Next(min, max - length + 1); // Первый элемент в допустимом диапазоне

            for (int i = 1; i < length; i++)
            {
                // Генерируем следующее число больше предыдущего, но в пределах max
                arr[i] = rnd.Next(arr[i - 1] + 1, max - (length - i - 1) + 1);
            }

            return arr;
        }

        //Убывающий массив
        static int[] Decreasing(int length, int min, int max)
        {
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = length - i;
            }
            return array;
        }

        //первая половина убывает, вторая возрастает
        static int[] Half(int length, int min = 0, int max = 0)
        {
            int[] array = new int[length];
            max = length;
            int maxx = max;
            int minn = min;
            for (int i = 0; i < length / 2; i++)
            {
                array[i] = maxx;
                maxx--;
            }
            for (int i = length / 2; i < length; i++)
            {
                array[i] = minn;
                minn++;
            }
            return array;
        }

        //Массив где много одинаковых значений
        static int[] NinetyPercent(int length, int min = 0, int max = 0)
        {
            int[] array = new int[length];
            max = length;
            Random rnd = new Random();
            int val = rnd.Next(min, max);
            for (int i = 0; i < length; i++)
            {
                array[i] = val;
            }
            int n = Convert.ToInt32(length * 0.1 - 1);
            for (int i = 0; i < n; i++)
            {
                int randinex = rnd.Next(0, length);
                array[randinex] = rnd.Next(min, max);
            }
            return array;
        }

        static int[] GenerateMassive(Rule rule, int length, int min = 0, int max = 0)
        {
            if (min == max)
                max = length;
            int[] massive = rule(length, min, max);
            return massive;
        }
    }
}
