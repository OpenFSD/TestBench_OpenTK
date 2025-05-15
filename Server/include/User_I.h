#pragma once
#include "Praise0_Input.h"
#include "Praise1_Input.h"
#include "Praise2_Input.h"

namespace Server_Library
{
	class User_I
	{
	public:
		User_I();
		virtual ~User_I();
		class Praise0_Input* Get_Praise0_Input();
		class Praise1_Input* Get_Praise1_Input();
		class Praise2_Input* Get_Praise2_Input();

	protected:

	private:
		static class Praise0_Input* ptr_Praise0_Input;
		static class Praise1_Input* ptr_Praise1_Input;
		static class Praise2_Input* ptr_Praise2_Input;

	};
}