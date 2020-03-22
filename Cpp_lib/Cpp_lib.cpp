// Cpp_lib.cpp : Определяет экспортированные функции для приложения DLL.
//

#include "stdafx.h"
#include <iostream>
#include <vector>
#include <fstream>
#include <string>
#include <iomanip>
#include <time.h>
#include <Cpp_lib.h>
#define MATR_SIZE 2

#define size3x3 9
#define order 3
using namespace std;

struct Matrix3x3
{
	vector<double> elements; //length 9

	Matrix3x3();
	Matrix3x3(const Matrix3x3& mat);
	Matrix3x3(const vector<double>& v);
	Matrix3x3(const double* elem);

	double get(int h, int w) const;
	void set(int h, int w, double value);

	Matrix3x3 operator+(const Matrix3x3& mat);
	Matrix3x3 operator-(const Matrix3x3& mat);
	Matrix3x3 operator*(const Matrix3x3& mat);


	vector<double> operator*(vector<double>& v);
	Matrix3x3 operator=(const Matrix3x3& mat);

	static Matrix3x3 inverse_matrix(const Matrix3x3& mat);
	Matrix3x3 inverse();

};
struct Block3DMatrix
{
	vector<Matrix3x3> A;
	vector<Matrix3x3> B;
	vector<Matrix3x3> C;

	vector<vector<double>> F;
	vector<vector<double>> solution;

	int size;

	Block3DMatrix(int x = MATR_SIZE);

	Block3DMatrix(int x, const vector<Matrix3x3>& a,
				  const vector<Matrix3x3>& b,
				  const vector<Matrix3x3>& c,
				  vector<vector<double>>& f);

	Block3DMatrix(double* mat, double* f, int mat_size);

	void solve();
	double get(int h, int w) const;
	void set(int h, int w, double value);

};
Matrix3x3::Matrix3x3() : elements(size3x3)
{
	for (int i = 0; i < size3x3; i++)
	{
		elements[i] = rand() % 30 + 1;
	}
	//elements[8] = 17;
}

Matrix3x3::Matrix3x3(const Matrix3x3 & mat)
{
	elements = mat.elements;
}

Matrix3x3::Matrix3x3(const vector<double>& v)
{
	elements = v;
}

Matrix3x3::Matrix3x3(const double * elem)
{
	for (int i = 0; i < size3x3; i++)
	{
		elements[i] = elem[i];
	}
}

double Matrix3x3::get(int h, int w) const
{

	return elements[order * h + w];
}

void Matrix3x3::set(int h, int w, double value)
{
	elements[h * order + w] = value;
}

Matrix3x3 Matrix3x3::operator+(const Matrix3x3 & mat)
{
	vector<double> res(size3x3);
	for (int i = 0; i < size3x3; i++)
	{
		res[i] = elements[i] + mat.elements[i];
	}
	return Matrix3x3(res);
}

Matrix3x3 Matrix3x3::operator-(const Matrix3x3 & mat)
{
	vector<double> res(size3x3);
	for (int i = 0; i < size3x3; i++)
	{
		res[i] = elements[i] - mat.elements[i];
	}
	return Matrix3x3(res);
}

Matrix3x3 Matrix3x3::operator*(const Matrix3x3 & mat)
{
	vector<double> res(size3x3);

	for (int i = 0; i < order; i++)
	{
		for (int j = 0; j < order; j++)
		{
			res[order * i + j] = 0;
			for (int k = 0; k < order; k++)
			{
				res[order * i + j] += elements[order * i + k] * mat.elements[order * k + j];
			}
		}
	}
	return Matrix3x3(res);
}

