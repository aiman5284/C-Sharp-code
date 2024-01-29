using System;
using System.IO;
using System.Threading;
namespace Game
{
public class Hangman
{
string randomString, userString;
int dataLength;
string Category;
string [] bookData = new string [5];
string [] movieData = new string[5];
int bookCount = 0, movieCount = 0;
public Hangman()
{
FillNameValues ();
}
private void FillNameValues()
{
string firstLine;
StreamReader sRead = new StreamReader("d:\\TextTest.txt");
sRead.BaseStream.Seek(0,  SeekOrigin.Begin);
firstLine=sRead.ReadLine();
while (firstLine != null)
{
if (firstLine.Substring(0, 1) =="B")
{
int stringStartPos = firstLine.IndexOf(':');
bookData [bookCount] = firstLine.Substring(stringStartPos + 1);
bookCount++;
}
else
{
int stringStartPos = firstLine.IndexOf(':');
movieData [movieCount] = firstLine.Substring(stringStartPos + 1);
movieCount++;
}
firstLine = sRead.ReadLine();
}
}
public int AcceptCategory()
{
Console.WriteLine("Enter The Category To Play - Book/ Movie");
Category = Console.ReadLine();
Category = Category.ToUpper();
if (Category != "BOOK" && Category != "MOVIE")
{
Console.WriteLine("Invalid Category...\n");
return 0;
}
else
{
ExtractName();
return 1;
}
}
public void ExtractName()
{
Random RandGen = new Random();
if (Category == "BOOK")
{
int Rnd = RandGen.Next(0, bookCount - 1);
randomString = bookData[Rnd];
}
else
{
int Rnd= RandGen.Next(0, movieCount -1);
randomString = movieData[Rnd];
}
}
public void StartGame()
{
dataLength = randomString.Length;
char locateChar;
int correctCnt = 0, inCorrectCnt = 0;
int i, k;
char[] s = new char[randomString.Length];
InitialiseUserString();
ShowUserInputString();
if(Category == "BOOK")
{
Console.WriteLine("The Total Number Of Characters In The Book: "+randomString.Length);
Console.WriteLine("The Total Number Of Characters You Can Enter To Guess The Name Of Book: {0}",randomString.Length +2);
}
for (i=1, k=0; i<=dataLength +2 || k == dataLength; i++)
{
if (correctCnt == dataLength || inCorrectCnt == dataLength)
break;
Console.WriteLine("Enter The Char ");
locateChar = Convert.ToChar(Console.ReadLine().ToLower());
int foundPos = 0;
int foundChar = 0;
foreach (char c in randomString)
{
if
(c == locateChar)
{
UpdateString(foundPos, locateChar.ToString());
k++;
foundChar =  1;
}
foundPos++;
}
if (foundChar == 0)
{
Console.WriteLine("Better Luck Next Time...!!\n\n");
inCorrectCnt++;
}
else
{
correctCnt++;
}
ShowUserInputString();
Console.WriteLine("Total Correct Attempts: {0}\t",correctCnt);
Console.WriteLine("Total Incorrect Attempts: {0}\n",inCorrectCnt);
if (k == dataLength)
break;
}
if (randomString == userString)
{
Console.WriteLine("You Have Won \n");
}
else
{
Console.WriteLine("The Correct Name Is "+randomString);
Console.WriteLine("You Have Lost \n");
}
}
private void UpdateString(int fPos, string updateStr)
{
string beforeString, afterString;
if (fPos != 0 && fPos != dataLength - 1)
{
if (fPos== 1)
beforeString = userString.Substring(0, 1);
else
beforeString = userString.Substring(0, fPos);
afterString = userString.Substring(fPos +1, dataLength - (fPos + 1));
userString = beforeString + updateStr + afterString;
}
if (fPos == 0)
{
afterString = userString.Substring(fPos +1, dataLength - (fPos + 1));
userString = updateStr + afterString;
}
if(fPos == dataLength - 1)
{
beforeString = userString.Substring(0, fPos);
userString = beforeString + updateStr;
}
}
public void InitialiseUserString()
{
userString = "              ";
for (int i = 0; i< dataLength; i++)
{
userString = userString.Insert (i, "*");
}
}
public void ShowUserInputString()
{
Console.Clear();
Console.WriteLine("Input Value: {0} \n\n", userString);
}
}
class Game
{
static void Main()
{
Console.Clear();
Console.WriteLine("You Have To Complete The Game Within 60 Seconds");
Hangman obj = new Hangman();
int returnVal = obj.AcceptCategory();
if (returnVal == 1)
{
Thread t = new Thread(new ThreadStart(obj.StartGame));
t.Start();
Thread.Sleep(60000);
try
{
t.Abort();
Console.WriteLine("Time Over");
}
catch (ThreadAbortException e) {Console.WriteLine(e.Message);}
}
Console.ReadLine();
}
}
}