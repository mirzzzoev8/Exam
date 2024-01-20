int cnt = 0;
int SumOfDigits(int n)
{    
    for (int i = n; i > 0; i/=10)
    {
        cnt+=i%10;
    }
    return cnt;    
}
int n = Convert.ToInt32(Console.ReadLine());

System.Console.WriteLine("The sum of the digits of the number 1234 is : " + SumOfDigits(n));