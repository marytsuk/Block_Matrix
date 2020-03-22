using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csh_lib
{
    public class Block3DMatrix
    {
        public const int MATR_SIZE = 2;
        public const int size3x3 = 9;
        public int order = 3;

        public List<Matrix3x3> A;
        public List<Matrix3x3> B;
        public List<Matrix3x3> C;

        public List<MyList> F;
        public List<MyList> solution;

        public int size;

        public Block3DMatrix(int x = MATR_SIZE)
        {
            size = x;
            A = new List<Matrix3x3>(x - 1);
            B = new List<Matrix3x3>(x - 1);
            C = new List<Matrix3x3>(x);
            F = new List<MyList>(x);
            solution = new List<MyList>(x);
            for (int i = 0; i < x; i++)
            {
                F.Add(new MyList(order));
                solution.Add(new MyList(order));
            }
            for (int i = 0; i < x - 1; i++)
            {
                A.Add(new Matrix3x3());
                B.Add(new Matrix3x3());
                
            }
            for (int i = 0; i < x; i++)
            {
                C.Add(new Matrix3x3());
                for (int j = 0; j < order; j++)
                {
                    F[i][j] = 1;
                }
            }
        }
        public Block3DMatrix(int x, List<Matrix3x3> a, List<Matrix3x3> b,
		List<Matrix3x3> c, List<MyList> f)
        {
            size = x;
            A = a;
            B = b;
            C = c;
            F = f;
        }
        public void solve()
        {
            MyList v = new MyList(size3x3);
            List<Matrix3x3> Alpha = new List<Matrix3x3>(size - 1);
            for (int i = 0; i < size - 1; i++)
            {
                Alpha.Add(new Matrix3x3(v));
            }
            List<MyList> Beta = new List<MyList>(size);
            for (int i = 0; i < size; i++)
            {
                Beta.Add(new MyList(order));
            }
            MyList tmp = new MyList(order);
            Alpha[0] = C[0].inverse() * B[0];
            Beta[0] = C[0].inverse() * F[0];
            
            for (int i = 1; i < size - 1; i++)
            {
                Alpha[i] = (C[i] - A[i - 1] * Alpha[i - 1]).inverse() * B[i];
                tmp = F[i] - A[i - 1] * Beta[i - 1];
                Beta[i] = (C[i] - A[i - 1] * Alpha[i - 1]).inverse() * tmp;
            }
            tmp = F[size - 1] - A[size - 1 - 1] * Beta[size - 1 - 1];
            Beta[size - 1] = (C[size - 1] - A[size - 1 - 1] * Alpha[size - 1 - 1]).inverse() * tmp;

            solution[size - 1] = Beta[size - 1];

            for (int i = size - 2; i > -1; --i)
            {
                solution[i] = Beta[i] - Alpha[i] * solution[i + 1];
            }
        }
        public static List<MyList> operator *(Block3DMatrix M, List<MyList> V)
        {
            List<MyList> res = new List<MyList>(M.size);
            for (int i = 0; i < M.size; i++)
            {
                res.Add(new MyList(M.order));

                for (int j = 0; j < M.size; j++)
                {
                    res[i].Add(0.0);
                }
            }

            int point_size = M.size * M.order;


            for (int k = 0; k < point_size; k++)
            {
                int vec_coord = k / M.order;
                res[vec_coord][k - vec_coord * M.order] = 0;

                for (int j = 0; j < point_size; ++j)
                {
                    res[vec_coord][k - vec_coord * M.order] += M.get(k, j) * V[j / M.order][j - M.order * (j / M.order)];
                }

            }

            return res;
        }
        public double get(int h, int w)
        {
            int block_coord_h = h / order;
            int block_coord_w = w / order;

            int h_in_block = h - block_coord_h * order;
            int w_in_block = w - block_coord_w * order;

            if (block_coord_h == block_coord_w)
                return C[block_coord_h].get(h_in_block, w_in_block);

            if (block_coord_w == block_coord_h + 1) 
                return B[block_coord_h].get(h_in_block, w_in_block);

            if (block_coord_h == block_coord_w + 1) 
                return A[block_coord_w].get(h_in_block, w_in_block);

            return 0.0;
        }
        public void set(int h, int w, double value)
        {
            int block_coord_h = h / order;
            int block_coord_w = w / order;

            int h_in_block = h - block_coord_h * order;
            int w_in_block = w - block_coord_w * order;

            if (block_coord_h == block_coord_w)
                C[block_coord_h].set(h_in_block, w_in_block, value);

            if (block_coord_w == block_coord_h + 1) 
                B[block_coord_h].set(h_in_block, w_in_block, value);

            if (block_coord_h == block_coord_w + 1) 
                A[block_coord_w].set(h_in_block, w_in_block, value);
        }
        public override string ToString()
        {
            string str = "";
            int N = size * order;

            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < N; ++j)
                {
                    str += this.get(i, j).ToString().PadLeft(8) + " ";
                }
                str += " | " + this.F[i / order][i - (i / order) * order].ToString().PadLeft(4) + "\n";
            }

            str += "\n\n\n";

            for (int i = 0; i < this.solution.Count(); ++i)
            {
                for (int j = 0; j < solution[i].Count(); ++j)
                {
                    str += solution[i][j].ToString().PadRight(10) + "; ";
                }
                str += "\n";
            }

            return str;
        }

        public Block3DMatrix(double[] matrix_data, double[] F_data, int X)
        {
            A = new List<Matrix3x3>(X - 1);
            B = new List<Matrix3x3>(X - 1);
            C = new List<Matrix3x3>(X);
            F = new List<MyList>(X);
            solution = new List<MyList>(X);
            for (int i = 0; i < X; i++)
            {
                F.Add(new MyList(order));
                solution.Add(new MyList(order));
            }
            for (int i = 0; i < X - 1; i++)
            {
                A.Add(new Matrix3x3());
                B.Add(new Matrix3x3());

            }
            for (int i = 0; i < X; i++)
            {
                C.Add(new Matrix3x3());
                for (int j = 0; j < order; j++)
                {
                    F[i][j] = 1;
                }
            }
            int matrix_size = X;

            solution = new List<MyList>(X);
            for (int i = 0; i < X; ++i)
            {
                solution.Add(new MyList());
            }


            int matr_point_size = matrix_size * order;

       
            size = matrix_size;


            for (int i = 0; i < matr_point_size; ++i)
            {
                for (int j = 0; j < matr_point_size; ++j)
                {
                    set(i, j, matrix_data[i * matr_point_size + j]);
                }
            }


            for (int i = 0; i < matrix_size; ++i)
            {
                for (int j = 0; j < order; ++j)
                {
                    F[i][j] = F_data[i * order + j];
                }
            }

        }
    }
}
