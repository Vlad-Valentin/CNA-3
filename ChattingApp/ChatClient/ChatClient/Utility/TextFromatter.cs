using ChatClient.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ChatClient.Utility
{
    class TextFormatter
    {
        private static char boldSymbol = '*';
        private static char italicSymbol = '_';
        private static char underlineSymbol = '`';
        private static char strikeSymbol = '~';

        public static TextBlock FormatText(ChatMessage message)
        {
            TextBlock formattedText = new TextBlock() { FontFamily = new System.Windows.Media.FontFamily("Times New Roman"), FontSize = 18, Foreground = Brushes.White };
            StringBuilder sectionToFormat = new StringBuilder();
            sectionToFormat.Append(message.Text);
            message.Text = strikeText(sectionToFormat);
            sectionToFormat.Clear();
            formattedText.Text = message.Sender + ": ";
            string textToFormat = message.Text;
            bool isBold = false;
            bool isItalic = false;
            bool isUnderlined = false;

            for (int index = 0; index < textToFormat.Length - 1;)
            {
                StringBuilder currentSection = new StringBuilder();
                currentSection.Append(textToFormat.Substring(index, 2));

                if (currentSection.ToString().Equals(" " + boldSymbol))
                {
                    formattedText.Inlines.Add(new Run(textToFormat[index].ToString()));

                    index += 2;
                    if (index >= textToFormat.Length - 1)
                    {
                        break;
                    }

                    while (index < textToFormat.Length - 1)
                    {
                        sectionToFormat.Append(textToFormat[index]);
                        currentSection.Clear();
                        currentSection.Append(textToFormat.Substring(index, 2));

                        if (currentSection.ToString().Equals(boldSymbol + " "))
                        {
                            sectionToFormat.Remove(sectionToFormat.Length - 1, 1);
                            isBold = true;
                            ++index;
                            break;
                        }
                        else if (textToFormat[index] == boldSymbol)
                        {
                            ++index;
                            break;
                        }
                        ++index;
                    }

                    if (isBold)
                    {
                        formattedText.Inlines.Add(new Bold(new Run(sectionToFormat.ToString())));
                        isBold = false;
                    }
                    else
                    {
                        sectionToFormat.Insert(0, boldSymbol);
                        formattedText.Inlines.Add(new Run(sectionToFormat.ToString()));
                    }

                    sectionToFormat.Clear();
                }
                else if (currentSection.ToString().Equals(" " + italicSymbol))
                {
                    formattedText.Inlines.Add(new Run(textToFormat[index].ToString()));

                    index += 2;
                    if (index >= textToFormat.Length - 1)
                    {
                        break;
                    }

                    while (index < textToFormat.Length - 1)
                    {
                        sectionToFormat.Append(textToFormat[index]);
                        currentSection.Clear();
                        currentSection.Append(textToFormat.Substring(index, 2));

                        if (currentSection.ToString().Equals(italicSymbol + " "))
                        {
                            sectionToFormat.Remove(sectionToFormat.Length - 1, 1);
                            isItalic = true;
                            ++index;
                            break;
                        }
                        else if (textToFormat[index] == italicSymbol)
                        {
                            ++index;
                            break;
                        }
                        ++index;
                    }

                    if (isItalic)
                    {
                        formattedText.Inlines.Add(new Italic(new Run(sectionToFormat.ToString())));
                        isItalic = false;
                    }
                    else
                    {
                        sectionToFormat.Insert(0, italicSymbol);
                        formattedText.Inlines.Add(new Run(sectionToFormat.ToString()));
                    }

                    sectionToFormat.Clear();
                }
                else if (currentSection.ToString().Equals(" " + underlineSymbol))
                {
                    formattedText.Inlines.Add(new Run(textToFormat[index].ToString()));

                    index += 2;
                    if (index >= textToFormat.Length - 1)
                    {
                        break;
                    }

                    while (index < textToFormat.Length - 1)
                    {
                        sectionToFormat.Append(textToFormat[index]);
                        currentSection.Clear();
                        currentSection.Append(textToFormat.Substring(index, 2));

                        if (currentSection.ToString().Equals(underlineSymbol + " "))
                        {
                            sectionToFormat.Remove(sectionToFormat.Length - 1, 1);
                            isUnderlined = true;
                            ++index;
                            break;
                        }
                        else if (textToFormat[index] == underlineSymbol)
                        {
                            ++index;
                            break;
                        }
                        ++index;
                    }

                    if (isUnderlined)
                    {
                        formattedText.Inlines.Add(new Underline(new Run(sectionToFormat.ToString())));
                        isUnderlined = false;
                    }
                    else
                    {
                        sectionToFormat.Insert(0, underlineSymbol);
                        formattedText.Inlines.Add(new Run(sectionToFormat.ToString()));
                    }

                    sectionToFormat.Clear();
                }
                else
                {
                    formattedText.Inlines.Add(new Run(textToFormat[index].ToString()));
                    ++index;
                }
            }
            formattedText.Inlines.Add(new Run(textToFormat.Last().ToString()));
            return formattedText;
        }

        private static string characters = "`~1!2@3#4$5%6^7&8*9(0)-_=+qQwWeErRtTyYuUiIoOpP[{]}\\|aAsSdDfFgGhHjJkKlL;:'\"zZxXcCvVbBnNmM,<.>/? ";
        private static string strikedCharacters = "`̶~̶1̶!̶2̶@̶3̶#̶4̶$̶5̶%̶6̶^̶7̶&̶8̶*̶9̶(̶0̶)̶-̶_̶=̶+̶q̶Q̶w̶W̶e̶E̶r̶R̶t̶T̶y̶Y̶u̶U̶i̶I̶o̶O̶p̶P̶[̶{̶]̶}̶\\̶|̶a̶A̶s̶S̶d̶D̶f̶F̶g̶G̶h̶H̶j̶J̶k̶K̶l̶L̶;̶:̶'̶\"̶z̶Z̶x̶X̶c̶C̶v̶V̶b̶B̶n̶N̶m̶M̶,̶<̶.̶>̶/̶?̶ ̶";
        private static Dictionary<char, string> strikeCharConverter;

        static TextFormatter()
        {
            strikeCharConverter = new Dictionary<char, string>();
            int x = characters.Length;
            int y = strikedCharacters.Length;
            for (int index = 0; index < characters.Length; ++index)
            {
                strikeCharConverter.Add(characters[index], strikedCharacters.Substring(2 * index, 2));
            }
        }

        private static string strikeText(StringBuilder text)
        {
            bool isStriked = false;
            int startingPosition = 0;
            int endingPosition = text.Length - 1;

            for (int index = 0; index < text.Length - 1; ++index)
            {
                if (isStriked)
                {
                    if (text.ToString().Substring(index, 2).Equals(strikeSymbol + " "))
                    {
                        endingPosition = index;
                        for (int index2 = startingPosition; index2 <= endingPosition; index2 += 2)
                        {
                            text.Replace(text[index2].ToString(), strikeCharConverter[text[index2]], index2, 1);
                            ++endingPosition;
                            index += 2;
                        }
                        text.Remove(endingPosition - 1, 2);
                        text.Remove(startingPosition, 2);
                        index -= 2;
                        isStriked = false;
                    }
                }
                else if (text.ToString().Substring(index, 2).Equals(" " + strikeSymbol))
                {
                    startingPosition = ++index;
                    isStriked = true;
                }
            }

            return text.ToString();
        }
    }
}