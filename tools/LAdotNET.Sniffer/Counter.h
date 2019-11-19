#pragma once
class Counter
{
	public:
		Counter();
		void Increase();
		void Reset();

		int getCount();

	private:
		int Count;
		unsigned char MaxCount;
		std::mutex _lock;
};

