// USB2I2CDlg.h : header file
//

#if !defined(AFX_USB2I2CDLG_H__792438EF_8C82_4EA0_905B_30A7A46AE668__INCLUDED_)
#define AFX_USB2I2CDLG_H__792438EF_8C82_4EA0_905B_30A7A46AE668__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CUSB2I2CDlg dialog
class CUSB2I2CDlg : public CDialog
{
// Construction
public:

	ULONG mIndex;
	BOOL m_open;
	CUSB2I2CDlg(CWnd* pParent = NULL);	// standard constructor
	PUCHAR mStrtoVal(PUCHAR str,ULONG strlen);
	UCHAR mCharToBcd(UCHAR iChar);
	ULONG CUSB2I2CDlg::mStrToBcd(CString str);
	BOOL mClose();
    void enablebtn(BOOL bEnable);

// Dialog Data
	//{{AFX_DATA(CUSB2I2CDlg)
	enum { IDD = IDD_USB2I2C_DIALOG };
	CButton	m_ok;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CUSB2I2CDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CUSB2I2CDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnDestroy();
	afx_msg void OnKeyUp(UINT nChar, UINT nRepCnt, UINT nFlags);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

//CUSB2I2CDlg * p_Dlg;
//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_USB2I2CDLG_H__792438EF_8C82_4EA0_905B_30A7A46AE668__INCLUDED_)
