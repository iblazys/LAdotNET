#include "pch.h"
#include "Counter.h"

Counter::Counter()
{
	Count = 0;
	MaxCount = 255;
}

void Counter::Increase() 
{
	_lock.lock();

	if (Count == MaxCount)
	{
		Count = 0;
	}
	else
	{
		Count++;
	}

	_lock.unlock();
}

void Counter::Reset() 
{
	Count = 0;
}

int Counter::getCount()
{
	return Count;
}