vector<double> Matrix3x3::operator*(vector<double>& v)
{
	vector<double> res(order);

	for (int i = 0; i < order; i++)
	{
		res[i] = 0;
		for (int j = 0; j < order; j++)
		{
			res[i] += v[j] * elements[order * i + j];
		}
	}
	return res;
}
Matrix3x3 Matrix3x3::operator=(const Matrix3x3 & mat)
{
	elements = mat.elements;
	return *this;
}
Matrix3x3 Matrix3x3::inverse_matrix(const Matrix3x3& mat)
{
	vector<double> res = mat.elements;
	vector<double> E(size3x3);

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
	return Matrix3x3(res);
}


Matrix3x3 Matrix3x3::inverse()
{
	vector<double> res(size3x3);
	res = elements;
	vector<double> E(size3x3);

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
	return Matrix3x3(res);
}
fstream& operator<<(fstream& out, Matrix3x3& mat)
{
	for (int i = 0; i < order; i++)
	{
		for (int j = 0; j < order; j++)
		{
			out << setw(10) << mat.elements[i * order + j];
		}
		out << endl;
	}
	return out;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


Block3DMatrix::Block3DMatrix(int x) : A(x - 1), B(x - 1), C(x), F(x, vector<double>(order, (rand() % 20 + 1))), solution(x, vector<double>(order))
{
	size = x;
}

Block3DMatrix::Block3DMatrix(int x, const vector<Matrix3x3>& a, const vector<Matrix3x3>& b, const vector<Matrix3x3>& c, vector<vector<double>>& f)
{
	size = x;
	A = a;
	B = b;
	C = c;
	F = f;
}

Block3DMatrix::Block3DMatrix(double * mat, double * f, int mat_size) :
	A(mat_size - 1), B(mat_size - 1), C(mat_size), F(mat_size, vector<double>(order)), solution(mat_size, vector<double>(order))
{
	size = mat_size;

	int N = mat_size * order;

	for (int i = 0; i < N; ++i)
	{
		for (int j = 0; j < N; ++j)
		{
			set(i, j, mat[i * N + j]);
		}
	}
	
	for (int i = 0; i < mat_size; ++i)
	{
		for (int j = 0; j < order; ++j)
		{
			F[i][j] = f[i * order + j];
		}
	}

}
vector<double> operator-(const vector<double>& X1, const vector<double>& X2)
{
	vector<double> res(X1.size());
	for (unsigned int i = 0; i < X1.size(); ++i)
	{
		res[i] = X1[i] - X2[i];
	}
	return res;
}

void Block3DMatrix::solve()
{
	vector<Matrix3x3>	Alpha(size - 1);
	vector<vector<double>>Beta(size);
	vector<double> tmp;
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

	for (int i = size - 2; i > -1; i--)
	{
		solution[i] = Beta[i] - Alpha[i] * solution[i + 1];
	}
}

double Block3DMatrix::get(int h, int w) const
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


void Block3DMatrix::set(int h, int w, double value)
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

void export_function(double* matrix,						 
					 double* heterogenity_vector,		
					 int matrix_size,										
					 double* solution,					
					 double* work_time)					
{
	time_t start, end;
	start = clock();

	Block3DMatrix My_matr(matrix, heterogenity_vector, matrix_size);

	My_matr.solve();

	for (int i = 0; i < matrix_size; ++i)
		for (int j = 0; j < order; ++j)
		{
			solution[i * order + j] = My_matr.solution[i][j];
		}


	end = clock();
	*work_time = end - start;
}

fstream& operator<<(fstream& file, Block3DMatrix& BM)
{
	int N = BM.size * order;

	for (int i = 0; i < N; ++i)
	{
		for (int j = 0; j < N; ++j)
		{
			file << setw(5) << BM.get(i, j) << " ";
		}
		file << "  |  " << setw(5) << BM.F[i / order][i - (i / order)*order] << endl;
	}

	file << endl << endl << endl;

	for (unsigned int i = 0; i < BM.solution.size(); ++i)
	{
		for (unsigned int j = 0; j < BM.solution[i].size(); ++j)
		{
			file << BM.solution[i][j] << " ";
		}
		file << endl;
	}

	return file;
}
