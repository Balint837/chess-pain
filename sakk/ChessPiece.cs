using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace sakk
{
    internal abstract partial class ChessPiece
    {
        public Point currentPosition { get; set; }
        public bool isWhite { get; set; }
        
        public string imageSource = "/chess-pain/sakk/images/b_bishop_png_512px.png";

    }


}
