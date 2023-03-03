using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace sakk
{
    internal class TestPiece : ChessPiece
    {
        public TestPiece()
        {
            this.currentPosition = new Point(1, 1);
            this.isWhite = true;
            Console.WriteLine(this.currentPosition);
            Console.WriteLine(this.isWhite);
            Console.ReadLine();

        }
    


        
        
    }
}
