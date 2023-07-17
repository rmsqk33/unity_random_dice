#include "pch.h"
#include "XmlReader.h"

XmlReader::~XmlReader()
{
    Close();
}

bool XmlReader::Open(const char* InPath)
{
    Close();

    locale::global(locale(".UTF-8"));

    string path(InPath);
    if (path.find_last_of(".xml") == string::npos)
        return false;

    m_ReadStream = new ifstream(path);
    return m_ReadStream->is_open();
}

void XmlReader::Close()
{
    if (m_ReadStream != nullptr)
    {
        m_ReadStream->close();

        delete m_ReadStream;
        m_ReadStream = nullptr;
    }
}

bool XmlReader::Read()
{
    if (m_ReadStream == nullptr)
        return false;

    if (!getline(*m_ReadStream, m_ReadLine))
        return false;

    m_Type = XmlReaderType::None;
    for (int i = 0; i < m_ReadLine.size(); ++i)
    {
        switch (m_ReadLine[i])
        {
        case '<':
            m_Type = m_ReadLine[i + 1] == '/' ? XmlReaderType::EndElement : XmlReaderType::Element;
            if (m_Type == XmlReaderType::Element)
            {
                for (int j = i + 1; j < m_ReadLine.size(); ++j)
                {
                    switch (m_ReadLine[j])
                    {
                    case '>':
                        SetName(m_ReadLine.substr(i + 1, j - i - 1));
                        m_ReadLine.clear();
                        return true;

                    case ' ':
                        SetName(m_ReadLine.substr(i + 1, j - i - 1));
                        m_ReadLine = m_ReadLine.c_str() + j + 1;
                        return true;
                    }
                }
            }
            return true;

        case '>':
            if (0 < i && m_ReadLine[i - 1] == '/')
                m_Type = XmlReaderType::EndElement;
            return true;
        }
    }

    return true;
}

bool XmlReader::MoveToNextAttribute()
{
    size_t valueStartIndex = 0;
    for (int i = 0; i < m_ReadLine.size(); ++i)
    {
        switch (m_ReadLine[i])
        {
        case '=':
            SetName(m_ReadLine.substr(0, i));
            break;

        case '\"':
            if (m_ReadLine[i - 1] == '=')
                valueStartIndex = i + 1;
            else
            {
                m_Value = m_ReadLine.substr(valueStartIndex, i - valueStartIndex);
                m_ReadLine = m_ReadLine.c_str() + i + 1;
                return true;
            }

        case '<':
            if (m_ReadLine[i + 1] == '/')
            {
                m_Type = XmlReaderType::EndElement;
                return false;
            }
            break;

        case '>':
            if (0 < i && m_ReadLine[i - 1] == '/')
            {
                m_Type = XmlReaderType::EndElement;
                return false;
            }
            break;
        }
    }

    return false;
}

const char* XmlReader::GetName() const
{
    return m_Name.c_str();
}

const char* XmlReader::GetValue() const
{
    return m_Value.c_str();
}

XmlReaderType XmlReader::GetType() const
{
    return m_Type;
}

void XmlReader::SetName(const string& InName)
{
    m_Name = InName;
    m_Name.erase(remove(m_Name.begin(), m_Name.end(), ' '), m_Name.end());
}
