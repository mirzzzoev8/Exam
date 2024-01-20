string a = Console.ReadLine();
int b = Convert.ToInt32(Console.ReadLine());

string Takror(string a, int b)
{
    if(b == 1) return a;
    else return a + Takror(a, b - 1);
    
}
System.Console.WriteLine(Takror(a,b));