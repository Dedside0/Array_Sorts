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
            
            Rule[] types = { Increasing, Decreasing, Half, Random, NinetyPercent };
            Sort[] sorts = { QuickSort, ShellSort, InputSort,  SelectionSort, ShakeSort, BubbleSort };
            var dict = new Dictionary<Sort, List<double>>();

            Console.WriteLine("Введите длинну массива");
            int len = int.Parse(Console.ReadLine());
            int min = 0;
            int max = len * 2;
            int[] arr;
            int[] arayForTests = new int[len];
            Stopwatch timer = Stopwatch.StartNew();

            #region Table Creating
            string[,] excel = new string[types.Length + 2, sorts.Length + 2];
            excel[0, 0] = string.Format("Тип");
            excel[0, 1] = string.Format("Создавался");
            excel[excel.GetLength(0) - 1, 0] = string.Format("Сред. время");

            for (int i = 2; i < excel.GetLength(1); i++)
            {
                excel[0, i] = string.Format(sorts[i - 2].Method.Name);
            }

            for (int i = 1; i < excel.GetLength(0) - 1; i++)
            {
                excel[i, 0] = string.Format(types[i - 1].Method.Name);
            }
            int row = 1, column = 1;
            double creatingAverageTime = 0;
            #endregion

            #region Заносим данные в таблицу и выводим
            PrintInfo(excel, len);
            foreach (var rule in types)
            {
                timer.Start();
                arr = GenerateMassive(rule, len, min, max);
                timer.Stop();
                creatingAverageTime += Convert.ToDouble(timer.ElapsedMilliseconds.ToString());

                excel[row, column] = string.Format("{0,14}", timer.ElapsedMilliseconds);
                column++;

                PrintInfo(excel, len);
                timer.Reset();

                foreach (var sort in sorts)
                {
                    for (int i = 0; i < len; i++)
                    {
                        arayForTests[i] = arr[i];
                    }
                    timer.Start();
                    sort(arayForTests);
                    timer.Stop();

                    if (!dict.ContainsKey(sort))
                        dict.Add(sort, new List<double> { Convert.ToDouble(timer.ElapsedMilliseconds.ToString()) });
                    else
                        dict[sort].Add(Convert.ToDouble(timer.ElapsedMilliseconds.ToString()));

                    excel[row, column] = string.Format("{0,14}", timer.ElapsedMilliseconds);
                    column++;

                    timer.Reset();

                    PrintInfo(excel, len);
                }
                column = 1;
                row++;
            }
            column = 2;
            row = excel.GetLength(0) - 1;
            foreach (var sort in sorts)
            {
                excel[row, column] = AverageTime(dict[sort]).ToString();
                column++;
            }
            excel[row, 1] = (creatingAverageTime / types.Length).ToString();
            PrintInfo(excel, len);
            #endregion

        }

        #region Методы для вывода таблица
        //посчитать среднее время
        static double AverageTime(List<double> arr)
        {
            double sum = 0;
            foreach (var item in arr)
            {
                sum += item;
            }
            if (sum == 0)
                return 0;
            return sum / arr.Count;
        }
        //очистить консоль и вывести таблицу
        static void PrintInfo(string[,] matrica, int len)
        {
            Console.Clear();
            Console.WriteLine("Все вычисления в миллисекундах.  Длинна массива: " + len);
            PrintTable(matrica);
        }
        //Вывести таблицу
        static void PrintTable(string[,] matrica)
        {
            for (int i = 0; i < matrica.GetLength(0); i++)
            {
                for (int j = 0; j < matrica.GetLength(1); j++)
                {
                    if (matrica[i, j] == null)
                        Console.Write("|{0,14}", "...");
                    else
                        Console.Write("|{0,-14}", matrica[i, j]);
                }
                Console.WriteLine();
            }
        }
        #endregion


        /// <summary>
        /// Генерирует массив указанного типо
        /// </summary>
        /// <param name="rule">Тип массива (убывающий, возрастаюший, и т.д.</param>
        /// <param name="length">длина массива</param>
        /// <param name="min">минимальный элемент</param>
        /// <param name="max">максимальный элемента</param>
        /// <returns></returns>
        static int[] GenerateMassive(Rule rule, int length, int min, int max)
        {
            int[] massive;

            if (min > max)
                massive = rule(length, max, min);
            else
                massive = rule(length, min, max);

            return massive;
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

        //Быстрая сортировка
        static void QuickSort(int[] arr)
        {
            QuickSortInPlace(arr, 0, arr.Length - 1);
        }
        static void QuickSortInPlace(int[] arr, int left, int right)
        {
            if (left >= right) return;

            int pivot = arr[(left + right) / 2]; // Опорный элемент (центр)
            int i = left, j = right;

            while (i <= j)
            {
                while (arr[i] < pivot) i++;
                while (arr[j] > pivot) j--;

                if (i <= j)
                {
                    (arr[i], arr[j]) = (arr[j], arr[i]); // Обмен значений
                    i++;
                    j--;
                }
            }
            // Рекурсивно сортируем левую и правую части
            if (left < j) QuickSortInPlace(arr, left, j);
            if (i < right) QuickSortInPlace(arr, i, right);
        }

        //Шейкерная сортировка
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

        static void ShellSort(int[] arr)
        {
            int len = arr.Length;
            int step = len / 2; //шаг проверки элементов

            while (step > 0)
            {
                for (int i = step; i < len; i++)
                {
                    int temp = arr[i]; // Запоминаем элемент для вставки
                    int j = i;

                    // Сортировка вставками с шагом `step`
                    while (j >= step && arr[j - step] > temp)
                    {
                        arr[j] = arr[j - step]; // Сдвигаем элементы вправо
                        j -= step;
                    }

                    arr[j] = temp; // Вставляем элемент на нужное место
                }

                step /= 2; // Уменьшаем шаг
            }
        }

        //Сортировка Вставками
        static void InputSort(int[] arr)
        {
            int len = arr.Length;
            for (int i = 1; i < len; i++)
            {
                //Первое значение из неотсорированной части массива
                int value = arr[i];
                int j = i;
                //сдвигаем элемента на место value чтобы освободить место для value
                while (j > 0 && arr[j - 1] > value)
                {
                    arr[j] = arr[j - 1];
                    j--;
                }
                arr[j] = value;
            }
        }

        //Сортировка Выбором
        static void SelectionSort(int[] arr)
        {
            int minIndex;
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
                int temp = arr[i];
                arr[i] = arr[minIndex];
                arr[minIndex] = temp;
            }
        }


        //Рандомный массив
        static int[] Random(int length, int min, int max)
        {
            int[] array = new int[length];

            for (int i = 0; i < length; i++)
            {
                array[i] = rnd.Next(min, max + 1);
            }
            return array;
        }

        //По возрастанию
        static int[] Increasing(int length, int min, int max)
        {
            int[] arr = new int[length];

            arr[0] = rnd.Next(min, max + 1);
            for (int i = 1; i < length; i++)
            {
                // Генерируем следующее число больше предыдущего
                arr[i] = rnd.Next(arr[i - 1], arr[i - 1] + 100);
            }

            return arr;
        }

        //Убывающий массив
        static int[] Decreasing(int length, int min, int max)
        {
            int[] array = new int[length];
            int len = array.Length;
            array[0] = max;
            for (int i = 1; i < len; i++)
            {
                array[i] = rnd.Next(array[i-1] - 100, array[i-1]);
            }
            return array;
        }

        //первая половина убывает, вторая возрастает
        static int[] Half(int length, int min, int max)
        {
            int halfLength = length / 2;
            int extra = length % 2; // 1, если нечётное количество элементов
            int[] array = new int[length];
            int[] decreasingArr = Decreasing(halfLength + extra, min, max);
            int[] increasingArr = Increasing(halfLength + extra, min, max);
            for (int i = 0; i < halfLength + extra; i++)
            {
                array[i] = decreasingArr[i];
            }
            for (int i = halfLength + extra; i < length; i++)
            {
                array[i] = increasingArr[i - halfLength - extra];
            }
            return array;
        }

        //Массив где много одинаковых значений
        static int[] NinetyPercent(int length, int min, int max)
        {
            int[] array = new int[length];
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

    }
}
