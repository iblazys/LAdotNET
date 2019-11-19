#ifndef _SCANNER_H__
#define _SCANNER_H__

class Scanner
{
	private:
		Settings* settings;
		Logger* logger;
		char* Scan(const char* pattern, const char* mask, char* begin, unsigned int size);

	public:

		Scanner(Settings* settings, Logger* logger);

		bool Init();
		bool RunScans();
};

#endif /* _SCANNER_H__ */