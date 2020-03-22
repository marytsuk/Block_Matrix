using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csh_lib;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace My_main
{
    class Program
    {
        public static double work_time_cpp = 0;
        public static double work_time_csharp = 0;
        public static int m_size = 2;
        static void Test()
        {
            Console.WriteLine("Метод Test() :");
            double[] solution = new double[m_size * 3];
            double[] matr_data = new double[m_size * m_size * 9];

            for (int i = 0; i < m_size * m_size * 9; i++)
            {
                matr_data[i] = i + 1;
            }

            double[] F_data = new double[m_size * 3];

            for (int i = 0; i < m_size * 3; i++)
            {
                F_data[i] = 1;
            }

            Console.WriteLine("  Правая часть : ");
            for (int i = 0; i < m_size; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.WriteLine("    " + F_data[i * 3 + j]);
                }
                //Console.WriteLine();
            }
            matr_data[7] = 7;
            matr_data[14] = 7;
            matr_data[21] = 7;
            matr_data[m_size * m_size * 9 - 1] = 16;
            matr_data[31] = 15;

            Block3DMatrix matr = new Block3DMatrix(matr_data, F_data, m_size);

            matr.solve();

            FileStream filestream = null;
            try
            {
                filestream = File.Create(@"C:\Users\Маша\Documents\Visual Studio 2017\Projects\Lab_5_Masha\mat.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Исключение : " + ex.Message);
                if (filestream != null)
                    filestream.Close();
            }

            byte[] input = Encoding.Default.GetBytes(matr.ToString());
            filestream.Write(input, 0, input.Length);

            List<MyList> F_computed = matr * matr.solution;
            Console.WriteLine("  Правая часть (результат перемножения) : ");
            for (int i = 0; i < matr.size; i++)
            {
                for (int j = 0; j < matr.order; j++)
                {
                    Console.WriteLine("    " + F_computed[i][j] + " ");
                }
                //Console.WriteLine();
            }
            export_function(matr_data, F_data, m_size, solution, ref work_time_cpp);
            Console.WriteLine("  Решение (с помощью С++) : ");
            for (int i = 0; i < m_size; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.WriteLine("    " + solution[i * 3 + j]);
                }
                Console.WriteLine();
            }

            filestream.Close();
            Console.WriteLine("Конец Test().");
        }

        static void Main(string[] args)
        {
            Test();
            bool flag = false;
            char symbol;
            
            
            while (!flag)
            {
                Console.WriteLine("Завершить работу? (y/n)");
                Console.Write("  ");
                symbol = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (symbol == 'y')
                {
                    return;
                }
                if (symbol == 'n')
                {
                    flag = true;
                }
                if (symbol != 'n')
                {
                        Console.WriteLine("Недопустимый символ !");
                }
            }
           

            TestTime obj = new TestTime();
            TestTime.Load(@"C:\Users\Маша\Documents\Visual Studio 2017\Projects\Lab_5_Masha\TestTime.txt", ref obj);

            flag = true;

            while (flag)
            {
                Console.WriteLine("Введите порядок матрицы :");
                Console.Write("  ");
                try
                {
                    m_size = Int32.Parse(Console.ReadLine());
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Исключение : " + ex.Message);
                    continue;
                }

                if (m_size >= 2)
                {
                    double[] solution = new double[m_size * 3];

                    double[] matr_data = new double[m_size * m_size * 9];

                    for (int i = 0; i < m_size * m_size * 9; ++i)
                    {
                        matr_data[i] = i + 1;
                    }

                    double[] F_data = new double[m_size * 3];

                    for (int i = 0; i < m_size * 3; ++i)
                    {
                        F_data[i] = i + 1;
                    }

                    Stopwatch swatch = new Stopwatch();
                    swatch.Start();

                    Block3DMatrix matr = new Block3DMatrix(matr_data, F_data, m_size);

                    matr.solve();

                    swatch.Stop();

                    export_function(matr_data, F_data, m_size, solution, ref work_time_cpp);

                    obj.Add(m_size.ToString(),
                           (swatch.Elapsed.Milliseconds).ToString(),
                           work_time_cpp.ToString(),
                           ((double)swatch.Elapsed.Milliseconds / (double)work_time_cpp).ToString());
                }


                else
                {
                    Console.WriteLine("Недопустимый размер матрицы !");
                    continue;
                }

                Console.WriteLine("Продолжить? (y/n)");
                Console.Write("  ");
                symbol = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (symbol == 'n')
                {
                    flag = false;
                }
                else
                {
                    if (symbol != 'y')
                    {
                        Console.WriteLine("Недопустимый символ !");
                        flag = false;
                    }
                }
            }
            TestTime.Save(@"C:\Users\Маша\Documents\Visual Studio 2017\Projects\Lab_5_Masha\TestTime.txt", ref obj);
            Console.WriteLine(obj);
        }

        [DllImport(@"C:\Users\Маша\Documents\Visual Studio 2017\Projects\Lab_5_Masha\Debug\Cpp_lib.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void export_function(double[] matrix,                       
                                                  double[] heterogenity_vector,        
                                                  int matrix_size,                                         
                                                  double[] solution,                    
                                                  ref double work_time);
    }
}
