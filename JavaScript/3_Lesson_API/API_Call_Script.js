const V_HTMLUI_SubmBut	= document.getElementById("ID_Submit");

const V_HTMLUI_SNSFW	= document.getElementById("ID_SNSFWOption");
const V_HTMLUI_CatSFW	= document.getElementById("ID_SFWCategory");
const V_HTMLUI_CatNSFW	= document.getElementById("ID_NSFWCategory");

const V_HTMLUI_ImgOut	= document.getElementById("ID_ImgOut");
const V_HTMLUI_ImgSrc	= document.getElementById("ID_ImgSrc");
const V_HTMLUI_TxtOut	= document.getElementById("ID_TextOut");
const V_HTMLUI_DomCol	= document.getElementById("ID_DomColor");

function F_GetPicture_RNil()
{
	let V_String_CatOpt;
	if(V_HTMLUI_SNSFW.value	== "sfw")
	{
		V_String_CatOpt		= V_HTMLUI_CatSFW.value;
	}
	else
	{
		V_String_CatOpt		= V_HTMLUI_CatNSFW.value;
	}

	fetch(`https://api.waifu.pics/${V_HTMLUI_SNSFW.value}/${V_String_CatOpt}`)
	.then(Rec_ImgResponse	=> Rec_ImgResponse.json())
	.then(Rec_ImgData		=> 
	{
		V_HTMLUI_ImgOut.src		= Rec_ImgData.url;
	})

	fetch("https://api.waifu.im/search?q=V_HTMLUI_ImgOut.src")
	.then(Rec_Response	=> Rec_Response.json())
	.then(Rec_Data		=> 
	{
		V_HTMLUI_DomCol.style.backgroundColor	= Rec_Data["images"][0]["dominant_color"];
		V_HTMLUI_ImgSrc.textContent	= "Image sources: "+Rec_Data["images"][0]["source"];
		V_HTMLUI_ImgSrc.href		= Rec_Data["images"][0]["source"];
		V_HTMLUI_TxtOut.textContent	=
		`Uploaded at date: ${Rec_Data["images"][0]["uploaded_at"]} \r\n 
			Tag \r\t: Descriptions: (If avaliable) \r\n
		`;

		for(let I_Index = 0; I_Index < Rec_Data["images"][0]["tags"]["length"]; I_Index++)
		{
			V_HTMLUI_TxtOut.textContent	+= `${Rec_Data["images"][0]["tags"][I_Index]["name"]}\r\t: ${Rec_Data["images"][0]["tags"][I_Index]["description"]} \r\n`
		}
		
	})
}

function F_ShowCatBase_RNil()
{
	if(V_HTMLUI_SNSFW.value	== "sfw")
	{
		V_HTMLUI_CatSFW.style.opacity	= 100;
		V_HTMLUI_CatNSFW.style.opacity	= 0;

		return;
	}

	V_HTMLUI_CatSFW.style.opacity	= 0;
	V_HTMLUI_CatNSFW.style.opacity	= 100;
}

V_HTMLUI_SNSFW.addEventListener("change", F_ShowCatBase_RNil)
V_HTMLUI_SubmBut.addEventListener("click", F_GetPicture_RNil)