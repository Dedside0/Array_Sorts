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
            Rule[] rules = { IncreaseArray, DecreasingArray, HalfArray, RandomArray, NinetyPercentArray };
            Sort[] sorts = { ShakeSort };

            Console.WriteLine("Введите длинну массива");
            int len = int.Parse(Console.ReadLine());
            Console.WriteLine("Выводить на консоль массивы?  0 - нет, 1 - да");
            int ans = int.Parse(Console.ReadLine());

            Stopwatch timer = Stopwatch.StartNew();
            int[] arr;

            foreach (var rule in rules)
            {
                timer.Start();
                arr = GenerateMassive(rule, len);
                timer.Stop();
                Console.WriteLine("Тип: " + rule.Method.Name + " - создавался " + timer.ElapsedMilliseconds);
                timer.Reset();
                if (ans == 1)
                    Console.WriteLine(String.Join(" ", arr));
                foreach (var sort in sorts)
                {
                    timer.Start();
                    sort(arr);
                    timer.Stop();
                    Console.WriteLine(sort.Method.Name + " - " + timer.ElapsedMilliseconds);
                    timer.Restart();
                    if (ans == 1)
                        Console.WriteLine(String.Join(" ", arr));
                }
                Console.WriteLine();
            }
            //  Console.WriteLine(string.Join(",", array));
           
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
        static int[] RandomArray(int length, int min = 0, int max = 0)
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
        static int[] IncreaseArray(int length, int min, int max)
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
        static int[] DecreasingArray(int length, int min, int max)
        {
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = length - i;
            }
            return array;
        }

        //первая половина убывает, вторая возрастает
        static int[] HalfArray(int length, int min = 0, int max = 0)
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
        static int[] NinetyPercentArray(int length, int min = 0, int max = 0)
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
            int[] massive = rule(length, min, max);
            return massive;
        }
    }
}
