#pragma once

#include <Windows.h>

class IntSaver
{
public:
  IntSaver()
    : mValue(0) {
    InitializeCriticalSection(&mCS);
  }
  ~IntSaver()
  {
    DeleteCriticalSection(&mCS);
  }
public:
  void SetValue(int aValue)
  {
    EnterCriticalSection(&mCS);
    mValue = aValue;
    LeaveCriticalSection(&mCS);
  }
  void GetValue(int &aValue)
  {
    EnterCriticalSection(&mCS);
    aValue = mValue;
    LeaveCriticalSection(&mCS);
  }
private:
  int mValue;
  CRITICAL_SECTION mCS;
};
