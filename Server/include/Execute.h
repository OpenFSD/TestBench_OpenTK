#pragma once
#include <array>
#include <thread>
#include "ConcurrentQue_Server_Framework.h"
#include "Execute_Control.h"
#include "Framework_Server.h"
#include "Global.h"
#include "WriteEnable_Stack_Server_InputAction_Framework.h"
#include "WriteEnable_Stack_Server_OutputRecieve_Framework.h"

namespace Server_Library
{
    class Execute
    {
    public:
        Execute(
            class Global* ptr_Global,
            __int8 number_Implemented_Cores
        );
        virtual ~Execute();
        void Initialise();
        void Initialise_Control(
            __int8 number_Implemented_Cores,
            class Global* ptr_Global
        );
        void Initialise_Threads();

        class Execute_Control* Get_Execute_Control();
        class ConcurrentQue::ConcurrentQue_Server_Framework* Get_LaunchConcurrency_ServerSide();
        class WaitEnableWrite::WriteEnable_Stack_Server_InputAction_Framework* Get_WriteEnable_Stack_Server_InputPraise();
        class WaitEnableWrite::WriteEnable_Stack_Server_OutputRecieve_Framework* Get_WriteEnable_Stack_Server_OutputPraise();

    protected:

    private:
        static class Execute_Control* ptr_Execute_Control;
        static class ConcurrentQue::ConcurrentQue_Server_Framework* ptr_LaunchConcurrency_ServerSide;
        static std::thread* ptr_Thread_WithCoreId[4];//NUMBER OF CORES
        static class WaitEnableWrite::WriteEnable_Stack_Server_InputAction_Framework* ptr_WriteEnable_Stack_Server_InputPraise;
        static class WaitEnableWrite::WriteEnable_Stack_Server_OutputRecieve_Framework* ptr_WriteEnable_Stack_Server_OutputPraise;
    };
}