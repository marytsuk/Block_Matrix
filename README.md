# Block_Matrix
The solution of SLAE with a three-diagonal block matrix.

The C# and C++ languages define classes for solving systems
linear algebraic equations with a block tridiagonal matrix.
Classes are used to compare the runtime of managed C# code and
unmanaged C++code. The C++ code is compiled into a DLL. C# code calls methods from
C++ DLL libraries using the PInvoke service. To save comparison results in a file
serialization is used for C# and C++ code execution times. 

The type of block matrix elements is a third-order matrix. 

Solved by the matrix run method.
