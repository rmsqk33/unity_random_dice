#pragma once

#define _WINSOCK_DEPRECATED_NO_WARNINGS

#include <WinSock2.h>
#include <thread>
#include <iostream>
#include <vector>
#include <map>
#include <functional>
#include <typeinfo>
#include <mutex>
#include <algorithm>

using namespace std;

#include "FEnum.h"
#include "Packet.h"

#include "FUtil.h"
#include "FDataCenter.h"

#pragma comment(lib, "ws2_32")



#define PORT 5642
#define MAX_PACKET_SIZE 10240

#define PRESET_MAX 5
#define DICE_PRESET_SLOT_MAX 5
#define DICE_ID_NONE 0

extern mutex gThreadLock;

