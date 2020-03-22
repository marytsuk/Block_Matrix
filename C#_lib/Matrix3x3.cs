using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csh_lib
{
    public class Matrix3x3
    {
        public const int MATR_SIZE = 2;
        public const int size3x3 = 9;
        public const int order = 3;
        public static int Current = 0;

        public MyList elements = new MyList(size3x3);

        public Matrix3x3()
        {
            Random rnd = new Random();
            int value;
            for (int i = 0; i < size3x3; i++)
            {
                value = rnd.Next(0, 30);
                elements[i] = value;//(++Current + i);
            }
            //elements[8] = 17;
        }
        public Matrix3x3(Matrix3x3 mat)
        {
            elements = mat.elements;
        }
	    public Matrix3x3(MyList v)
        {
            elements = v;
        }

        public double get(int h, int w)
        {
            return elements[order * h + w];
        }
        public void set(int h, int w, double value)
        {
            elements[h * order + w] = value;
        }

        public static Matrix3x3 operator+(Matrix3x3 mat1, Matrix3x3 mat2)
        {
            MyList res = new MyList(size3x3);
            for (int i = 0; i < size3x3; i++)
            {
                res[i] = (mat1.elements[i] + mat2.elements[i]);
            }
            return new Matrix3x3(res);
        }
           
	    public static Matrix3x3 operator-(Matrix3x3 mat1, Matrix3x3 mat2)
        {
            MyList res = new MyList(size3x3);
            for (int i = 0; i < size3x3; i++)
            {
                res[i] = (mat1.elements[i] - mat2.elements[i]);
            }
            return new Matrix3x3(res);
        }
	    public static Matrix3x3 operator*(Matrix3x3 mat1, Matrix3x3 mat2)
        {
            MyList res = new MyList(size3x3);
            for (int i = 0; i < order; i++)
            {
                for (int j = 0; j < order; j++)
                {
                    res[order * i + j] = 0;
                    for (int k = 0; k < order; k++)
                    {
                        res[order * i + j] += mat1.elements[order * i + k] * mat2.elements[order * k + j];
                    }
                }
            }
            return new Matrix3x3(res);
        }


	    public static MyList operator *(Matrix3x3 mat, MyList v)
        {
            MyList res = new MyList(order);

            for (int i = 0; i < order; i++)
            {
                res[i] = 0;
                for (int j = 0; j < order; j++)
                {
                    res[i] += v[j] * mat.elements[order * i + j];
                }
            }
            return res;
        }

	    public Matrix3x3 inverse()
        {
            MyList res = new MyList(size3x3);
            for (int i = 0; i < size3x3; i++)
            {
                res[i] = elements[i];
            }
            
            MyList E = new MyList(size3x3);

            double temp;

            for (int i = 0; i < order; i++)
            {
                for (int j = 0; j < order; j++)
                {
                    E[i * order + j] = 0.0;
                    if (i == j)
                        E[i * order + j] = 1.0;
                }
            }

            for (int k = 0; k < order; k++)
            {
                temp = res[order * k + k];

                for (int j = 0; j < order; j++)
                {
                    res[k * order + j] /= temp;
                    E[k * order + j] /= temp;
                }
                for (int i = k + 1; i < order; i++)
                {
                    temp = res[i * order + k];

                    for (int j = 0; j < order; j++)
                    {
                        res[i * order + j] -= res[k * order + j] * temp;
                        E[i * order + j] -= E[k * order + j] * temp;
                    }
                }
            }
            for (int k = order - 1; k > 0; k--)
            {
                for (int i = k - 1; i >= 0; i--)
                {
                    temp = res[i * order + k];

                    for (int j = 0; j < order; j++)
                    {
                        res[i * order + j] -= res[k * order + j] * temp;
                        E[i * order + j] -= E[k * order + j] * temp;
                    }
                }
            }
            for (int i = 0; i < order; i++)
                for (int j = 0; j < order; j++)
                    res[i * order + j] = E[i * order + j];
            return new Matrix3x3(res);
        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < order; i++)
            {
                for (int j = 0; j < order; j++)
                {
                    str += "   " + elements[i * order + j].ToString().PadLeft(4) + ' ';
                }
                str += '\n';
            }
            return str;
        }
    }
}
