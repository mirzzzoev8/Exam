int s = 0;
int a = Convert.ToInt32(Console.ReadLine());
int b = Convert.ToInt32(Console.ReadLine());
void Swap( ref int a,  ref int b)
{
    s = b;
    b = a;
    a = s;
}

Swap( ref a, ref b);
System.Console.WriteLine(b);
System.Console.WriteLine(a);

