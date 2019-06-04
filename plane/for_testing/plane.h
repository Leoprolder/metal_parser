/*
 * MATLAB Compiler: 6.2 (R2016a)
 * Date: Wed Jun 05 01:00:02 2019
 * Arguments: "-B" "macro_default" "-W" "lib:plane" "-T" "link:lib" "-d"
 * "C:\repositories\metal_parser\plane\for_testing" "-v"
 * "C:\repositories\metal_parser\plane.m" 
 */

#ifndef __plane_h
#define __plane_h 1

#if defined(__cplusplus) && !defined(mclmcrrt_h) && defined(__linux__)
#  pragma implementation "mclmcrrt.h"
#endif
#include "mclmcrrt.h"
#ifdef __cplusplus
extern "C" {
#endif

#if defined(__SUNPRO_CC)
/* Solaris shared libraries use __global, rather than mapfiles
 * to define the API exported from a shared library. __global is
 * only necessary when building the library -- files including
 * this header file to use the library do not need the __global
 * declaration; hence the EXPORTING_<library> logic.
 */

#ifdef EXPORTING_plane
#define PUBLIC_plane_C_API __global
#else
#define PUBLIC_plane_C_API /* No import statement needed. */
#endif

#define LIB_plane_C_API PUBLIC_plane_C_API

#elif defined(_HPUX_SOURCE)

#ifdef EXPORTING_plane
#define PUBLIC_plane_C_API __declspec(dllexport)
#else
#define PUBLIC_plane_C_API __declspec(dllimport)
#endif

#define LIB_plane_C_API PUBLIC_plane_C_API


#else

#define LIB_plane_C_API

#endif

/* This symbol is defined in shared libraries. Define it here
 * (to nothing) in case this isn't a shared library. 
 */
#ifndef LIB_plane_C_API 
#define LIB_plane_C_API /* No special import/export declaration */
#endif

extern LIB_plane_C_API 
bool MW_CALL_CONV planeInitializeWithHandlers(
       mclOutputHandlerFcn error_handler, 
       mclOutputHandlerFcn print_handler);

extern LIB_plane_C_API 
bool MW_CALL_CONV planeInitialize(void);

extern LIB_plane_C_API 
void MW_CALL_CONV planeTerminate(void);



extern LIB_plane_C_API 
void MW_CALL_CONV planePrintStackTrace(void);

extern LIB_plane_C_API 
bool MW_CALL_CONV mlxPlane(int nlhs, mxArray *plhs[], int nrhs, mxArray *prhs[]);



extern LIB_plane_C_API bool MW_CALL_CONV mlfPlane(int nargout, mxArray** res, mxArray* strfunc, mxArray* vx0, mxArray* vx1, mxArray* vy0, mxArray* vy1, mxArray* h);

#ifdef __cplusplus
}
#endif
#endif
