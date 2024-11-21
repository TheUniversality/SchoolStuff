import java.util.Scanner;

public class Class_Grade_Calculator {
	public static int F_GetNumber_RInt(String PAR_String)
	{
		int V_Int_Ammount			= -1;
		Scanner V_Scanner = new Scanner(System.in);

		do
		{
			System.out.print(PAR_String);
			try
			{
				V_Int_Ammount	= V_Scanner.nextInt();
				V_Scanner.nextLine();
			}
			catch (Exception e)
			{
				System.out.println("You must enter whole number");
			}
		}
		while (V_Int_Ammount == -1);

		return V_Int_Ammount;
	}

	public static void main(String[] args)
	{
		Scanner V_Scanner = new Scanner(System.in);

		int V_Int_Ammount				= F_GetNumber_RInt("Enter number how much classes do you want: ");
		int V_IntArr2D[][]				= new int[V_Int_Ammount][];
		float V_FloatArr_GradeAverage[]	= new float[V_Int_Ammount];

		String V_StringArr[]	= new String[V_Int_Ammount];

		for(int I_Index = 0; I_Index < V_IntArr2D.length; I_Index++)
		{
			V_IntArr2D[I_Index]		= new int[F_GetNumber_RInt("Enter number how much numbers of grades do you want: ")];
			System.out.print("Enter the name of class: ");
			V_StringArr[I_Index]	= V_Scanner.next();
			V_Scanner.nextLine();
		}

		for(int I_Index = 0; I_Index < V_IntArr2D.length; I_Index++)
		{
			int V_Int_GrantOfClass	= 0;
			for(int SI_Index = 0; SI_Index < V_IntArr2D[I_Index].length; SI_Index++)
			{
				V_IntArr2D[I_Index][SI_Index]	= F_GetNumber_RInt("Enter the grade for class "+V_StringArr[I_Index]+": ");
				V_Int_GrantOfClass				+= V_IntArr2D[I_Index][SI_Index];
			}

			V_FloatArr_GradeAverage[I_Index]		= V_Int_GrantOfClass/V_IntArr2D[I_Index].length;
		}

		for(int I_Index = 0; I_Index < V_IntArr2D.length; I_Index++)
		{
			System.out.print("Average grade for class "+V_StringArr[I_Index]+" is "+V_FloatArr_GradeAverage[I_Index]+"\n");
		}
	}
}