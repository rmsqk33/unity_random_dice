#include "stdafx.h"
#include "FControllerBase.h"

FControllerBase::FControllerBase(FPlayer* InOwner)
{
	m_Owner = InOwner;
}

FPlayer* FControllerBase::GetOwner() const
{
	return m_Owner;
}

int FControllerBase::GetDBID() const
{
	return m_Owner->GetDBID();
}

SOCKET FControllerBase::GetSocket() const
{
	return GetOwner()->GetClientSocket();
}

