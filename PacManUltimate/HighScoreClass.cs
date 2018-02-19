using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PacManUltimate
{
    class HighScoreClass
    {
        //Contains two simple functions one for loading and other for saving highscore
        public int LoadHighScore()
        {
            //The actual highscore is saved at the 10th line of the config file
            //it is just necesary to convert it from binary to decimaly number
            int highScore = 0;
            StreamReader sr = new StreamReader("../config.bin");
            for (int i = 0; i < 10; i++)
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

        public void SaveHighScore(int newHighScore)
        {
            //Function generates 9 lines of random binary numbers of aproximately same length as highscore
            //after that the actual highscore is converted to binary number and saved at 10th line
            //at the end another 4 random lines are generated and saved into the file
            StreamWriter sw = new StreamWriter("../config.bin");
            Random rndm = new Random();
            string binaryScore = Convert.ToString(newHighScore, 2);
            for (int i = 0; i < 15; i++)
            {
                if (i == 10)
                    sw.WriteLine(binaryScore);
                else
                    sw.WriteLine(Convert.ToString(rndm.Next(newHighScore - 32, newHighScore + 32), 2));
            }
            sw.Flush();
            sw.Close();
        }
    }
}
