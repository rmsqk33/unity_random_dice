#include "stdafx.h"
#include "FTimeManager.h"
#include "FObjectManager.h"

void FTimeManager::Tick(float InDeltaTime)
{
	ExecuteSchedule(InDeltaTime);
}

void FTimeManager::AddSchedule(float InSec, const function<void()>& InFunc)
{
	ScheduleData scheduleData;
	scheduleData.executeSec = InSec;
	scheduleData.func = InFunc;

	m_ScheduleList.push_back(scheduleData);
}

void FTimeManager::AddScheduleLoop(float InSec, const function<void()>& InFunc)
{
	ScheduleData scheduleData;
	scheduleData.executeSec = InSec;
	scheduleData.loop = true;
	scheduleData.func = InFunc;

	m_ScheduleList.push_back(scheduleData);
}

void FTimeManager::AddScheduleCount(float InSec, int InCount, const function<void()>& InFunc)
{
	ScheduleData scheduleData;
	scheduleData.executeSec = InSec;
	scheduleData.count = InCount;
	scheduleData.func = InFunc;

	m_ScheduleList.push_back(scheduleData);
}

void FTimeManager::ExecuteSchedule(float InDeltaTime)
{
	vector<int> removeScheduleIndexList;
	for(int i = 0; i < m_ScheduleList.size(); ++i)
	{
		ScheduleData& scheduleData = m_ScheduleList[i];

		scheduleData.elapsedSec += InDeltaTime;
		if (scheduleData.elapsedSec < scheduleData.executeSec)
			continue;
		
		scheduleData.func();
		scheduleData.elapsedSec = 0.f;
		if (scheduleData.loop)
			continue;

		--scheduleData.count;
		if (0 < scheduleData.count)
			continue;

		removeScheduleIndexList.push_back(i);
	}

	for(int i = removeScheduleIndexList.size() - 1; 0 <= i; --i)
	{
		m_ScheduleList.erase(m_ScheduleList.begin() + removeScheduleIndexList[i]);
	}
}
