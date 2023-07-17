#pragma once

template<typename T>
class FSingleton
{
private:
    static T* m_Instance;

protected:
    FSingleton() {}
    virtual ~FSingleton() {}
    virtual void Release() {}

public:
    static T* GetInstance()
    {
        if (m_Instance == nullptr)
        {
            m_Instance = new T();
            atexit(Destroy);
        }

        return m_Instance;
    }

private:
    static void Destroy()
    {
        if (m_Instance != nullptr)
        {
            m_Instance->Release();

            delete m_Instance;
        }
    }
};

template<typename T>
T* FSingleton<T>::m_Instance = nullptr;