using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PacManUltimate
{
    /// <summary>
    /// Contains two simple functions one for loading and other for saving highscore.
    /// </summary>
    class HighScoreClass
    {
        const int ImportantLine = 10;
        const int LinesInFile = 15;

        /// <summary>
        /// Function for HighScore loading.
        /// </summary>
        /// <returns></returns>
        public int LoadHighScore()
        {
            // The actual highscore is saved at the 10th line of the config file.
            // It is necesary to just convert it from binary to decimaly number.
            int highScore = 0;
            StreamReader sr = new StreamReader("../config.bin");
            for (int i = 0; i < ImportantLine; i++)
            {
                if (sr.EndOfStream)
                    return 0;
                sr.ReadLine();
            }
            if (sr.EndOfStream)
                return 0;
            highScore = Convert.ToInt32(sr.ReadLine(), 2);
            return highScore;
        }

        /// <summary>
        /// Function for HighScore Saving.
        /// </summary>
        /// <param name="newHighScore">New HighScore to be saved to config file.</param>
        public void SaveHighScore(int newHighScore)
        {
            // Function generates 9 lines of random binary numbers of aproximately same length as highscore.
            // After that the actual highscore is converted to binary number and saved at 10th line.
            // Another 4 lines of random binary data are generated and saved at the end of the file.
            StreamWriter sw = new StreamWriter("../config.bin");
            Random rndm = new Random();
            string binaryScore = Convert.ToString(newHighScore, 2);
            for (int i = 0; i < LinesInFile; i++)
            {
                if (i == ImportantLine)
                    sw.WriteLine(binaryScore);
                else
                    sw.WriteLine(Convert.ToString(rndm.Next(newHighScore - 32, newHighScore + 32), 2));
            }
            sw.Flush();
            sw.Close();
        }
    }
}
