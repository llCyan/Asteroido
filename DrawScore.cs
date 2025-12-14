using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Asteroido
{
    internal class DrawScore
    {
        int score;
        ScoreLevels scoreLevels;
        public void Draw()
        {
            Raylib_cs.Raylib.DrawText("Score: " + score, 700, 10, 20, Raylib_cs.Color.White);
        }

        public enum ScoreLevels
        {
            Low,
            Medium,
            High
        }

        public void MathScore(ScoreLevels meteorSize)
        {
            score += GetScoreLevel(meteorSize);
            
        }
        public int GetScoreLevel(ScoreLevels whatScore)
        {
            scoreLevels = whatScore;
            if (scoreLevels == ScoreLevels.Low)
            {
                
                return 100;
            }
            else if (scoreLevels == ScoreLevels.Medium)
            {
                return 500;
            }
            else if(scoreLevels == ScoreLevels.High)
            {
                return 1000;
            }
           return 0;
        }
    }
}
