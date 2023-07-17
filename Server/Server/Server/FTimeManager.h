#pragma once

#include "FSingleton.h"

class FTimeManager : public FSingleton<FTimeManager>
{
private:
	struct ScheduleData
	{
		float executeSec = 0.f;
		float elapsedSec = 0.f;
		int count = 0;
		bool loop = false;
		function<void()> func = nullptr;
	};

	vector<ScheduleData> m_ScheduleList;

public:
	void Tick(float InDeltaTime);

	void AddSchedule(float InSec, const function<void()>& InFunc);
	void AddScheduleLoop(float InSec, const function<void()>& InFunc);
	void AddScheduleCount(float InSec, int InCount, const function<void()>& InFunc);

private:
	void ExecuteSchedule(float InDeltaTime);
};

