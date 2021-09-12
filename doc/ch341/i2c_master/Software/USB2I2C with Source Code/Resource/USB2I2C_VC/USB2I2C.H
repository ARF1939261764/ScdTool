// USB2I2C.h : main header file for the USB2I2C application
//

#if !defined(AFX_USB2I2C_H__BA8EDDAD_99F0_4548_AAF9_5CFF36B45429__INCLUDED_)
#define AFX_USB2I2C_H__BA8EDDAD_99F0_4548_AAF9_5CFF36B45429__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CUSB2I2CApp:
// See USB2I2C.cpp for the implementation of this class
//

class CUSB2I2CApp : public CWinApp
{
public:
	CUSB2I2CApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CUSB2I2CApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CUSB2I2CApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_USB2I2C_H__BA8EDDAD_99F0_4548_AAF9_5CFF36B45429__INCLUDED_)
