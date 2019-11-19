#pragma once

namespace util {
	static void hexdump(void* ptr, int buflen) {
		unsigned char* buf = (unsigned char*)ptr;
		int i, j;
		for (i = 0; i < buflen; i += 16) {
			printf("%06x: ", i);
			for (j = 0; j < 16; j++)
				if (i + j < buflen)
					printf("%02x ", buf[i + j]);
				else
					printf("   ");
			printf(" ");
			for (j = 0; j < 16; j++)
				if (i + j < buflen)
					printf("%c", isprint(buf[i + j]) ? buf[i + j] : '.');
			printf("\n");
		}
	}

	char* substr(char* arr, int begin, int len)
	{
		char* res = new char[len];
		for (int i = 0; i < len; i++)
			res[i] = *(arr + begin + i);
		res[len] = 0;
		return res;
	}

	static int GetPortNumber(SOCKET s)
	{
		sockaddr_in sa = { 0 };
		socklen_t sl = sizeof(sa);
		if (getpeername(s, (sockaddr*)&sa, &sl))
			return -1;
		else
			return ntohs(sa.sin_port);
	}
}