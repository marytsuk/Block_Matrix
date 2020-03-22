#pragma once


extern "C" __declspec(dllimport) void export_function(double* matrix,						
														double* heterogenity_vector,		
														int matrix_size,											
														double* solution,					
														double* work_time);	
