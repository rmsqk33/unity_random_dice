#pragma once

enum class XmlReaderType
{
	None,
	Element,
	EndElement,
};

class XmlReader
{
private:
	XmlReaderType m_Type = XmlReaderType::None;

	ifstream* m_ReadStream = nullptr;
	string m_ReadLine;

	string m_Name;
	string m_Value;

public:
	~XmlReader();

	bool Open(const char* InPath);
	void Close();

	bool Read();
	bool MoveToNextAttribute();

	const char* GetName() const;
	const char* GetValue() const;
	XmlReaderType GetType() const;

private:
	void SetName(const string& InName);
};

