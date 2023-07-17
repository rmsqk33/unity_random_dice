#pragma once

class FControllerBase;

class FPlayer
{
private:
	SOCKET m_ClientSocket;
	int m_DBID;
	map<ControllerType, FControllerBase*> m_Controllers;

public:
	FPlayer(SOCKET InClientSocket, int InID);

	void Initialize();
	void Release();
	void Tick(float InDeltaTime);

	SOCKET GetClientSocket() const;
	int GetDBID() const;

	template<typename T>
	void AddController()
	{
		m_Controllers.insert(pair<ControllerType, FControllerBase*>(T::GetType(), new T(this)));
	}

	template<typename T>
	T* FindController() const
	{
		auto iter = m_Controllers.find(T::GetType());
		if (iter != m_Controllers.end())
			return static_cast<T*>(iter->second);

		return nullptr;
	}

private:
	void AddController(FControllerBase* InController);
	void InitController();
};