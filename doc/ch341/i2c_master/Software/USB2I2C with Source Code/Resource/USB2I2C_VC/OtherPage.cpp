// OtherPage.cpp : implementation file
//

#include "stdafx.h"
#include "Total.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// COtherPage property page

IMPLEMENT_DYNCREATE(COtherPage, CPropertyPage)

COtherPage::COtherPage() : CPropertyPage(COtherPage::IDD)
{
	//{{AFX_DATA_INIT(COtherPage)
	m_data = _T("");
	m_dataaddr = _T("");
	m_devaddr = _T("50");
	//}}AFX_DATA_INIT
}

COtherPage::~COtherPage()
{
}

void COtherPage::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(COtherPage)
	DDX_Text(pDX, IDC_EDIT_I2CDATA, m_data);
	DDV_MaxChars(pDX, m_data, 2);
	DDX_Text(pDX, IDC_EDIT_I2CDATAADD, m_dataaddr);
	DDV_MaxChars(pDX, m_dataaddr, 2);
	DDX_Text(pDX, IDC_EDIT_I2CDEVADD, m_devaddr);
	DDV_MaxChars(pDX, m_devaddr, 2);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(COtherPage, CPropertyPage)
	//{{AFX_MSG_MAP(COtherPage)
	ON_BN_CLICKED(IDC_BUTTON_I2CREAD, OnButtonI2cread)
	ON_BN_CLICKED(IDC_BUTTON_I2CWRITE, OnButtonI2cwrite)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// COtherPage message handlers

void COtherPage::OnButtonI2cread()		//��I2C
{
	UCHAR mData=0;
	UCHAR mDataAddr=0;
	UCHAR mDevAddr=0;
	UpdateData(TRUE);	
	if(strlen(m_dataaddr) == 0 )
	{
		MessageBox("���������ݵ�Ԫ��ַ","USB2I2C",MB_OK);
		return;
	}
	else if(strlen(m_devaddr) == 0)
	{
		MessageBox("�������豸��ַ","USB2I2C",MB_OK);
		return;
	}
	if(strlen(m_dataaddr) > 2 || strlen(m_devaddr) > 2)
	{
		MessageBox("�豸��ַ�����ݵ�Ԫ��ַ��Ӧ������ʮ������FFH","USB2I2C",MB_OK);
		return;
	}
    
	if(strlen(m_dataaddr) > 1)
	{
		mDataAddr = (p_Dlg->mCharToBcd(m_dataaddr.GetAt(0)) << 4) + (p_Dlg->mCharToBcd(m_dataaddr.GetAt(1)));
	}
	else 
	{
		mDataAddr = p_Dlg->mCharToBcd(m_dataaddr.GetAt(0));
	}
	if(strlen(m_devaddr) > 1)
	{
		mDevAddr = (p_Dlg->mCharToBcd(m_devaddr.GetAt(0)) << 4) + (p_Dlg->mCharToBcd(m_devaddr.GetAt(1)));
	}
	else
	{
		mDevAddr = p_Dlg->mCharToBcd(m_devaddr.GetAt(0));
	}
	if(p_Dlg->m_open)
	{
		if( !USBIO_ReadI2C( p_Dlg->mIndex, mDevAddr, mDataAddr, &mData ) )
		{
			MessageBox("I2C������ʧ�ܣ�","USB2I2C",MB_OK|MB_ICONSTOP);
		}
		else
		{
			CHAR mtemp[4]="";		
			sprintf(&mtemp[0],"%2X",mData);    //��λʮ����������һ���ո�
			m_data=mtemp;		
			UpdateData(false);		
		}
	}
	else
	{
		MessageBox("�豸δ�򿪣�","USB2I2C",MB_OK|MB_ICONSTOP);
	}
	UpdateData(false);
}

void COtherPage::OnButtonI2cwrite()			//дI2C
{
	UCHAR mData;
	UCHAR mDataAddr;
	UCHAR mDevAddr;
	UpdateData(TRUE);
	
	if(strlen(m_dataaddr) == 0 || strlen(m_devaddr) == 0 || strlen(m_data) == 0)
	{
		MessageBox("�������豸��ַ�����ݵ�Ԫ��ַ������","USB2I2C",MB_OK);
		return;
	}
	if(strlen(m_dataaddr) > 2 || strlen(m_devaddr) > 2 || strlen(m_data) > 2)
	{
		MessageBox("�豸��ַ�����ݵ�Ԫ��ַ�ʹ�д�����ݶ�Ӧ������ʮ������FFH","USB2I2C",MB_OK);
		return;
	}
	
	if(strlen(m_dataaddr) > 1)
	{
		mDataAddr = (p_Dlg->mCharToBcd(m_dataaddr.GetAt(0)) << 4) + (p_Dlg->mCharToBcd(m_dataaddr.GetAt(1)));
	}
	else
	{
		mDataAddr = p_Dlg->mCharToBcd(m_dataaddr.GetAt(0));
	}
	if(strlen(m_devaddr) > 1)
	{
		mDevAddr = (p_Dlg->mCharToBcd(m_devaddr.GetAt(0)) << 4) + (p_Dlg->mCharToBcd(m_devaddr.GetAt(1)));
	}
	else
	{
		mDevAddr = p_Dlg->mCharToBcd(m_devaddr.GetAt(0));
	}
	if(strlen(m_data) > 1)
	{
		mData = (p_Dlg->mCharToBcd(m_data.GetAt(0)) << 4) + (p_Dlg->mCharToBcd(m_data.GetAt(1)));
	}
	else
	{
		mData = p_Dlg->mCharToBcd(m_data.GetAt(0));
	}
	if(p_Dlg->m_open)
	{
		if(!USBIO_WriteI2C( p_Dlg->mIndex, mDevAddr, mDataAddr, mData))
		{
			MessageBox("I2Cд����ʧ�ܣ�","USB2I2C",MB_OK|MB_ICONSTOP);
		}
		else
		{
			MessageBox("I2Cд���ݳɹ���","USB2I2C",MB_OK);			
		} 
	}
	else
	{
		MessageBox("�豸δ�򿪣�","USB2I2C",MB_OK|MB_ICONSTOP);
	}
}


BOOL COtherPage::OnInitDialog() 
{
	CPropertyPage::OnInitDialog();

	p_Dlg->enablebtn(p_Dlg->m_open );    //��ʼ����ť

	return TRUE;  
}
