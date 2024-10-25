let V_String_LocalStorage	= window.localStorage.getItem("TSK_Task");

if(V_String_LocalStorage	== null)
{
	window.localStorage.setItem("TSK_Task", JSON.stringify({}));
}

let V_JSON_LocalStorage		= JSON.parse(V_String_LocalStorage);

console.warn(V_JSON_LocalStorage);

const V_TS_TaskNameInput	= document.getElementById("ID_InpTaskName");
const V_TS_BtnAddTask		= document.getElementById("ID_BtnAddTask");

function F_SaveToLStorage(PAR_TSName, PAR_TSFinished)
{
	V_JSON_LocalStorage[PAR_TSName]	= PAR_TSFinished;

	window.localStorage.setItem("TSK_Task", JSON.stringify(V_JSON_LocalStorage));

	console.warn("INDEX: Save call: "+JSON.stringify(V_JSON_LocalStorage));
}


function F_CheckFinishState_RNil(PAR_TSName, PAR_ElementDIV, PAR_ElementChB)
{
	if(PAR_ElementChB.checked == true)
	{
		PAR_ElementDIV.style.backgroundColor	= "green";
		PAR_ElementDIV.style.gridColumn			= 2;
	}
	else
	{
		PAR_ElementDIV.style.backgroundColor	= "red";
		PAR_ElementDIV.style.gridColumn			= 1;
	}

	F_SaveToLStorage(PAR_TSName, PAR_ElementChB.checked);
}

function F_DeleteTask(PAR_TSName, PAR_ElementDIV)
{
	delete V_JSON_LocalStorage[PAR_TSName];

	window.localStorage.setItem("TSK_Task", JSON.stringify(V_JSON_LocalStorage));

	PAR_ElementDIV.remove();

	console.warn("INDEX: Delete call: "+JSON.stringify(V_JSON_LocalStorage));
}

// PAR_BySys	- bool (true - loaded tasks from LocalStorage) (false, added)
// PAR_KeyJSON	- if PAR_BySys is true holds access key to the JSON within the LocalStorage
function F_AddTask_RNil(PAR_BySys, PAR_KeyJSON)
{
	const New_TS_Div	= document.createElement("div");
	const New_TS_Ch		= document.createElement("input");
	const New_TS_TSName	= document.createElement("p");
	const New_TS_Delete	= document.createElement("button");

	New_TS_Div.className		= "Class_Task";
	New_TS_Div.style.display	= "flex";

	New_TS_Ch.type				= "CheckBox";
	New_TS_Ch.className			= "Class_CH";

	New_TS_TSName.className		= "Class_TaskName";

	New_TS_Delete.textContent	= "Delete";

	New_TS_Div.style.gridColumn			= 1;
	New_TS_Div.style.backgroundColor	= "red";

	if(PAR_BySys	== true)
	{
		New_TS_TSName.textContent			= PAR_KeyJSON;

		New_TS_Ch.checked					= V_JSON_LocalStorage[PAR_KeyJSON]

		if(V_JSON_LocalStorage[PAR_KeyJSON]	== true)
		{
			New_TS_Div.style.backgroundColor	= "green";
			New_TS_Div.style.gridColumn			= 2;
		}
	}
	else
	{
		New_TS_TSName.textContent	= V_TS_TaskNameInput.value;
	}

	New_TS_Div.appendChild(New_TS_TSName);
	New_TS_Div.appendChild(New_TS_Ch);
	New_TS_Div.appendChild(New_TS_Delete);
	document.getElementById("ID_DivBoard").appendChild(New_TS_Div);

	F_SaveToLStorage(New_TS_TSName.textContent, New_TS_Ch.checked)

	New_TS_Ch.addEventListener("change", ()		=> F_CheckFinishState_RNil(New_TS_TSName.textContent, New_TS_Div, New_TS_Ch));

	New_TS_Delete.addEventListener("click", ()	=> F_DeleteTask(New_TS_TSName.textContent, New_TS_Div));
}

if(V_JSON_LocalStorage	!= null)
{
	for(const I_Key in V_JSON_LocalStorage)
	{
		F_AddTask_RNil(true, I_Key);
	}
}

document.onkeydown	= function (E_Data)
{
	if(E_Data.key	== "Enter")
	{
		F_AddTask_RNil(false);
	}
}


V_TS_BtnAddTask.addEventListener("click", () => F_AddTask_RNil(false));