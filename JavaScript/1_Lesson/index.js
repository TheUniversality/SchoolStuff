let V_Array_String	= ["Volnost", "Hehe", "Hihi", "Skibidi", "No", "Yes", "Help", "Turbulin", "Výrůstek", "Skrček"];

const V_HTMLE_Button1	= document.getElementById("ID_1");
const V_HTMLE_Button2	= document.getElementById("ID_2");
const V_HTMLE_Button4	= document.getElementById("ID_3");
const V_HTMLE_Button3	= document.getElementById("ID_4");

const V_HTMLE_P_W		= document.getElementById("ID_Words");
const V_HTMLE_P_O		= document.getElementById("ID_Output");


V_HTMLE_Button1.addEventListener("click", F_Process1_RNil);
V_HTMLE_Button2.addEventListener("click", F_Process2_RNil);
V_HTMLE_Button4.addEventListener("click", F_Process3_RNil);
V_HTMLE_Button3.addEventListener("click", F_Process4_RNil);


V_HTMLE_P_W.textContent	= V_Array_String;

console.log(V_Array_String);

function F_Process1_RNil()
{
	V_Array_String.sort();

	console.info(V_Array_String);

	V_HTMLE_P_O.textContent	= V_Array_String;
}

function F_Process2_RNil()
{
	V_Array_String.sort((V_E1, V_E2) => V_E1.length - V_E2.length);

	console.info(V_Array_String);

	V_HTMLE_P_O.textContent	= V_Array_String;
}

function F_Process3_RNil()
{
	for(I_Index = 0; I_Index < V_Array_String.length; I_Index++)
	{
		const V_RandomN	= Math.floor(Math.random() * V_Array_String.length-1) + 1;

		const V_Temp	= V_Array_String[V_RandomN];

		V_Array_String[V_RandomN]	= V_Array_String[I_Index];
		V_Array_String[I_Index]		= V_Temp;
	}

	console.info(V_Array_String);

	V_HTMLE_P_O.textContent	= V_Array_String;
}


// Shiftes array (First element will switch position with the last element, second first with the last first and yady yady yada...)
function F_Process4_RNil()
{
	for(I_Index = 0; I_Index < V_Array_String.length/2; I_Index++)
	{
		const V_Int_FromBack	= V_Array_String.length-I_Index-1

		const I_Element			= V_Array_String[I_Index];
		const V_Temp			= V_Array_String[V_Int_FromBack];

		V_Array_String[I_Index]			= V_Temp;
		V_Array_String[V_Int_FromBack]	= I_Element;
	};

	console.info(V_Array_String);

	V_HTMLE_P_O.textContent	= V_Array_String;
}