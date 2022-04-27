using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsmhTools
{
    public static class StringTool
    {
        public static char[] PersianChars { get; } = { '\u06CC' };
        //public static char[] PersianChars { get; } = { 'ی' };
        public static bool ContainsPersianChars(this string s)
        {
            return s.IndexOfAny(PersianChars) >= 0;
        }
        //============================================================================================
        /// <summary>
        /// Truncates the TextBox.Text property so it will fit in the TextBox. 
        /// </summary>
        static public void Truncate(this TextBox textBox)
        {
            //Determine direction of truncation
            bool direction = false;
            if (textBox.TextAlign == HorizontalAlignment.Right) direction = true;

            //Get text
            string truncatedText = textBox.Text;

            //Truncate text
            truncatedText = truncatedText.Truncate(textBox.Font, textBox.Width, direction);

            //If text truncated
            if (truncatedText != textBox.Text)
            {
                //Set textBox text
                textBox.Text = truncatedText;

                //After setting the text, the cursor position changes. Here we set the location of the cursor manually.
                //First we determine the position, the default value applies to direction = left.

                //This position is when the cursor needs to be behind the last char. (Example:"…My Text|");
                int position = 0;

                //If the truncation direction is to the right the position should be before the ellipsis
                if (!direction)
                {
                    //This position is when the cursor needs to be before the last char (which would be the ellipsis). (Example:"My Text|…");
                    position = 1;
                }

                //Set the cursor position
                textBox.Select(textBox.Text.Length - position, 0);
            }
        }

        /// <summary>
        /// Truncates the string to be smaller than the desired width.
        /// </summary>
        /// <param name="font">The font used to determine the size of the string.</param>
        /// <param name="width">The maximum size the string should be after truncating.</param>
        /// <param name="direction">The direction of the truncation. True for left (…ext), False for right(Tex…).</param>
        static public string Truncate(this string text, Font font, int width, bool direction)
        {
            string truncatedText, returnText;
            int charIndex = 0;
            bool truncated = false;
            //When the user is typing and the truncation happens in a TextChanged event, already typed text could get lost.
            //Example: Imagine that the string "Hello Worl" would truncate if we add 'd'. Depending on the font the output 
            //could be: "Hello Wor…" (notice the 'l' is missing). This is an undesired effect.
            //To prevent this from happening the ellipsis is included in the initial sizecheck.
            //At this point, the direction is not important so we place ellipsis behind the text.
            truncatedText = text + "…";

            //Get the size of the string in pixels.
            SizeF size = MeasureString(truncatedText, font);

            //Do while the string is bigger than the desired width.
            while (size.Width > width)
            {
                //Go to next char
                charIndex++;
                //If the character index is larger than or equal to the length of the text, the truncation is unachievable.
                if (charIndex >= text.Length)
                {
                    //Truncation is unachievable!
                    truncated = true;
                    truncatedText = string.Empty;
                    //Throw exception so the user knows what's going on.
                    string msg = "The desired width of the string is too small to truncate to.";
                    Console.WriteLine(msg);
                    break;
                    //throw new IndexOutOfRangeException(msg);
                }
                else
                {
                    //Truncation is still applicable!
                    //Raise the flag, indicating that text is truncated.
                    truncated = true;
                    //Check which way to text should be truncated to, then remove one char and add an ellipsis.
                    if (direction)
                    {
                        //Truncate to the left. Add ellipsis and remove from the left.
                        truncatedText = "…" + text.Substring(charIndex);
                    }
                    else
                    {
                        //Truncate to the right. Remove from the right and add the ellipsis.
                        truncatedText = text.Substring(0, text.Length - charIndex) + "…";
                    }

                    //Measure the string again.
                    size = MeasureString(truncatedText, font);
                }
            }
            //If the text got truncated, change the return value to the truncated text.
            if (truncated) returnText = truncatedText;
            else returnText = text;
            //Return the desired text.
            return returnText;
        }

        /// <summary>
        /// Measures the size of this string object.
        /// </summary>
        /// <param name="text">The string that will be measured.</param>
        /// <param name="font">The font that will be used to measure to size of the string.</param>
        /// <returns>A SizeF object containing the height and size of the string.</returns>
        static private SizeF MeasureString(string text, Font font)
        {
            //To measure the string we use the Graphics.MeasureString function, which is a method that can be called from a PaintEventArgs instance.
            //To call the constructor of the PaintEventArgs class, we must pass a Graphics object. We'll use a PictureBox object to achieve this. 
            PictureBox pb = new();

            //Create the PaintEventArgs with the correct parameters.
            PaintEventArgs pea = new(pb.CreateGraphics(), new Rectangle());
            pea.Graphics.PageUnit = GraphicsUnit.Pixel;
            pea.Graphics.PageScale = 1;

            //Call the MeasureString method. This methods calculates what the height and width of a string would be, given the specified font.
            SizeF size = pea.Graphics.MeasureString(text, font);

            //Return the SizeF object.
            return size;
        }
    }
}
